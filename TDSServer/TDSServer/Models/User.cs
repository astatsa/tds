using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TDSServer.Models
{
    public class User : BaseModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public ICollection<UserRole> UserRoles { get; set; }
        public Employee Employee { get; set; }
    }
}
