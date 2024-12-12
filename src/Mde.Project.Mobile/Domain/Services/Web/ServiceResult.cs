namespace Mde.Project.Mobile.Domain.Services.Web;

public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public string ErrorMessage { get; set; }

    public static ServiceResult<T> Success(T data)
    {
        return new ServiceResult<T> { IsSuccess = true, Data = data };
    }

    public static ServiceResult<T> Failure(string errorMessage)
    {
        return new ServiceResult<T> { IsSuccess = false, ErrorMessage = errorMessage };
    }
}

