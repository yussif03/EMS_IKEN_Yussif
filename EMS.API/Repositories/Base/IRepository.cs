namespace EMS.API.Repositories.Base
{
    public interface IRepository<T> where T : class
    {
        Task<int> AddAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> UpdateAsync(T entity); // bool --> dapper return affected rows
        Task<bool> SoftDeleteAsync(int id);
    }
}
