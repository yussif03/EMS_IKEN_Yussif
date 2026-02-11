using Dapper;
using EMS.API.Data.Interface;
using EMS.API.Models;
using EMS.API.Repositories.Base;

public class DepartmentRepository : Repository<Department>
{
    private readonly ILogger<DepartmentRepository> _logger;

    public DepartmentRepository(IDbConnectionFactory connectionFactory, ILogger<DepartmentRepository> logger) : base(connectionFactory, logger)
    {
        _logger = logger;
    }

    protected override string TableName => "Departments";

    public override async Task<int> AddAsync(Department department)
    {
        var sql = $@"
            INSERT INTO {TableName} (Name, Description, IsDeleted)
            VALUES (@Name, @Description, @IsDeleted);
            SELECT CAST(SCOPE_IDENTITY() AS int);
        ";

        try
        {
            using var connection = _connectionFactory.CreateConnection();

            _logger.LogDebug("Executing INSERT for Department with Name {DepartmentName}", department.Name);

            var newId = await connection.QuerySingleAsync<int>(sql, department);

            _logger.LogDebug("Database returned new DepartmentId {DepartmentId}", newId);

            return newId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database error while inserting Department {DepartmentName}", department.Name);
            throw;
        }
    }

    public override async Task<bool> UpdateAsync(Department department)
    {
        var sql = $@"
            UPDATE {TableName} SET
                Name = @Name,
                Description = @Description
            WHERE Id = @Id AND IsDeleted = 0
        ";

        try
        {
            using var connection = _connectionFactory.CreateConnection();

            _logger.LogDebug("Executing UPDATE for DepartmentId {DepartmentId}", department.Id);

            var rows = await connection.ExecuteAsync(sql, department);

            if (rows == 0)
            {
                _logger.LogWarning("UPDATE affected 0 rows for DepartmentId {DepartmentId}", department.Id);
            }

            return rows > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database error while updating DepartmentId {DepartmentId}", department.Id);
            throw;
        }
    }
}
