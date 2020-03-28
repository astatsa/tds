using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public class Counterparty : BaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        [Column("CounterpartyTypeId")]
        public int TypeId { get; set; }
        public CounterpartyType Type { get; set; }
    }
}
