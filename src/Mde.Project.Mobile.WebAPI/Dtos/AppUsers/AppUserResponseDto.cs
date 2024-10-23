

using Mde.Project.Mobile.WebAPI.Dtos;
using Mde.Project.Mobile.WebAPI.Dtos.Functions;

public class AppUserResponseDto:BaseDto{
    public string AccessLevelType { get; set; }
    public string UserName { get; set; }
}