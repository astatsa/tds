using System;
using System.ComponentModel;

namespace TDSDTO.Documents
{
    public class Order : BaseModel
    {
        [DisplayName("Дата")]
        public DateTime Date { get; set; }
        [DisplayName("Номер")]
        public int Number { get; set; }
        public int SupplierId { get; set; }
        [DisplayName("Поставщик")]
        public string SupplierName { get; set; }
        public int CustomerId { get; set; }
        [DisplayName("Покупатель")]
        public string CustomerName { get; set; }
        public int MaterialId { get; set; }
        [DisplayName("Материал")]
        public string MaterialName { get; set; }
        public int DriverId { get; set; }
        [DisplayName("Водитель")]
        public string DriverName { get; set; }
        [DisplayName("Объем")]
        public double Volume { get; set; }
        public DateTime DateCreate { get; set; }
        public int OrderStateId { get; set; }
        [DisplayName("Статус")]
        public string OrderStateName { get; set; }
    }
}
