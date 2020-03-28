using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public enum CounterpartyTypes
    {
        Supplier,
        Customer
    }
    public class CounterpartyType : BaseModel
    {
        public CounterpartyTypes Name { get; set; }
        public string FullName { get; set; }
        public ICollection<Counterparty> Counterparty { get; set; }
    }
}
