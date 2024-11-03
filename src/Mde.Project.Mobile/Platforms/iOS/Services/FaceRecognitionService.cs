
using CoreFoundation;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using LocalAuthentication;
using Foundation;
using Mde.Project.Mobile.Domain.Services;

namespace Mde.Project.Mobile.Platforms;
public class FaceRecognitionService : IAuthFaceRecognition
{
    private readonly LAContext context;

    public FaceRecognitionService()
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

    public async Task<NativeAuthResult> PromptLoginAsync(string prompt)
    {
        var supportStatus = IsSupported();
        if (!supportStatus.IsSupported)
        {
            return new NativeAuthResult { Authenticated = false, ErrorMessage = supportStatus.ErrorMessage };
        }

        // Configure the context for the specific operation
        context.LocalizedReason = prompt;
        context.LocalizedCancelTitle = "Cancel";

        var replyHandler = new TaskCompletionSource<NativeAuthResult>();
        DispatchQueue.MainQueue.DispatchAsync(() => {
            context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, null, (success, evalError) => {
                if (success) {
                    replyHandler.SetResult(new NativeAuthResult() { Authenticated = true });
                } else {
                    var message = evalError?.LocalizedDescription ?? "Authentication failed";
                    replyHandler.SetResult(new NativeAuthResult() { Authenticated = false, ErrorMessage = message });
                }
            });
        });
        return await replyHandler.Task;
    }
}

