using RecruitmentApp.Shared.Data;

namespace RecruitmentApp.Shared.Api;

public class PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
{
  public int CurrentPage { get; init; } = currentPage;
  public int ItemsPerPage { get; init; } = itemsPerPage;
  public int TotalItems { get; init; } = totalItems;
  public int TotalPages { get; init; } = totalPages;

  public static PaginationHeader FromPagedList<T>(PagedList<T> pagedList)
    => new PaginationHeader(pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalCount, pagedList.TotalPages);
}
