
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Functions;

namespace Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;

public class AppUserResponseDto{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public FunctionDto? Function { get; set; }
    
    public List<CargoResponseDto>? Cargos { get; set; }

}