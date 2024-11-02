using CoreFoundation;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using LocalAuthentication;
using Foundation;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services;

namespace Mde.Project.Mobile.Platforms.iOS.Services;

public class FaceRecognitionService : IAuthFaceRecognition
{
    public bool IsSupported()
    {
        using var context = new LAContext();
        return context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, out NSError error);
    }

    public async Task<NativeAuthResult> PromptLoginAsync(string prompt)
    {
        var context = new LAContext
        {
            LocalizedReason = prompt, // Toon reden voor authenticatie
            LocalizedCancelTitle = "Cancel" // Optie om te annuleren
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