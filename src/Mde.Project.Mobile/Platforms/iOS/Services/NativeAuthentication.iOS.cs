
using CoreFoundation;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using LocalAuthentication;
using Foundation;
using Mde.Project.Mobile.Domain.Services;

namespace Mde.Project.Mobile.Platforms;
public class NativeAuthentication : INativeAuthentication
{
    private readonly LAContext context;

    public NativeAuthentication()
    {
        context = new LAContext();
    }

    public SupportStatus IsSupported()
    {
        NSError error;
        var supportStatus = new SupportStatus { IsSupported = context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out error) };

        if (!supportStatus.IsSupported)
        {
            switch ((LAStatus)error.Code)
            {
                case LAStatus.BiometryNotEnrolled:
                    supportStatus.ErrorMessage = "Biometrics not set up.";
                    break;
                case LAStatus.BiometryNotAvailable:
                    supportStatus.ErrorMessage = "Biometrics not available on this device.";
                    break;
                case LAStatus.BiometryLockout:
                    supportStatus.ErrorMessage = "Biometrics are locked out due to too many attempts.";
                    break;
                default:
                    supportStatus.ErrorMessage = "Biometrics are not available: " + error.LocalizedDescription;
                    break;
            }
        }

        return supportStatus;
    }

    /*public async Task<NativeAuthResult> PromptLoginAsync(string prompt)
    {
        var supportStatus = IsSupported();
        if (!supportStatus.IsSupported)
        {
            return new NativeAuthResult { Authenticated = false, ErrorMessage = supportStatus.ErrorMessage };
        }

        // Configureer de context voor de specifieke operatie
        if (string.IsNullOrEmpty(prompt))
        {
            prompt = "Bevestig uw identiteit om door te gaan"; // Standaardbericht indien geen prompt voorzien
        }

        context.LocalizedReason = prompt; // Hier moet je zeker zijn dat prompt niet null is.
        context.LocalizedCancelTitle = "Annuleren";

        var replyHandler = new TaskCompletionSource<NativeAuthResult>();
        DispatchQueue.MainQueue.DispatchAsync(() => {
            context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, null, (success, evalError) => {
                if (success) {
                    replyHandler.SetResult(new NativeAuthResult() { Authenticated = true });
                } else {
                    var message = evalError?.LocalizedDescription ?? "Authenticatie mislukt";
                    replyHandler.SetResult(new NativeAuthResult() { Authenticated = false, ErrorMessage = message });
                }
            });
        });
        return await replyHandler.Task;
    }*/
    public async Task<NativeAuthResult> PromptLoginAsync(string prompt)
    {
        var supportStatus = IsSupported();
        if (!supportStatus.IsSupported)
        {
            return new NativeAuthResult { Authenticated = false, ErrorMessage = supportStatus.ErrorMessage };
        }
        var context = new LAContext
        {
            LocalizedReason = prompt, 
            LocalizedCancelTitle = "Cancel" 
        };

        var replyHandler = new TaskCompletionSource<NativeAuthResult>();
        DispatchQueue.MainQueue.DispatchAsync(() => {
            context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, prompt, (success, error) => {
                if (success) {
                    replyHandler.SetResult(new NativeAuthResult() { Authenticated = true });
                } else {
                    var message = error?.LocalizedDescription ?? "Authentication failed";
                    Console.WriteLine($"Authentication failed with error: {error?.Code} - {message}");
                    replyHandler.SetResult(new NativeAuthResult() { Authenticated = false, ErrorMessage = message });
                }
            });

        });
        return await replyHandler.Task;
    }

}

