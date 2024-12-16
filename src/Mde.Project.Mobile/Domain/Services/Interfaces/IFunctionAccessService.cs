using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Web;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IFunctionAccessService
{
    Task<ServiceResult<IEnumerable<Function>>> GetFunctionsAsync();
    Task<ServiceResult<Guid>> GetFunctionIdAsync(Function function);
    Task<ServiceResult<Function>> GetUserFunctionFromTokenAsync();
}
