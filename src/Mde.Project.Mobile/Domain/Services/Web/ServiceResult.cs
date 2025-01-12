public class ServiceResult<T>
{
    public bool IsSuccess { get;  set; }
    public T Data { get; private set; }
    public string Message { get; private set; }
    public string ErrorMessage { get;  set; }

    public static ServiceResult<T> Success(T data, string message = "")
    {
        return new ServiceResult<T> { IsSuccess = true, Data = data, Message = message };
    }

    public static ServiceResult<T> Failure(string errorMessage)
    {
        return new ServiceResult<T> { IsSuccess = false, ErrorMessage = errorMessage };
    }
}