using Mde.Project.Mobile.Models;

namespace Mde.Project.Mobile.Core.Service.Interfaces;

public interface ICargoService{
    
    public Task<Cargo> GetById(Guid id);
    public Task<ICollection<Cargo>> GetAll();
    public Task<Cargo> Add(Cargo cargo);
    public Task<Cargo> Update(Cargo cargo);
}