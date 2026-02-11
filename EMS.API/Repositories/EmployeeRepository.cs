using Dapper;
using EMS.API.Data.Interface;
using EMS.API.Models;
using EMS.API.Repositories.Base;

public class EmployeeRepository : Repository<Employee>
{
    private readonly ILogger<EmployeeRepository> _logger;

    public EmployeeRepository(IDbConnectionFactory connectionFactory, ILogger<EmployeeRepository> logger) : base(connectionFactory, logger)
    {
        _logger = logger;
    }

    protected override string TableName => "Employees";

    public override async Task<int> AddAsync(Employee employee)
    {
        var sql = $@"
            INSERT INTO {TableName}
                (FullName, Email, PhoneNumber, DateOfBirth, HireDate, Address, Salary, Status, IsDeleted, DepartmentId, PositionId)
                VALUES
                (@FullName, @Email, @PhoneNumber, @DateOfBirth, @HireDate, @Address, @Salary, @Status, @IsDeleted, @DepartmentId, @PositionId);
            SELECT CAST(SCOPE_IDENTITY() AS int);
        ";

        try
        {
            using var connection = _connectionFactory.CreateConnection();

            _logger.LogDebug("Executing INSERT for employee with email {Email}", employee.Email);

            var newId = await connection.QuerySingleAsync<int>(sql, employee);

            _logger.LogDebug("Database returned new EmployeeId {EmployeeId}", newId);

            return newId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database error while inserting employee with email {Email}", employee.Email);
            throw; // Let API layer handle response
        }
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

        try
        {
            using var connection = _connectionFactory.CreateConnection();

            _logger.LogDebug("Executing UPDATE for EmployeeId {EmployeeId}", employee.Id);

            var rows = await connection.ExecuteAsync(sql, employee);

            if (rows == 0)
            {
                _logger.LogWarning("UPDATE affected 0 rows for EmployeeId {EmployeeId}", employee.Id);
            }

            return rows > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database error while updating EmployeeId {EmployeeId}", employee.Id);
            throw;
        }
    }
}
