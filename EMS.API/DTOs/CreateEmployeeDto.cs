using EMS.API.Enums;

namespace EMS.API.DTOs
{
    public class CreateEmployeeDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
        // Foreign Keys
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
    }
}
