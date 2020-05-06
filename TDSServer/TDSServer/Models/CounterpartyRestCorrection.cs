using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public class CounterpartyRestCorrection : DocumentBaseModel
    {
        public CounterpartyRestCorrection() : base(2)
        {
        }

        public DateTime Date { get; set; }
        public int CounterpartyId { get; set; }
        [ForeignKey("CounterpartyId")]
        public Counterparty Counterparty { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public ICollection<CounterpartyRestCorrectionMaterial> MaterialCorrections { get; set; }
    }
}
