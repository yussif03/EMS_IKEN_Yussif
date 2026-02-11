namespace EMS.API.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ManagerId { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
