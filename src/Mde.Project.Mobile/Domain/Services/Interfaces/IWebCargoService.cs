using Mde.Project.Mobile.Models;

namespace Mde.Project.Mobile.Core.Service.Interfaces;

public interface IWebCargoService{
    
    public Task<Cargo> GetById(Guid id);
    public Task<ICollection<Cargo>> GetAll();
    public Task<Cargo> Add(Cargo cargo);
    public Task<Cargo> Update(Cargo cargo);
    public Task Delete(Cargo cargo);
}