namespace RecruitmentApp.Features.Contacts.Api;

public class GetAllContactsQueryParams
{
  public string Sort { get; init; } = "asc";
  public int PageNumber { get; init; } = 1;
  public int PageSize { get; init; } = 5;
}
