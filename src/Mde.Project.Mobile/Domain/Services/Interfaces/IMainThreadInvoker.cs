namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IMainThreadInvoker{
    void InvokeOnMainThread(Action action);
}