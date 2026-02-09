using Dapper;
using EMS.API.Data.Interface;
using EMS.API.Models;
using EMS.API.Repositories.Interface;

namespace EMS.API.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public EmployeeRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> AddAsync(Employee employee)
        {
            var sql = @"
                INSERT INTO Employee (Name, Email, DepartmentId)
                VALUES (@Name, @Email, @DepartmentId);
                SELECT CAST(SCOPE_IDENTITY() as int);
            ";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleAsync<int>(sql, employee); // dapper automatically inserts the data in database
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var sql = "SELECT * FROM Employee";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Employee>(sql);
        }
    }
}
