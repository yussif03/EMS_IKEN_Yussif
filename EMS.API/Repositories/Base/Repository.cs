using Dapper;
using EMS.API.Data.Interface;

namespace EMS.API.Repositories.Base
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IDbConnectionFactory _connectionFactory;
        protected readonly ILogger _logger;

        protected Repository(IDbConnectionFactory connectionFactory, ILogger logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        protected abstract string TableName { get; }

        protected virtual string SelectAllQuery =>
            $"SELECT * FROM {TableName} WHERE IsDeleted = 0";

        protected virtual string SelectByIdQuery =>
            $"SELECT * FROM {TableName} WHERE Id = @Id AND IsDeleted = 0";

        protected virtual string SoftDeleteQuery =>
            $"UPDATE {TableName} SET IsDeleted = 1 WHERE Id = @Id";

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                _logger.LogDebug("Executing query: GetAll from {Table}", TableName);

                var result = await connection.QueryAsync<T>(SelectAllQuery);

                _logger.LogDebug("Query GetAll from {Table} returned {Count} rows", TableName, result.Count());

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error while fetching all records from {Table}", TableName);
                throw;
            }
        }


        public virtual async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                _logger.LogDebug("Executing query: GetById {Id} from {Table}", id, TableName);

                var entity = await connection.QuerySingleOrDefaultAsync<T>(
                    SelectByIdQuery,
                    new { Id = id }
                );

                if (entity is null)
                    _logger.LogDebug("No record found with Id {Id} in {Table}", id, TableName);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error while fetching Id {Id} from {Table}", id, TableName);
                throw;
            }
        }

        public virtual async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                _logger.LogDebug("Executing SoftDelete for Id {Id} in {Table}", id, TableName);

                var rows = await connection.ExecuteAsync(
                    SoftDeleteQuery,
                    new { Id = id }
                );

                if (rows == 0)
                    _logger.LogWarning("SoftDelete affected 0 rows for Id {Id} in {Table}", id, TableName);

                return rows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error during SoftDelete for Id {Id} in {Table}", id, TableName);
                throw;
            }
        }

        public abstract Task<int> AddAsync(T entity);
        public abstract Task<bool> UpdateAsync(T entity);
    }
}
