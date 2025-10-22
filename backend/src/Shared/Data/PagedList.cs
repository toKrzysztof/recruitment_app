namespace RecruitmentApp.Shared.Data;

public class PagedList<T>(IEnumerable<T> items, int totalCount, int currentPage, int pageSize)
{
  public IEnumerable<T> Items { get; } = items;
  public int TotalCount { get; } = totalCount;
  public int CurrentPage { get; } = currentPage;
  public int PageSize { get; } = pageSize;
  public int TotalPages { get; } = (int)Math.Ceiling(totalCount / (double)pageSize);
}
