namespace RecruitmentApp.Shared.Application;

public class ServiceResult<T>
{
    public T Data { get; init; }
    public IEnumerable<string> Errors { get; init; } = [];
    public bool IsSuccess => !Errors.Any();

    public static ServiceResult<T> Success(T data)
    {
        return new ServiceResult<T>
        {
            Data = data
        };
    }

    public static ServiceResult<T> Failure(IEnumerable<string> errors)
    {
        return new ServiceResult<T>
        {
            Errors = errors
        };
    }
}
