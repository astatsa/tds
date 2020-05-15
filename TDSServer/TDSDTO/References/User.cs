using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TDSDTO.References
{
    public class User : BaseModel
    {
        [DisplayFormat("Пользователь")]
        public string Username { get; set; }
        [DisplayFormat("Полное наименование")]
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
