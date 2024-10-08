

using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Mde.Project.Mobile.WebAPI.Controllers;

[Route("api/enums")]
public class EnumsController : Controller{
    
    private readonly IEnumService _enumService;
    public EnumsController(IEnumService enumService){
        _enumService = enumService;
    }
    [HttpGet("accessLevel")]
    public async Task<ActionResult> GetAccessLevel(){
        
        var values = await _enumService.GetAccessLevelTypeAsync();
        return Ok(values);
    }
   

}