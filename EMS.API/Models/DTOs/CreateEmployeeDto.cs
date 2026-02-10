namespace EMS.API.Models.DTOs
{
    public class CreateEmployeeDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public bool isDeleted { get; set; } = false;
        // Foreign Keys
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
    }
}
