using System.Collections.Generic;

namespace TDSServer.Models
{
    public class User : BaseModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
