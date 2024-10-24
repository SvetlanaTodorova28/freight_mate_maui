
using System.Text;
using Mde.Project.Mobile.WebAPI.Api.Dtos;
using Mde.Project.Mobile.WebAPI.Api.Dtos.Users;
using Mde.Project.Mobile.WebAPI.Dtos.Cargos;
using Mde.Project.Mobile.WebAPI.Dtos.Functions;
using Mde.Project.Mobile.WebAPI.Entities;

using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Utilities;


namespace Mde.Project.Mobile.WebAPI.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppUserController:ControllerBase{
   
    private readonly UserManager<AppUser> _userManager;
    private readonly IAppUserService _userService;
    
    

    public AppUserController(IAppUserService userService , UserManager<AppUser> userManager) {
    
        _userService = userService;
        _userManager = userManager;
        
       
        
    }
    
    //========================================= GET REQUESTS ==============================================================
    /// <summary>
    /// Retrieves a list of all users from the database.
    /// </summary>
    /// <returns>
    /// An ActionResult containing a list of UserResponsDto objects representing the users.
    /// If the operation is successful, the HTTP status code will be 200 (Ok).
    /// </returns>
    [HttpGet()]
    [Authorize(Policy = GlobalConstants.AdminRoleName)] 
    
    public async Task<ActionResult<IEnumerable<UserResponsDto>>> GetUsers()
    {
        // Fetch all users from the database
        var users = await _userManager
            .Users
            .ToListAsync();
        
        // Map the user entities to UserResponsDto objects
        var userDtos = users
            .Select(user => new UserResponsDto {
                Id = Guid.Parse(user.Id),
                UserName = user.UserName,
                FirstName = user.FirstName
            });

        // Return the list of UserResponsDto objects as Ok result
        return Ok(userDtos);
    }

    /// <summary>
    /// Retrieves a AppUser entity by its unique identifier from the database.
    /// </summary>
    /// <remarks>
    ///GET api/cargos/{id}:
    ///
    /// 
    ///     {
    ///         "00000000-0000-0000-0000-200000000000"
    ///         "00000000-0000-0000-0000-300000000000" 
    ///         "00000000-0000-0000-0000-400000000000" 
    ///     }
    /// 
    /// </remarks>
    /// <param name="id">The unique identifier of the AppUser entity to retrieve.</param>
    /// <returns>An IActionResult containing a AppUserResponseDto object or an error message.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponsDto>> GetUserById(Guid id){

        var user = await _userManager
            .Users
            .Include(u => u.Cargos)
            .FirstOrDefaultAsync(u => u.Id == id.ToString());


        if (user == null){
            return NotFound("User not found");
        }

        var userDto = new UserResponsDto{
            Id = Guid.Parse(user.Id),
            Email = user.Email,
            UserName = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AccessLevelType = new AccessLevelsResponseDto{
                Name = user.AccessLevel.Name
            },
            Cargos = user.Cargos.Select(c => new CargoResponseDto()
            {
                Id = c.Id,
                Destination = c.Destination,
                TotalWeight = c.TotalWeight
            }).ToList()
            
            
        };
        return Ok(userDto);
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
    public async Task<IActionResult> Add([FromBody] UserCreateRequestDto model){

        var user = new AppUser{
            Email = model.Email,
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            AccessLevelId = model.AccessLevelTypeId
        };
        
        var result = await _userService.CreateUserAsync(user, model.Password);

        if (!result.Success){
            return BadRequest(result.Errors);
        }
       // await SendEmailAsync(result.Data);

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
     
        
        var existingUser = await _userManager.FindByIdAsync(changePasswordDto.Id.ToString());
        if (existingUser == null){
            return NotFound("User not found.");
        }
       

        var result = await _userManager.ResetPasswordAsync(existingUser, changePasswordDto.Token, changePasswordDto.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { message = "Password has been successfully reset." });
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
        var existingUser = await _userManager
            .Users
            .FirstOrDefaultAsync( u => u.Id == userUpdateRequestDto.Id.ToString());
        if (existingUser == null)
        {
            return NotFound($"No user with id '{userUpdateRequestDto.Id}' found.");
        }

        // Update user properties
        existingUser.UserName = userUpdateRequestDto.Email;
        existingUser.Email = userUpdateRequestDto.Email;
        existingUser.FirstName = userUpdateRequestDto.FirstName;
        existingUser.LastName = userUpdateRequestDto.LastName;
        existingUser.AccessLevelId = userUpdateRequestDto.AccessLevelTypeId;
        
           
    

        var updateResult = await _userManager.UpdateAsync(existingUser);
        if (!updateResult.Succeeded)
        {
            return BadRequest(updateResult.Errors);
        }

        return Ok(new { message = "User successfully updated" });
    }

    //========================================= PUT REQUESTS ==============================================================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id){
        var existingUser = await _userManager.FindByIdAsync(id.ToString());
        if (existingUser == null){
            return NotFound($"No user with id '{id}' found.");
        }
        
        var result = await _userManager.DeleteAsync(existingUser);
        if (result.Succeeded){
            return Ok(new{ message = "User successfully deleted" });
        }
        return BadRequest(result.Errors);
    }
    
    
   
}