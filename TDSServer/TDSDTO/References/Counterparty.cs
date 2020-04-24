namespace TDSDTO.References
{
    public class Counterparty : BaseModel
    {
        [DisplayFormat("Наименование")]
        public string Name { get; set; }
        [DisplayFormat("Адрес")]
        public string Address { get; set; }
        [DisplayFormat("Описание")]
        public string Description { get; set; }
        [DisplayFormat("Тип")]
        public string TypeName { get; set; }
        public bool IsSupplier { get; set; }
    }
}
