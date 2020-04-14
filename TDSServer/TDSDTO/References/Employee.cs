using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace TDSDTO.References
{
    public class Employee : BaseModel
    {
        [DisplayName("Наименование")]
        public string Name { get; set; }
        [DisplayName("ФИО")]
        public string FullName { get; set; }
        public int? UserId { get; set; }
        [DisplayName("Имя пользователя")]
        public string UserName { get; set; }
        public int? PositionId { get; set; }
        [DisplayName("Должность")]
        public string PositionName { get; set; }
    }
}
