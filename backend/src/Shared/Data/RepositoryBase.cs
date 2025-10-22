using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RecruitmentApp.Features.Contacts.Data;
using RecruitmentApp.Shared.Data.Contracts;
using RecruitmentApp.Shared.Domain.Contracts;

namespace RecruitmentApp.Shared.Data;

public class RepositoryBase<T>(
  ApplicationDbContext dbContext) : IRepositoryBase<T> where T : class, IEntity
{
  protected readonly ApplicationDbContext DbContext = dbContext;
  protected readonly DbSet<T> DbSet = dbContext.Set<T>();
  public async Task<IEnumerable<T>> GetAllAsync()
  {
    return await DbSet.ToListAsync();
  }


  public async Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> predicate)
  {
    return await DbSet
      .Where(predicate)
      .ToListAsync();
  }


  public async Task<T?> GetByIdAsync(int id)
  {
    return await DbSet.FindAsync(id);
  }

  public T Add(T entity)
  {
    DbSet.Add(entity);
    return entity;
  }

  public T Update(T entity)
  {
    DbSet.Update(entity);
    return entity;
  }

  public void Delete(T entity)
  {
    DbSet.Remove(entity);
  }

  public async Task<bool> ExistsAsync(int id)
  {
    return await DbSet.AnyAsync(e => e.Id == id);
  }
}
