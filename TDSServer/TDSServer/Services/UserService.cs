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

        public string Authenticate(string username, string password)
        {
            string passwordHash = GetPasswordHash(password);
            var user = dbContext.Users
                .Include(u => u.Role)
                .SingleOrDefault(u => u.PasswordHash == passwordHash);
            if(user == null)
            {
                return null;
            }

            byte[] key = Encoding.UTF8.GetBytes(secureSettings.SecretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });
            return tokenHandler.WriteToken(token);
        }

        public static string GetPasswordHash(string password)
        {
            using var sha = SHA256.Create();
            return Encoding.UTF8.GetString(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}
