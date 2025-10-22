using RecruitmentApp.Shared.Domain.Contracts;

namespace RecruitmentApp.Shared.Data.Contracts;

public interface IRepositoryBase<T> where T : IEntity
{
    public Task<IEnumerable<T>> GetAllAsync();

    public Task<T?> GetByIdAsync(int id);

    public T Add(T entity);

    public T Update(T entity);

    public void Delete(T entity);

    public Task<bool> ExistsAsync(int id);
}
