using System;
using System.ComponentModel;

namespace TDSDTO.Documents
{
    public class Order : BaseModel
    {
        [DisplayFormat("Дата", "dd.MM.yyyy")]
        public DateTime Date { get; set; }
        [DisplayFormat("Номер")]
        public int Number { get; set; }
        public int SupplierId { get; set; }
        [DisplayFormat("Поставщик")]
        public string SupplierName { get; set; }
        public int CustomerId { get; set; }
        [DisplayFormat("Покупатель")]
        public string CustomerName { get; set; }
        public int MaterialId { get; set; }
        [DisplayFormat("Материал")]
        public string MaterialName { get; set; }
        public int DriverId { get; set; }
        [DisplayFormat("Водитель")]
        public string DriverName { get; set; }
        [DisplayFormat("Объем", horizontalAlignment: Alignments.Right)]
        public double Volume { get; set; }
        public DateTime DateCreate { get; set; }
        public int OrderStateId { get; set; }
        [DisplayFormat("Статус")]
        public string OrderStateName { get; set; }
    }
}
