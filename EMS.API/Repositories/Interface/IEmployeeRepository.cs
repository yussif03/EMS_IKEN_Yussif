using EMS.API.Models;

namespace EMS.API.Repositories.Interface
{
    public interface IEmployeeRepository
    {
        Task<int> AddAsync(Employee employee);
        Task<IEnumerable<Employee>> GetAllAsync();
    }
}
