using System.Text.Json;
using RecruitmentApp.Shared.Api.Contracts;

namespace RecruitmentApp.Shared.Api;

public class HttpService : IHttpService
{
  private static readonly JsonSerializerOptions JsonSerializerOptions = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
  };

  public void AddPaginationHeader(HttpResponse response, PaginationHeader header)
  {
    response.Headers.Append("Pagination", JsonSerializer.Serialize(header, JsonSerializerOptions));
    response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
  }
}
