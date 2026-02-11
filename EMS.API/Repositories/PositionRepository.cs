using Dapper;
using EMS.API.Data.Interface;
using EMS.API.Models;
using EMS.API.Repositories.Base;

public class PositionRepository : Repository<Position>
{
    private readonly ILogger<PositionRepository> _logger;

    public PositionRepository(IDbConnectionFactory connectionFactory, ILogger<PositionRepository> logger) : base(connectionFactory, logger)
    {
        _logger = logger;
    }

    protected override string TableName => "Positions";

    public override async Task<int> AddAsync(Position position)
    {
        var sql = $@"
            INSERT INTO {TableName} (Title, MinSalary, MaxSalary, IsDeleted)
            VALUES (@Title, @MinSalary, @MaxSalary, @IsDeleted);
            SELECT CAST(SCOPE_IDENTITY() AS int);
        ";

        try
        {
            using var connection = _connectionFactory.CreateConnection();

            _logger.LogDebug("Executing INSERT for Position with Title {Title}", position.Title);

            var newId = await connection.QuerySingleAsync<int>(sql, position);

            _logger.LogDebug("Database returned new PositionId {PositionId}", newId);

            return newId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database error while inserting Position {Title}", position.Title);
            throw;
        }
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

        try
        {
            using var connection = _connectionFactory.CreateConnection();

            _logger.LogDebug("Executing UPDATE for PositionId {PositionId}", position.Id);

            var rows = await connection.ExecuteAsync(sql, position);

            if (rows == 0)
            {
                _logger.LogWarning("UPDATE affected 0 rows for PositionId {PositionId}", position.Id);
            }

            return rows > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database error while updating PositionId {PositionId}", position.Id);
            throw;
        }
    }
}
