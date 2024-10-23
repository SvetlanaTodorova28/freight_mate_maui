
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Core.Service.Web.Dto;
using Mde.Project.Mobile.Core.Service.Web.Dto.Functions;
using Mde.Project.Mobile.Core.Service.Web.Dtos.AppUsers;
using Mde.Project.Mobile.Core.Services.Web;
using Mde.Project.Mobile.Models;
using Microsoft.Maui.Storage;
using Utilities;

namespace Mde.Project.Mobile.Core.Service.Web;
    public class SecureWebAuthenticationStorage : IAuthenticationServiceMobile
    {
        private const string TokenKey = "token";
        private readonly HttpClient _httpClient;
        private Dictionary<Function, Guid> functionMappings;

        public SecureWebAuthenticationStorage(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
        }

        public async Task<string> GetTokenAsync()
        {
            return await SecureStorage.Default.GetAsync(TokenKey);
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
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/accounts/login", new LoginRequestDto{
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
           
            var moods = ((Function[])Enum.GetValues(typeof(Function))).AsEnumerable();
            return Task.FromResult(moods);
        }
        private async Task EnsureFunctionMappings()
        {
            if (functionMappings != null) return;

            var functionDtos = await _httpClient.GetFromJsonAsync<IEnumerable<FunctionDto>>("/api/accesslevels");

            Dictionary<Function, Guid> functionDictionary = new Dictionary<Function, Guid>();
            foreach (var functiondto in functionDtos)
            {
                switch (functiondto.Name.ToLower())
                {
                    case "admin":
                        functionDictionary.Add(Function.Admin,functiondto.Id);
                        break;
                    case "advanced":
                        functionDictionary.Add( Function.Consignee,functiondto.Id);
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
        
        public async Task<bool> TryRegisterAsync(string username, string password,string firstname, string lastname, Function function){
            Guid functionId;
            try
            {
                functionId = await GetFunctionIdAsync(function);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return false; // Of log de fout en behandel deze zoals vereist
            }
            
            RegisterRequestDto registerRequestDto =  new RegisterRequestDto{
                Username = username, 
                Password = password,
                FirstName = firstname,
                ConfirmPassword = password,
                LastName = lastname,
                AccessLevelTypeId = functionId
            };
            var response = await _httpClient.PostAsJsonAsync("/api/accounts/register", registerRequestDto);
           
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Failed to register: " + errorContent);
            return false;
        }


        private async Task StoreToken(string token)
        {
            await SecureStorage.Default.SetAsync(TokenKey, token);
        }
        
        private async Task<Guid> GetFunctionIdAsync(Function function)
        {
            await EnsureFunctionMappings();

            if (functionMappings.TryGetValue(function, out Guid functionId))
            {
                return functionId; 
            }

            throw new KeyNotFoundException($"Function {function} not found in function mappings.");
        }

    }

