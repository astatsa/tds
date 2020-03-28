namespace MobileApp.Models
{
    public class Counterparty : BaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public CounterpartyType Type { get; set; }
    }
}
