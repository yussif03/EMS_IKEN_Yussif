using Dapper;
using EMS.API.Data.Interface;
using EMS.API.Models;
using EMS.API.Repositories.Base;

public class EmployeeRepository : Repository<Employee>
{
    public EmployeeRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    protected override string TableName => "Employees";

    public override async Task<int> AddAsync(Employee employee)
    {
        var sql = $@"
            INSERT INTO {TableName}
            (FullName, Email, PhoneNumber, DateOfBirth, HireDate,
             Address, Salary, Status, IsDeleted, DepartmentId, PositionId)
            VALUES
            (@FullName, @Email, @PhoneNumber, @DateOfBirth, @HireDate,
             @Address, @Salary, @Status, @IsDeleted, @DepartmentId, @PositionId);

            SELECT CAST(SCOPE_IDENTITY() AS int);
        ";

        // SELECT CAST(SCOPE_IDENTITY() AS int); returns last identity value (id) created

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, employee);
    }

    public override async Task<bool> UpdateAsync(Employee employee)
    {
        var sql = $@"
            UPDATE {TableName} SET
                FullName = @FullName,
                Email = @Email,
                PhoneNumber = @PhoneNumber,
                DateOfBirth = @DateOfBirth,
                HireDate = @HireDate,
                Address = @Address,
                Salary = @Salary,
                Status = @Status,
                DepartmentId = @DepartmentId,
                PositionId = @PositionId
            WHERE Id = @Id AND IsDeleted = 0
        ";

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, employee);

        return rows > 0;
    }
}
