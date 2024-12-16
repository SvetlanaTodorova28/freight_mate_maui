using AndroidGmsTasks = Android.Gms.Tasks;

namespace Mde.Project.Mobile.Platforms.Android.Listeners;

public class OnTokenSuccessListener : Java.Lang.Object, AndroidGmsTasks.IOnSuccessListener
{
    private readonly Action<string> _onSuccess;

    public OnTokenSuccessListener(Action<string> onSuccess)
    {
        _onSuccess = onSuccess ?? throw new ArgumentNullException(nameof(onSuccess));
    }

    public void OnSuccess(Java.Lang.Object result)
    {
        if (result != null)
        {
            _onSuccess.Invoke(result.ToString());
        }
    }
}