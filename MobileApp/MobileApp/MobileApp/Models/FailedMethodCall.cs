using SQLite;

namespace MobileApp.Models
{
    [Table("FailedCalls")]
    class FailedMethodCall
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string MethodName { get; set; }
        public string Parameters { get; set; }
    }
}
