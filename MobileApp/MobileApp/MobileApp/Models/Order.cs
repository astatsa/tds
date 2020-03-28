using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileApp.Models
{
    class Order : BaseModel
    {
        public DateTime Date { get; set; }
        public int SupplierId { get; set; }
        public Counterparty Supplier { get; set; }
        public int CustomerId { get; set; }
        public Counterparty Customer { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        public int DriverId { get; set; }
        public Employee Driver { get; set; }
        public double Volume { get; set; }
        public DateTime DateCreate { get; set; }
        public OrderState OrderState { get; set; }
    }
}
