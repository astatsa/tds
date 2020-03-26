using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public class Employee : BaseModel
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public int? PositionId { get; set; }
        public Position Position { get; set; }
    }
}
