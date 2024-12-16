using AndroidGmsTasks = Android.Gms.Tasks;

namespace Mde.Project.Mobile.Platforms.Android.Listeners;

public class OnTokenFailureListener : Java.Lang.Object, AndroidGmsTasks.IOnFailureListener
{
    private readonly Action<Exception> _onFailure;

    public OnTokenFailureListener(Action<Exception> onFailure)
    {
        _onFailure = onFailure ?? throw new ArgumentNullException(nameof(onFailure));
    }

    public void OnFailure(Java.Lang.Exception e)
    {
        _onFailure?.Invoke(new Exception(e.Message, e));
    }
}
