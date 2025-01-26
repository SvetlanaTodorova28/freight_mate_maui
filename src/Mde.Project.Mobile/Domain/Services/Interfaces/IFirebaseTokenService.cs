namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IFirebaseTokenService{
    Task<ServiceResult<string>> RetrieveFromFireBaseAndStoreFcmTokenLocallyAsync();
    Task<ServiceResult<string>> UpdateFcmTokenOnServerAsync(IAppUserService appUserService);
    Task<ServiceResult<string>> GetStoredFcmTokenAsync();
}