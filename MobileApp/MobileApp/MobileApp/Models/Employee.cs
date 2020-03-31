using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Models
{
    class Employee : BaseModel
    {
        public string Name { get; set; }
        public string FullName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
