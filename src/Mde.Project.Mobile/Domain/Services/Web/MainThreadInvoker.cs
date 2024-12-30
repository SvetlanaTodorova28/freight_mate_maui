using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.Domain.Services.Web;
public class MainThreadInvoker : IMainThreadInvoker
{
    public void InvokeOnMainThread(Action action)
    {
        MainThread.BeginInvokeOnMainThread(action);
    }
}