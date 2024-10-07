using System.IdentityModel.Tokens.Jwt;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Mde.Project.Mobile.WebAPI.Services.Models;
using Microsoft.AspNetCore.Identity;
using Utilities;


namespace Mde.Project.Mobile.WebAPI.Services;

public class AuthenticationService:IAuthenticationService{
         private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        

        public AuthenticationService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService){
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        //Het primaire doel van de registratie is het aanmaken van een nieuwe gebruikersaccount in het systeem.
        //Dit omvat het verzamelen van gebruikersgegevens, het aanmaken van een record in de database,
        //en soms het toewijzen van een of meer rollen aan de gebruiker (bijvoorbeeld standaardrollen zoals "gebruiker" of "consignee").
        //Rollen worden vaak toegevoegd om toegangsniveaus te bepalen, die bij latere interacties relevant zijn.
     public async Task<ResultModel<AppUser>> RegisterUserAsync(AppUser newUser, string password){
               
            var result = await _userManager.CreateAsync(newUser, password);
            if (!result.Succeeded){
                return new ResultModel<AppUser>{
                    Errors = new List<string>(result.Errors.Select(x => x.Description))
                };
            }

            // Add role only if user creation is successful
            var roleResult = await _userManager.AddToRoleAsync(newUser, GlobalConstants.AdminRoleName);
            if (!roleResult.Succeeded){
                // Optionally handle errors related to role assignment
                return new ResultModel<AppUser>{
                    Errors = new List<string>(roleResult.Errors.Select(x => x.Description))
                };
            }
           
            newUser = await _userManager.FindByNameAsync(newUser.UserName);
           
            return new ResultModel<AppUser>{
                Data = newUser
            };
        }
//Het loginproces is gericht op authenticatie en het bepalen van de toegangsrechten van de gebruiker tijdens die sessie.
//Tijdens het loginproces worden claims gegenereerd en toegevoegd aan het JWT
//Deze claims kunnen de gebruikersidentiteit, rollen en andere relevante attributen bevatten die nodig zijn
//voor autorisatiebeslissingen tijdens de sessie.
        public async Task<ResultModel<string>> Login(string username, string password){
            var user = await _userManager.FindByNameAsync(username);
            if (user == null){
                return new ResultModel<string>{
                    Errors = new List<string>{"Invalid username or password"}
                };
            }
            
            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);
            if (!result.Succeeded){
                return new ResultModel<string>{
                    Errors = new List<string>{"Invalid login attempt."}
                };
            }
           
            JwtSecurityToken token = await _tokenService.GenerateTokenAsync(user);
            string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new ResultModel<string>{
                Data = serializedToken
            };
        }
}