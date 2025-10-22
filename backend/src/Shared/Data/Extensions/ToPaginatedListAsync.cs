using Microsoft.EntityFrameworkCore;

namespace RecruitmentApp.Shared.Data.Extensions;

public static class QueryableExtensions
{
  public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int currentPage, int pageSize)
  {
    var totalCount = await query.CountAsync();
    var items = await query
      .Skip((currentPage - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();

    return new PagedList<T>(items, totalCount, currentPage, pageSize);
  }
}
