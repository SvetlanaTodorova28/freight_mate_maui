namespace Mde.Project.Mobile.WebAPI.Services.Models;

public class ResultModel<T>:BaseResult{
    public T Data { get; set; }
}