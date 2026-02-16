namespace EMS.API.DTOs
{
    public class CreatePositionDto
    {
        public string Title { get; set; } = string.Empty;
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
    }
}
