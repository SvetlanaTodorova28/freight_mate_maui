using Mde.Project.Mobile.Domain.Models;

namespace Mde.Project.Mobile.Helpers;

public class CargosEventArgs : EventArgs
{
    public IEnumerable<Cargo> Cargos { get; }

    public CargosEventArgs(IEnumerable<Cargo> cargos)
    {
        Cargos = cargos;
    }
}
