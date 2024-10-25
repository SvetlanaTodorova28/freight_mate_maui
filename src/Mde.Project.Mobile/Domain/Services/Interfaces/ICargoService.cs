using Mde.Project.Mobile.Domain.Models;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface ICargoService{
    
    public Task<Cargo> GetById(Guid id);
    public Task<ICollection<Cargo>> GetAll();
    public Task<Cargo> Add(Cargo cargo);
    public Task<Cargo> Update(Cargo cargo);
    public Task Delete(Cargo cargo);
}