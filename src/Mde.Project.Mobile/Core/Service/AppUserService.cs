using System.Text.Json;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Models;

namespace Mde.Project.Mobile.Core.Service;

public class AppUserService: IAppUserService{
    
    private readonly string targetFile = $"{FileSystem.AppDataDirectory}/appusers.json";
    public async Task<ICollection<AppUser>> GetAll()
        {
            EnsureFileExists(targetFile);

            string savedSerialized = await File.ReadAllTextAsync(targetFile);
            List<AppUser> savedAppUsers = JsonSerializer.Deserialize<List<AppUser>>(savedSerialized);

            return savedAppUsers.OrderByDescending(appUser => appUser.FirstName).ToList();
        }

        public async Task<AppUser> GetById(Guid id)
        {
            List<AppUser> appUsers = (await GetAll()).ToList();
            AppUser existingAppUser = appUsers.FirstOrDefault(search =>
            {
                return search.Id == id;
            });

            return existingAppUser;
        }
        public async Task<AppUser> Add(AppUser appUser)
        {
            List<AppUser> appUsers = (await GetAll()).ToList();
            bool appUserExists = appUsers.Any(search => search.Id == appUser.Id);

            if (!appUserExists)
            {
                appUser.Id = Guid.NewGuid();
                appUsers.Add(appUser);
            }
            else
            {
                throw new ArgumentException("The appUser you're trying to update already exists.");
            }

            await WriteAppUsers(appUsers);
            return appUser;
        }

        public async Task<AppUser> Update(AppUser appUser)
        {
            List<AppUser> appUsers = (await GetAll()).ToList();
            AppUser existingApUser = appUsers.FirstOrDefault(search =>
            {
                return search.Id == appUser.Id;
            });

            if (existingApUser != null){
                appUsers.Remove(existingApUser);
                appUser.Id = existingApUser.Id;
                appUsers.Add(appUser);
            }
            else
            {
                throw new ArgumentException("The appUser you're trying to update does not have a correct id.");
            }

            await WriteAppUsers(appUsers);
            return appUser;
        }

        private void EnsureFileExists(string targetFile)
        {
            if (!File.Exists(targetFile))
            {
                File.WriteAllText(targetFile, JsonSerializer.Serialize(new List<AppUser>()));
            }
        }

        private async Task WriteAppUsers(List<AppUser> appUsers)
        {
            string serializedAppUsers = JsonSerializer.Serialize(appUsers);
            await File.WriteAllTextAsync(targetFile, serializedAppUsers);
        }
}