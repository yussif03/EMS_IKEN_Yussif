using Dapper;
using EMS.API.Data.Interface;
using EMS.API.Models;
using EMS.API.Repositories.Base;

public class PositionRepository
    : Repository<Position>
{
    public PositionRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    protected override string TableName => "Position";

    public override async Task<int> AddAsync(Position position)
    {
        const string sql = @"
            INSERT INTO Position (Title, IsDeleted)
            VALUES (@Title, @IsDeleted);
            SELECT CAST(SCOPE_IDENTITY() AS int);
        ";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, position);
    }

    public override async Task<bool> UpdateAsync(Position position)
    {
        const string sql = @"
            UPDATE Position SET
                Title = @Title
            WHERE Id = @Id AND IsDeleted = 0
        ";

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, position);
        return rows > 0;
    }
}
