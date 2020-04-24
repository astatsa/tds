namespace TDSDTO.References
{
    public class Employee : BaseModel
    {
        [DisplayFormat("Наименование")]
        public string Name { get; set; }
        [DisplayFormat("ФИО")]
        public string FullName { get; set; }
        public int? UserId { get; set; }
        [DisplayFormat("Имя пользователя")]
        public string UserName { get; set; }
        public int? PositionId { get; set; }
        [DisplayFormat("Должность")]
        public string PositionName { get; set; }
    }
}
