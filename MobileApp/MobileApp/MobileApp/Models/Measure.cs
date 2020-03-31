namespace MobileApp.Models
{
    class Measure : BaseModel
    {
        public string Name { get; set; }
        public string FullName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
