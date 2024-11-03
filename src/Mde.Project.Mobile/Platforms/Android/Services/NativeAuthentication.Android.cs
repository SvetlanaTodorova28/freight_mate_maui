using AndroidX.Biometric;
using Java.Util.Concurrent;
using AndroidX.Fragment.App;
using Java.Lang;
using Mde.Project.Mobile.Domain.Services;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Application = Android.App.Application;

namespace Mde.Project.Mobile.Platforms;

public class NativeAuthentication : INativeAuthentication
{
    private readonly BiometricManager biometricManager;

    public NativeAuthentication()
    {
        biometricManager = BiometricManager.From(Application.Context);
    }

    public SupportStatus IsSupported()
    {
        SupportStatus supportStatus = new SupportStatus { IsSupported = false };

        int result = biometricManager.CanAuthenticate(BiometricManager.Authenticators.BiometricStrong);

        switch (result)
        {
            case BiometricManager.BiometricSuccess:
                supportStatus.IsSupported = true;
                break;
            case BiometricManager.BiometricErrorNoHardware:
                supportStatus.ErrorMessage = "No biometric features available on this device.";
                break;
            case BiometricManager.BiometricErrorNoneEnrolled:
                supportStatus.ErrorMessage = "No biometrics enrolled for android.";
                break;
            case BiometricManager.BiometricErrorSecurityUpdateRequired:
            case BiometricManager.BiometricStatusUnknown:
                supportStatus.ErrorMessage = "An error occurred with the biometric sensors.";
                break;
        }

        return supportStatus;
    }

    public async Task<NativeAuthResult> PromptLoginAsync(string reason){
        SupportStatus status = IsSupported();
        if (!status.IsSupported){
            return new NativeAuthResult{ Authenticated = false, ErrorMessage = status.ErrorMessage };
        }

        var taskCancellationSource = new CancellationTokenSource();

        
        var taskCompletionSource = new TaskCompletionSource<NativeAuthResult>();
        var executor = Executors.NewSingleThreadExecutor();
        var activity = (FragmentActivity)Platform.CurrentActivity; 

//build the prompt
        var builder = new BiometricPrompt.PromptInfo.Builder()
            .SetTitle("Verify it's you")
            .SetConfirmationRequired(true)
            .SetDescription(reason)
            .SetNegativeButtonText("Cancel");

        var info = builder.Build();


        var authenticationCallback = new AuthenticationCallback(taskCompletionSource);
        using var dialog = new BiometricPrompt(activity, executor, authenticationCallback);
        await using (var taskCancellation = taskCancellationSource.Token.Register(dialog.CancelAuthentication)){
            dialog.Authenticate(info);
        }

        return await taskCompletionSource.Task;

    }

    /*var taskCompletionSource = new TaskCompletionSource<NativeAuthResult>();
    var executor = Executors.NewSingleThreadExecutor();
    var activity = (FragmentActivity)Application.Context;
    var promptInfo = new BiometricPrompt.PromptInfo.Builder()
        .SetTitle("Biometric Login")
        .SetSubtitle("Log in using your biometric credential")
        .SetDescription(reason)
        .SetNegativeButtonText("Cancel")
        .Build();

    var authenticationCallback = new AuthenticationCallback(taskCompletionSource);
    var biometricPrompt = new BiometricPrompt(activity, executor, authenticationCallback);

    biometricPrompt.Authenticate(promptInfo);

    return await taskCompletionSource.Task;*/
    }

    class AuthenticationCallback : BiometricPrompt.AuthenticationCallback
    {
        private readonly TaskCompletionSource<NativeAuthResult> taskCompletionSource;

        public AuthenticationCallback(TaskCompletionSource<NativeAuthResult> taskCompletionSource)
        {
            this.taskCompletionSource = taskCompletionSource;
        }

        public override void OnAuthenticationSucceeded(BiometricPrompt.AuthenticationResult result)
        {
            taskCompletionSource.SetResult(new NativeAuthResult { Authenticated = true });
        }

        public override void OnAuthenticationFailed()
        {
            taskCompletionSource.SetResult(new NativeAuthResult { Authenticated = false, ErrorMessage = "Authentication failed. Please try again." });
        }

        public override void OnAuthenticationError(int errorCode, ICharSequence errString)
        {
            taskCompletionSource.SetResult(new NativeAuthResult { Authenticated = false, ErrorMessage = errString.ToString() });
        }
    }

