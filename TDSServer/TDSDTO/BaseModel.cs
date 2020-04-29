using System;
using System.Collections.Generic;
using System.Text;

namespace TDSDTO
{
    public class BaseModel
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;

            if (!(obj is BaseModel bm)) return false;

            return this.Id == bm.Id;
        }

        public override int GetHashCode()
        {
            int hashCode = -1513014534;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            return hashCode;
        }
    }
}
