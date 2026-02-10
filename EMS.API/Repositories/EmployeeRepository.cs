using Dapper;
using EMS.API.Data.Interface;
using EMS.API.Models;
using EMS.API.Repositories.Base;

public class EmployeeRepository
    : Repository<Employee>
{
    public EmployeeRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    protected override string TableName => "Employee";

    public override async Task<int> AddAsync(Employee employee)
    {
        const string sql = @"
            INSERT INTO Employee
            (FullName, Email, PhoneNumber, HireDate, Salary, IsDeleted, DepartmentId, PositionId)
            VALUES
            (@FullName, @Email, @PhoneNumber, @HireDate, @Salary, @IsDeleted, @DepartmentId, @PositionId);
            SELECT CAST(SCOPE_IDENTITY() AS int);
        ";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, employee);
    }

    public override async Task<bool> UpdateAsync(Employee employee)
    {
        const string sql = @"
            UPDATE Employee SET
                FullName = @FullName,
                Email = @Email,
                PhoneNumber = @PhoneNumber,
                HireDate = @HireDate,
                Salary = @Salary,
                DepartmentId = @DepartmentId,
                PositionId = @PositionId
            WHERE Id = @Id AND IsDeleted = 0
        ";

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, employee);
        return rows > 0;
    }
}
