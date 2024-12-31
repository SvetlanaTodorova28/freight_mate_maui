using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.Domain.Services.Web;
public class MainThreadInvoker : IMainThreadInvoker
{
    public  void InvokeOnMainThread(Action action){
        MainThread.BeginInvokeOnMainThread(action);
    }
 
    public async Task InvokeOnMainThreadAsync(Func<Task> action)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                await action();
                tcs.SetResult(true);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        await tcs.Task;
    }
}