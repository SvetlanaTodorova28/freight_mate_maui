

using Mde.Project.Mobile.Models;

namespace Mde.Project.Mobile.Core.Service.Interfaces;

    public interface IAppUserService {
        public Task<AppUser> GetById(Guid id);
        public Task<ICollection<AppUser>> GetAll();
        public Task<AppUser> Add(AppUser appUser);
        public Task<AppUser> Update(AppUser appUser);
       
    }
