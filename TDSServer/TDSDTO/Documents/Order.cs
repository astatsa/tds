using System;
using System.ComponentModel;
using TDSDTO.References;

namespace TDSDTO.Documents
{
    public class Order : BaseModel
    {
        [DisplayFormat("Дата", "dd.MM.yyyy")]
        public DateTime? Date { get; set; }
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
        [DisplayFormat("Вес заявки", horizontalAlignment: Alignments.Right)]
        public double Volume { get; set; }
        [DisplayFormat("Вес(загрузка)", horizontalAlignment: Alignments.Right)]
        public double LoadedVolume { get; set; }
        [DisplayFormat("Время загрузки", "dd.MM.yyyy HH:mm")]
        public DateTime? LoadedDate { get; set; }
        [DisplayFormat("Вес(разгрузка)", horizontalAlignment: Alignments.Right)]
        public double UnloadedVolume { get; set; }
        [DisplayFormat("Время разгрузки", "dd.MM.yyyy HH:mm")]
        public DateTime? UnloadedDate { get; set; }
        public DateTime DateCreate { get; set; }
        public int OrderStateId { get; set; }
        [DisplayFormat("Статус")]
        public string OrderStateName { get; set; }
        public OrderStates OrderState { get; set; }
    }

    public enum OrderStates
    {
        New,
        Viewed,
        Loaded,
        Completed
    }
}
