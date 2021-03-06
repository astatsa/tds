﻿using System.Collections.Generic;

namespace TDSServer.Models
{
    public class Role : BaseModel
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<PositionRole> PositionRoles { get; set; }
    }
}
