using System.ComponentModel.DataAnnotations.Schema;

namespace TDSServer.Models
{
    public class CounterpartyMaterialMvt : RegisterBaseModel
    {
        public int CounterpartyId { get; set; }
        [ForeignKey("CounterpartyId")]
        public Counterparty Counterparty { get; set; }
        public int MaterialId { get; set; }
        [ForeignKey("MaterialId")]
        public Material Material { get; set; }
        public bool IsComing { get; set; }
        public double Quantity { get; set; }
    }
}
