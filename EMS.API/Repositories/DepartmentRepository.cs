using Dapper;
using EMS.API.Data.Interface;
using EMS.API.Models;
using EMS.API.Repositories.Base;

public class DepartmentRepository : Repository<Department>
{
    public DepartmentRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    protected override string TableName => "Departments";

    public override async Task<int> AddAsync(Department department)
    {
        var sql = $@"
            INSERT INTO {TableName}
            (Name, Description, ManagerId, IsDeleted)
            VALUES (@Name, @Description, @ManagerId, @IsDeleted);

            SELECT CAST(SCOPE_IDENTITY() AS int);
        ";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, department);
    }

    public override async Task<bool> UpdateAsync(Department department)
    {
        var sql = $@"
            UPDATE {TableName} SET
                Name = @Name,
                Description = @Description,
                ManagerId = @ManagerId
            WHERE Id = @Id AND IsDeleted = 0
        ";

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, department);

        return rows > 0;
    }
}
