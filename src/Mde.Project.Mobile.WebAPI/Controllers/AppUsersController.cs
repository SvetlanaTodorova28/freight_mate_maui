
using System.Net;
using System.Text;
using Mde.Project.Mobile.WebAPI.Api.Dtos;
using Mde.Project.Mobile.WebAPI.Api.Dtos.Users;
using Mde.Project.Mobile.WebAPI.Dtos;
using Mde.Project.Mobile.WebAPI.Dtos.Cargos;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;



namespace Mde.Project.Mobile.WebAPI.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppUsersController:ControllerBase{
    
    private readonly IAppUserService _userService;
    private readonly IAuthenticationService _authenticationService;
    

    public AppUsersController(IAppUserService userService, IAuthenticationService authenticationService ) {
        _userService = userService;
        _authenticationService = authenticationService;
    }
    
    //========================================= GET REQUESTS ==============================================================
    /// <summary>
    /// Retrieves a list of all users from the database.
    /// </summary>
    /// <returns>
    /// An ActionResult containing a list of UserResponsDto objects representing the users.
    /// If the operation is successful, the HTTP status code will be 200 (Ok).
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUserResponseDto>>> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        if (!users.Success){
            return StatusCode((int)HttpStatusCode.NotFound, users.Errors);
        }

        var userDtos = users.Data.Select(user => new AppUserResponseDto{
            Id = Guid.Parse(user.Id),
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Cargos = user.Cargos?.Select(cargo => new CargoResponseDto
            {
                Destination = cargo.Destination,
                TotalWeight = cargo.TotalWeight,
                IsDangerous = cargo.IsDangerous,
            }).ToList(),
            AccessLevelTypeId = user.AccessLevel.Id.ToString()
            
        }).ToList();
        return Ok(userDtos);
    }

    [HttpGet("get-user-by-email/{email}")]
    public async Task<ActionResult<AppUserResponseDto>> GetUserIdByEmail([FromRoute] string email){
        var resultUser = await _userService.GetUserByEmailAsync(email);
       
        if (!resultUser.Success)
        {
            return NotFound(resultUser.Errors);
        }
        var userDto = new AppUserResponseDto{
            Id = Guid.Parse(resultUser.Data.Id),
            Email = resultUser.Data.Email,
            UserName = resultUser.Data.Email,
            FirstName = resultUser.Data.FirstName,
            LastName = resultUser.Data.LastName,
            FCMToken = resultUser.Data.FCMToken,
            AccessLevelTypeId = resultUser.Data.AccessLevel.Id.ToString()
        };
        return Ok(userDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppUserResponseDto>> GetUserById(string id){

        var resultUser = await _userService.GetUserByIdAsync(id);

        if (!resultUser.Success){
            return NotFound(resultUser.Errors);
        }

        var userDto = new AppUserResponseDto{
            Id = Guid.Parse(resultUser.Data.Id),
            Email = resultUser.Data.Email,
            UserName = resultUser.Data.Email,
            FirstName = resultUser.Data.FirstName,
            LastName = resultUser.Data.LastName,
            FCMToken = resultUser.Data.FCMToken,
            AccessLevelTypeId = resultUser.Data.AccessLevel.Id.ToString()
        };
        return Ok(userDto);
    }

    [HttpGet("get-fcm-token/{userId}")]
    public async Task<IActionResult> GetUserFcmToken([FromRoute] string userId)
    {
        var result = await _userService.GetUserFcmToken(userId);
        if (!result.Success)
        {
            return NotFound(result.Errors);
        }
        return Ok(result.Data);
    }
    
    //========================================= POST REQUESTS ==============================================================
    
    
    /// <summary>
    /// Handles the creation of a new AppUser entity in the database.
    /// </summary>
    /// <remarks>
    /// ADD APPUSER:
    ///
    ///
    ///         {
    ///         "firstName": "Milka",
    ///         "lastName": "Strauss",
    ///         "email": "milka@fedex.com",
    ///         "password": "consignor1234",
    ///         "confirmPassword": "consignor1234",
    ///         "gender": "female"
    ///         }
    ///
    ///         {
    ///             "firstName": "Phill",
    ///             "lastName": "Doctrow",
    ///             "email": "phill@glxtrans.com",
    ///             "password": "consignor1234",
    ///             "gender": "male",
    ///             "confirmPassword": "consignor1234"
    ///         }
    /// </remarks>
    /// <param name="model">The data used to create the new user.</param>
    /// <returns>
    /// An IActionResult indicating the outcome of the user creation process.
    /// If the user creation is successful, returns Ok result with a success message and the user's ID.
    /// If there are any errors during the user creation process, returns BadRequest result with the error messages.
    /// </returns>
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] RegisterUserRequestDto model){

        var userDto = new AppUser(){
            Email = model.Username,
            UserName = model.Username,
            FirstName = model.FirstName,
            LastName = model.LastName,
            AccessLevelId = Guid.Parse(model.AccessLevelTypeId)
        };
        
        var result = await _authenticationService.RegisterUserAsync(userDto, model.Password);

        if (!result.Success){
            return BadRequest(result.Errors);
        }
      

        return Ok(new { message = "User successfully registered", userId = result.Data.Id });
        
    }
    
    /// <summary>
    /// Handles the password reset process for a user.
    /// </summary>
    /// <remarks>
    /// This function takes a UserChangePasswordDto object as input, which contains the user's ID,
    /// the password reset token, and the new password.
    /// It decodes the token, finds the user in the database,
    /// and then resets the user's password using the provided token and new password.
    /// If the user is not found, a NotFound result is returned.
    /// If the password reset is successful, an Ok result is returned with a success message.
    /// If there are any errors during the password reset process, a BadRequest result is returned with the error messages.
    /// </remarks>
    /// <param name="changePasswordDto">The data used to reset the user's password.</param>
    
    [HttpPost("ResetPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] UserChangePasswordDto changePasswordDto)
    {
        var bytes = WebEncoders.Base64UrlDecode( changePasswordDto.Token);
        changePasswordDto.Token = Encoding.UTF8.GetString(bytes);
     
        

        var result = await _userService
            .UpdateUserPasswordAsync(changePasswordDto.Id.ToString()
                , changePasswordDto.Token
                , changePasswordDto.NewPassword);
        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { message = result.Data });
    }
    
    //========================================= PUT REQUESTS ==============================================================
    
    /// <summary>
    /// Updates an existing user in the database.
    /// </summary>
    /// <remarks>
    /// This method retrieves an existing user from the database using the provided user ID,
    /// then updates the user's properties with the data from the provided UserUpdateRequestDto.
    /// If the user is not found, a NotFound result is returned.
    /// If the user is successfully updated, an Ok result is returned with a success message.
    /// If there are any errors during the update process, a BadRequest result is returned with the error messages.
    /// </remarks>
    /// <param name="userUpdateRequestDto">The data used to update the user.</param>
    /// <returns>
    /// An IActionResult indicating the outcome of the update operation.
    /// If the user is not found, returns NotFound result.
    /// If the user is successfully updated, returns Ok result with a success message.
    /// If there are any errors during the update process, returns BadRequest result with the error messages.
    /// </returns>
    [HttpPut("UpdateUser")]
    public async Task<IActionResult> Update([FromBody]UserUpdateRequestDto userUpdateRequestDto)
    {
        var existingUserResult = await _userService
            .GetUserByIdAsync(userUpdateRequestDto.Id.ToString());
        if (existingUserResult == null)
        {
            return NotFound(existingUserResult.Errors);
        }
        var existingUser = existingUserResult.Data;

        // Update user properties
        existingUser.UserName = userUpdateRequestDto.Email;
        existingUser.Email = userUpdateRequestDto.Email;
        existingUser.FirstName = userUpdateRequestDto.FirstName;
        existingUser.LastName = userUpdateRequestDto.LastName;
        existingUser.AccessLevelId = userUpdateRequestDto.AccessLevelTypeId;
        
           
    

        var updateResult = await _userService.UpdateUserAsync(existingUser);
        if (!updateResult.Success)
        {
            return BadRequest(updateResult.Errors);
        }

        return Ok(new { message = "User successfully updated" });
    }

    [HttpPut("update-fcm-token/{userId}")]
    public async Task<IActionResult> UpdateUserFcmToken([FromRoute] string userId, [FromBody] string newToken)
    {
        var result = await _userService.UpdateUserFcmToken(userId, newToken);
        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Data);
    }
    //========================================= DELETE REQUESTS ==============================================================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id){

        var result = await _userService.DeleteUserAsync(id);
        if (result.Success){
            return Ok(new{ message = "User successfully deleted" });
        }
        return BadRequest(result.Errors);
    }
    
   
   



    
   

    
    
   
}