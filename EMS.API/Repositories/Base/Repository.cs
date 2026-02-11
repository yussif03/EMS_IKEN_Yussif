using Dapper;
using EMS.API.Data.Interface;

namespace EMS.API.Repositories.Base
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IDbConnectionFactory _connectionFactory;

        protected Repository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected abstract string TableName { get; }

        protected virtual string SelectAllQuery => $"SELECT * FROM {TableName} WHERE IsDeleted = 0";

        protected virtual string SelectByIdQuery => $"SELECT * FROM {TableName} WHERE Id = @Id AND IsDeleted = 0";

        protected virtual string SoftDeleteQuery => $"UPDATE {TableName} SET IsDeleted = 1 WHERE Id = @Id";

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<T>(SelectAllQuery);
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<T>(SelectByIdQuery, new { Id = id });
        }

        public virtual async Task<bool> SoftDeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var rows = await connection.ExecuteAsync(SoftDeleteQuery,new { Id = id });

            return rows > 0;
        }

        public abstract Task<int> AddAsync(T entity);
        public abstract Task<bool> UpdateAsync(T entity);
    }
}
