using Mde.Project.Mobile.WebAPI.Dtos.Functions;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Mde.Project.Mobile.WebAPI.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccessLevelsController:ControllerBase{
    private readonly IAccessLevelService _accessLevelService;

    public AccessLevelsController(IAccessLevelService accessLevelService){
        this._accessLevelService = accessLevelService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(){

        // Calls the GetAllAsync method from the ICargoService interface
        var result = await _accessLevelService.GetAllAsync();

        // Checks if the operation was successful
        if (result.Success){

            // Maps the Cargo entities to CargoResponseDto objects
            var cargosDtos = result
                .Data
                .Select(x => new AccessLevelsResponseDto(){
                    Id = x.Id,
                   Name = x.Name,
                    }).ToList();

            // Returns the list of CargoResponseDto objects as a successful HTTP response
            return Ok(cargosDtos);
        }

        // Returns an error message as a bad request HTTP response
        return BadRequest(result.Errors);
    }
}