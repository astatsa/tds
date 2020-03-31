namespace MobileApp.Models
{
    class Material : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Measure Measure { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
