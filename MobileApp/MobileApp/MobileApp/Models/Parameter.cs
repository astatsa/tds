namespace MobileApp.Models
{
    class Parameter
    {
        public Parameter(string name, object value)
        {
            this.Name = name;
            this.Value = value;
            this.TypeName = value?.GetType()?.AssemblyQualifiedName;
        }

        public Parameter(object value) : this("", value)
        {

        }

        public Parameter() : this(null)
        {

        }

        public string Name { get; set; }
        public string TypeName { get; set; }
        public object Value { get; set; }
    }
}
