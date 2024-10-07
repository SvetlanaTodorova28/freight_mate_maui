namespace Mde.Project.Mobile.WebAPI.Services.Models;

public class BaseResult{
    public bool Success => !Errors.Any();
    public List<string> Errors { get; set; } = new List<string>();
}