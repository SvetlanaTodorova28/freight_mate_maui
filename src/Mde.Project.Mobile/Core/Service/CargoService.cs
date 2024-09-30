using System.Text.Json;
using Mde.Project.Mobile.Models;

namespace Mde.Project.Mobile.Core.Service;

public class CargoService{
      private readonly string targetFile = $"{FileSystem.AppDataDirectory}/cargos.json";
    public async Task<ICollection<Cargo>> GetAll()
        {
            EnsureFileExists(targetFile);

            string savedSerialized = await File.ReadAllTextAsync(targetFile);
            List<Cargo> savedCargos = JsonSerializer.Deserialize<List<Cargo>>(savedSerialized);

            return savedCargos.OrderByDescending(cargo => cargo.TotalWeight).ToList();
        }

        public async Task<Cargo> GetById(Guid id)
        {
            List<Cargo> cargos = (await GetAll()).ToList();
            Cargo existingCargo = cargos.FirstOrDefault(search =>
            {
                return search.Id == id;
            });

            return existingCargo;
        }
        public async Task<Cargo> Add(Cargo cargo)
        {
            List<Cargo> cargos = (await GetAll()).ToList();
            bool cargoExists = cargos.Any(search => search.Id == cargo.Id);

            if (!cargoExists)
            {
                cargo.Id = Guid.NewGuid();
                cargos.Add(cargo);
            }
            else
            {
                throw new ArgumentException("The cargo you're trying to update already exists.");
            }

            await WriteCargos(cargos);
            return cargo;
        }

        public async Task<Cargo> Update(Cargo cargo)
        {
            List<Cargo> cargos = (await GetAll()).ToList();
            Cargo existingCargo = cargos.FirstOrDefault(search =>
            {
                return search.Id == cargo.Id;
            });

            if (existingCargo != null){
                cargos.Remove(existingCargo);
                cargo.Id = existingCargo.Id;
                cargos.Add(cargo);
            }
            else
            {
                throw new ArgumentException("The cargo you're trying to update does not have a correct id.");
            }

            await WriteCargos(cargos);
            return cargo;
        }

        private void EnsureFileExists(string targetFile)
        {
            if (!File.Exists(targetFile))
            {
                File.WriteAllText(targetFile, JsonSerializer.Serialize(new List<Cargo>()));
            }
        }

        private async Task WriteCargos(List<Cargo> cargos)
        {
            string serializedCargos = JsonSerializer.Serialize(cargos);
            await File.WriteAllTextAsync(targetFile, serializedCargos);
        }
}