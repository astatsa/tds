using System;
using System.Collections.Generic;
using System.Text;

namespace TDSDTO.Documents
{
    public class CounterpartyRestCorrectionMaterial
    {
        public int Id { get; set; }
        public int CounterpartyRestCorrectionId { get; set; }
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public double Correction { get; set; }
    }
}
