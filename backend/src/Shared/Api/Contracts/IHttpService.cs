namespace RecruitmentApp.Shared.Api.Contracts;

public interface IHttpService
{
    void AddPaginationHeader(HttpResponse response, PaginationHeader header);
}
