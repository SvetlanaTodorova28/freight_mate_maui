
using System.Threading.Tasks;
using Mde.Project.Mobile.WebAPI.Api.Dtos;
using Mde.Project.Mobile.WebAPI.Dtos;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Utilities;


namespace Mde.Project.Mobile.WebAPI.Api.Controllers;
//Het primaire doel van de registratie is het aanmaken van een nieuwe gebruikersaccount in het systeem.
//Dit omvat het verzamelen van gebruikersgegevens, het aanmaken van een record in de database,
//en soms het toewijzen van een of meer rollen aan de gebruiker (bijvoorbeeld standaardrollen zoals "gebruiker" of "consignee").
//Rollen worden vaak toegevoegd om toegangsniveaus te bepalen, die bij latere interacties relevant zijn.

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase{
    
    private readonly IAuthenticationService _authenticationService;

    public AccountsController(IAuthenticationService authenticationService){
        _authenticationService = authenticationService;
    }
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <remarks>
    /// REQUIRES:
    ///
    /// Email is required and must be unique.
    ///
    /// Password is required and must meet certain criteria (e.g., minimum length(4), presence of uppercase (not here))
    ///
    /// ConfirmPassword is required and must match the Password.
    ///
    /// FirstName and LastName are optional.
    ///
    /// ProfilePicture is optional and should be a valid URL pointing to an image file.
    ///
    ///REGISTER:
    /// 
    ///         {
    ///             "username": "sve@dhl.com",
    ///             "email": "sve@dhl.com",
    ///             "password": "string",
    ///             "confirmPassword": "string",
    ///             "firstName": "svetlana",
    ///             "lastName": "todorova",
    ///             "accessLevelType": "string"
    /// };
    ///         }
    ///
    ///
    ///        {
   ///              "username": "deb@fedex.com",
   ///              "email": "deb@fedex.com",
    ///             "password": "string",
   ///              "confirmPassword": "string",
    ///             "firstName": "debby",
    ///             "lastName": "salen"
    ///             }   
    /// </remarks>

   
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto registerUserRequestDto){
        var userToRegister = new AppUser(){
            UserName = registerUserRequestDto.Username,
            Email = registerUserRequestDto.Username,
            FirstName = registerUserRequestDto.FirstName?? "",
            LastName = registerUserRequestDto.LastName??"",
            AccessLevelId = registerUserRequestDto.AccessLevelTypeId
           
        };

        var result = await _authenticationService.RegisterUserAsync(userToRegister, registerUserRequestDto.Password);

        if (!result.Success){
            return BadRequest(result.Errors);
        }

        return Ok(new{ message = "User successfully registered", userId = result.Data.Id });
    }

    /// <summary>
    /// Login a  user in the system.
    /// </summary>
    /// <remarks>
    /// REQUIRES:
    ///
    /// Email is required .
    ///
    /// Password is required and must meet certain criteria (e.g., minimum length(4), presence of uppercase (not here))
    ///
    ///
    ///LOGIN:
    /// 
    ///         {
    ///             "username": "Admin@fedex.com",
    ///             "password": "Admin1234"
    ///         }
    ///
    ///         {
    ///             "username": "tom@gmail.com",
    ///             "password": "Driver1234"
    ///         }
    ///
    ///         {
    ///             "username": "milka@speedy.gr",
    ///             "password": "Consignee1234"
    ///          }
    ///
    ///
    ///         
    /// </remarks>
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequestDto loginUserRequestDto){ 
        //Het loginproces is gericht op authenticatie en het bepalen van de toegangsrechten van de gebruiker tijdens die sessie.
//Tijdens het loginproces worden claims gegenereerd en toegevoegd aan het JWT
//Deze claims kunnen de gebruikersidentiteit, rollen en andere relevante attributen bevatten die nodig zijn
//voor autorisatiebeslissingen tijdens de sessie.

        var result = await _authenticationService.Login(loginUserRequestDto.Username, loginUserRequestDto.Password);
        //TO DO: add error handling at the client-side in case of invalid credentials
        if (!result.Success){
            return Unauthorized(new{ message = "Username or password is incorrect." });
        }

        return Ok(new LoginUserResponseDto{
            Token = result.Data
        });
    }



}



