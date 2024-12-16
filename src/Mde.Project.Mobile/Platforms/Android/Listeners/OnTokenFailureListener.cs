namespace Mde.Project.Mobile.Platforms.Listeners;

public class OnTokenFailureListener : Java.Lang.Object, Android.Gms.Tasks.IOnFailureListener
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