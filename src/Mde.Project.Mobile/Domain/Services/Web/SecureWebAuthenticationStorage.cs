
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Functions;
using Utilities;

namespace Mde.Project.Mobile.Domain.Services.Web;
    public class SecureWebAuthenticationStorage : IAuthenticationServiceMobile
    {
        private const string TokenKey = "token";
        private const string FcmTokenKey = "fcmToken";
        private readonly HttpClient _azureHttpClient;
        private readonly HttpClient _firebaseHttpClient;
        private Dictionary<Function, Guid> functionMappings;

        public SecureWebAuthenticationStorage(IHttpClientFactory httpClientFactory)
        {
            _azureHttpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
            _firebaseHttpClient = httpClientFactory.CreateClient(GlobalConstants.BaseUrlFireBase);
        }

        public async Task<string> GetTokenAsync()
        {
            return await SecureStorage.Default.GetAsync(TokenKey);
        }
        public async Task StoreFcmToken(string token)
        {
            await SecureStorage.Default.SetAsync(FcmTokenKey, token);
        }

        public async Task<string> GetFcmTokenAsync()
        {
            return await SecureStorage.Default.GetAsync(FcmTokenKey);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            string encodedToken = await GetTokenAsync();

            if (encodedToken == null) { return false; }

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(encodedToken) as JwtSecurityToken;

            return jsonToken.ValidTo > DateTime.UtcNow;
        }

        public bool Logout()
        {
            bool success = SecureStorage.Default.Remove(TokenKey);
            return success;
        }

        public async Task<bool> TryLoginAsync(string username, string password)
        {
            HttpResponseMessage response = await _azureHttpClient.PostAsJsonAsync("/api/accounts/login", new LoginRequestDto{
                Username = username, 
                Password = password
            });

            if (response.IsSuccessStatusCode)
            {
                LoginResponseDto loginResponseDto = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

                await StoreToken(loginResponseDto.Token);
                return true;
            }

            return false;
        }
        

        public Task<IEnumerable<Function>> GetFunctionsAsync()
        {
           
            var functions = ((Function[])Enum.GetValues(typeof(Function))).AsEnumerable();
            return Task.FromResult(functions);
        }
        private async Task EnsureFunctionMappings()
        {
            if (functionMappings != null) return;

            var functionDtos = await _azureHttpClient.GetFromJsonAsync<IEnumerable<AccessLevelsResponseDto>>("/api/AccessLevels");

            Dictionary<Function, Guid> functionDictionary = new Dictionary<Function, Guid>();
            foreach (var functiondto in functionDtos)
            {
                switch (functiondto.Name.ToLower())
                {
                    case "admin":
                        functionDictionary.Add(Function.Admin,functiondto.Id);
                        break;
                    case "advanced":
                        functionDictionary.Add(Function.Consignee,functiondto.Id);
                        break;
                    case "basic":
                        functionDictionary.Add(Function.Driver,functiondto.Id);
                        break;
                    default:
                        break;
                }
            }

            functionMappings = functionDictionary;
        }
        
        public async Task<bool> TryRegisterAsync(string username, string password, string confirmPassword, string firstname, string lastname, Function function){
            Guid functionId;
            try
            {
                functionId = await GetFunctionIdAsync(function);
            }
            catch (KeyNotFoundException ex)
            {
               
                return false; 
            }
            
            RegisterRequestDto registerRequestDto =  new RegisterRequestDto{
                Username = username, 
                Password = password,
                FirstName = firstname,
                ConfirmPassword = confirmPassword,
                LastName = lastname,
                AccessLevelTypeId = functionId
            };
            var response = await _azureHttpClient.PostAsJsonAsync("/api/accounts/register", registerRequestDto);
           
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
           
            return false;
        }


        private async Task StoreToken(string token)
        {
            await SecureStorage.Default.SetAsync(TokenKey, token);
        }
        
        public async Task<string> GetUserIdFromTokenAsync()
        {
            var token = await SecureStorage.GetAsync("token");
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("No token found");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new InvalidOperationException("User ID claim not found in token");

            return userIdClaim.Value;
        }
        
        public async Task<Guid> GetFunctionIdAsync(Function function)
        {
            await EnsureFunctionMappings();

            if (functionMappings.TryGetValue(function, out Guid functionId))
            {
                return functionId; 
            }

            throw new KeyNotFoundException($"Function {function} not found in function mappings.");
        }
        
        public async Task<Function> GetUserFunctionFromTokenAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("No token found");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role); 

            if (roleClaim == null)
                throw new InvalidOperationException("Role claim not found in token");

            // Map the role claim to the corresponding Function
            return roleClaim.Value.ToLower() switch
            {
                "admin" => Function.Admin,
                "advanced" => Function.Consignee,
                "basic" => Function.Driver,
                _ => throw new InvalidOperationException("Invalid role")
            };
        }
        
        public async Task<string> GetUserFirstNameFromTokenAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("No token found");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
    
            
            var firstNameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "FirstName");

            return firstNameClaim?.Value ?? "Unknown"; 
        }

        public async Task SendNotificationAsync(object message)
        {
            var accessToken = "ya29.c.c0ASRK0GZDY2W7YcH9VHJj6zubKKVJHaByHLzKrzk9o2mFvUJxye43DgTADjtA-6iGSZXZ5FzVe2Bwl8RtfarsdhpGrxdTY_5VMGIqVSUDyPQVuUR2uyMkGLzH3tbMjDGZwTlakOpTmSC-WyZHatL0IhCttmOhy3SSfALxirSYs0uo1wnK-X4JlAa-Vjc1QOZYtc21Hmnp0uB-gBk450Kg-r36nHBLPFkc01ziUsgCE4spRKFBMh2XDIQG4aUF_WkGAwx_NUYnx05eQ_oF_Ha3yERl4MhCsLAPN3ooefAIcaBBt3EJ0wIsxrpRkCGDCL6xsVB6aUV0hGnhB3D6PIa_q1kGJX4xCGr5lU-nRMarGR8wqGymu47QPjHcM4r0aiIqK7sGTmMN400DQiOgj_lra2BueXd1d08QY7Yo8Z08wOujJRJow3SBda2w14i5bqfg80_OlruF57Fv_zn5s5Z2s45t-oRafSJWkbgmg44zxsyVXjp03baXlwdnjuWwjSfdxp6s_laX2ZI2SsR20XY3bvk9mdcWx5F1srnIJvtyzuMs3UR_nMimOX6bVt7S26vwSklSjWbIJ4v_6fl74lilikVJ4hk2ehiWJjQbIYk9zfnSaxhd5U0yQrJ_wm6Qr3o5v3i0OxF3f-1wYuUh2Msw08ROMS_6ORVazV-j2bUZZF9cFzf53jJVOY2xd9-Rc0R3q4J36OgvI24pgrSzQio1_W6Z_iIjbY2wfn2Jk54kn-f623cj351d3Rp1Zu818cb5WRpqx6Fib9cgV7099SqvVQmcXzl8B5Yefu29B82rc2554doYwcjamf314u9R7V2M4FXknS8k6aUyFV2Zt5on9YctvwVwdbMci_OJtxp5kipi4mjo6iXsfmtciW8OBggWhFp1dkW0ZypufR1fWem3fX1vle8YRa-poZauBj10pvbU2orrBWXQO-VUl0Ic-Qu_9RF_mfd8vM6nzYsgmyq1tyOabmiMum37XXeblX21MpdUbdmc387ikxq";
    
            _firebaseHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _firebaseHttpClient.PostAsJsonAsync("https://fcm.googleapis.com/v1/projects/mde-project-mobile/messages:send", message);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to send notification. Status: {response.StatusCode}, Error: {errorDetails}");
            }
        }




    }

