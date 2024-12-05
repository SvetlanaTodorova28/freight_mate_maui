
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Functions;

namespace Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;

public class AppUserResponseDto{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public AccessLevelsResponseDto? AccessLevelType { get; set; }

}

    
