using CoreFoundation;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using LocalAuthentication;
using Foundation;
using Mde.Project.Mobile.Domain.Services;

namespace Mde.Project.Mobile.Platforms;
public class NativeAuthentication : INativeAuthentication
{
    

    public SupportStatus IsSupported()
    {
        NSError error;
        var context = new LAContext();
        var supportStatus = new SupportStatus { IsSupported = context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out error) };

        if (!supportStatus.IsSupported && error != null)
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

        var context = new LAContext
        {
            LocalizedReason = prompt,
            LocalizedCancelTitle = "Cancel",
            LocalizedFallbackTitle = "Use Passcode"
        };

        var (success, error) = await context.EvaluatePolicyAsync(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, prompt);

        if (success)
        {
            return new NativeAuthResult { Authenticated = true };
        }
        
        string errorMessage =
            error != null ? error.LocalizedDescription : "Authentication failed for an unknown reason.";
        return new NativeAuthResult{ Authenticated = false, ErrorMessage = errorMessage };
        
    }


}