using Dapper;
using EMS.API.Data.Interface;
using EMS.API.Models;
using EMS.API.Repositories.Base;

public class PositionRepository : Repository<Position>
{
    public PositionRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    protected override string TableName => "Positions";

    public override async Task<int> AddAsync(Position position)
    {
        var sql = $@"
            INSERT INTO {TableName}
            (Title, MinSalary, MaxSalary, IsDeleted)
            VALUES (@Title, @MinSalary, @MaxSalary, @IsDeleted);

            SELECT CAST(SCOPE_IDENTITY() AS int);
        ";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, position);
    }

    public override async Task<bool> UpdateAsync(Position position)
    {
        var sql = $@"
            UPDATE {TableName} SET
                Title = @Title,
                MinSalary = @MinSalary,
                MaxSalary = @MaxSalary
            WHERE Id = @Id AND IsDeleted = 0
        ";

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, position);

        return rows > 0;
    }
}
