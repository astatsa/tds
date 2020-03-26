using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public class PositionRole
    {
        public int PositionId { get; set; }
        public Position Position { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
