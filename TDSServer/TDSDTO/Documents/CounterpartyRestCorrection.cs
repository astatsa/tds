using System;
using System.Collections.Generic;
using System.Text;

namespace TDSDTO.Documents
{
    public class CounterpartyRestCorrection : BaseModel
    {
        public DateTime Date { get; set; }
        public int CounterpartyId { get; set; }
        public int UserId { get; set; }
        public ICollection<CounterpartyRestCorrectionMaterial> MaterialCorrections { get; set; }
    }
}
