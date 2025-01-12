namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IFirebaseTokenService{
    Task<ServiceResult<string>> RetrieveAndStoreFcmTokenLocallyAsync();
    Task<ServiceResult<string>> UpdateFcmTokenOnServerAsync(IAppUserService appUserService);
    Task<ServiceResult<string>> GetStoredFcmTokenAsync();
}