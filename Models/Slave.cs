namespace _7semester_ASP_SecondTask.Models
{
    public class Slave
    {
        public int Id { get; set; }
        public int? Price { get; set; }
        public int? Age { get; set; }
        public string? SkinTone { get; set; }
        public int? MasterId { get; set; }
        public Master? Master { get; set; }
    }
}