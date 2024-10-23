using Mde.Project.Mobile.WebAPI.Dtos.Functions;

namespace Mde.Project.Mobile.WebAPI.Api.Dtos;

public class UserRequestDto{
    public string Id { get; set; }
    public string Name { get; set; }
    public AccessLevelsResponseDto AccessLevelType { get; set; }
}