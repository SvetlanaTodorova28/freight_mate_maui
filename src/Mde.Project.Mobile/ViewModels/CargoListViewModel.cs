using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Mde.Project.Mobile.ViewModels;

public class CargoListViewModel:ObservableObject{
    private ICommand createCargoCommand;

    public ICommand CreateCargoCommand{
        get{ return createCargoCommand;}
        set{
            SetProperty(ref createCargoCommand, value);
        }
    }
}