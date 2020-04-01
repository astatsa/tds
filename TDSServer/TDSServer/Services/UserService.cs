using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TDSServer.Helpers;
using TDSServer.Models;

namespace TDSServer.Services
{
    public class UserService : IUserService
    {
        private readonly SecureSettings secureSettings;
        private readonly AppDbContext dbContext;
        public UserService(IOptions<SecureSettings> secureSettings, AppDbContext dbContext)
        {
            this.secureSettings = secureSettings.Value;
            this.dbContext = dbContext;
        }

        public async Task<(User User, string Token)> AuthenticateAsync(string username, string password)
        {
            string passwordHash = GetPasswordHash(password);
            var user = await dbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(x => x.Role)
                .ThenInclude(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
                .Include(x => x.Employee)
                .SingleOrDefaultAsync(u => u.Username == username && u.PasswordHash == passwordHash);
            if(user == null)
            {
                return (null, null);
            }

            byte[] key = Encoding.ASCII.GetBytes(secureSettings.SecretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }
                .Concat(
                    user.UserRoles?
                    .SelectMany(
                        x => x.Role.RolePermissions?
                        .Select(
                            q => new Claim(ClaimTypes.Role, q.Permission.Name)
                         )
                    )
                )),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });
            return (user, tokenHandler.WriteToken(token));
        }

        public static string GetPasswordHash(string password)
        {
            using var sha = SHA256.Create();
            return Encoding.UTF8.GetString(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}
