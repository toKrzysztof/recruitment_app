namespace RecruitmentApp.Shared.Api;

public interface IHttpService
{
    void AddPaginationHeader(HttpResponse response, PaginationHeader header);
}