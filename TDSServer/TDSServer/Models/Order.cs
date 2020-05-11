using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public class Order : DocumentBaseModel
    {
        public Order() : base(1)
        {
        }

        public DateTime Date { get; set; }
        public int Number { get; set; }
        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public Counterparty Supplier { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Counterparty Customer { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        public int DriverId { get; set; }
        [ForeignKey("DriverId")]
        public Employee Driver { get; set; }
        public double Volume { get; set; }
        public DateTime DateCreate { get; set; }
        public int OrderStateId { get; set; }
        [ForeignKey("OrderStateId")]
        public OrderState OrderState { get; set; }
        public double LoadedVolume { get; set; }
        public double UnloadedVolume { get; set; }
        public DateTime? LoadedDate { get; set; }
        public DateTime? UnloadedDate { get; set; }
    }
}
