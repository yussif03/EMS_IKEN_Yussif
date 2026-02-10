namespace EMS.API.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
        public bool IsDeleted { get; set; } = false;
        // Foreign Keys
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
    }
}
