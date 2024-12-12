using Mde.Project.Mobile.Domain.Models;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IFunctionAccessService{
    public Task<IEnumerable<Function>> GetFunctionsAsync();
    public Task<Guid> GetFunctionIdAsync(Function function);
    public Task<Function> GetUserFunctionFromTokenAsync();
}