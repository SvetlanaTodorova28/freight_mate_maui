

using Mde.Project.Mobile.WebAPI.Dtos.Cargos;
using Mde.Project.Mobile.WebAPI.Dtos.Categories;
using Mde.Project.Mobile.WebAPI.Dtos.Products;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities;


namespace Mde.Project.Mobile.WebAPI.Api.Controllers;

//required:
//List of vehicles
//List of products

[Route("api/[controller]")]
[ApiController]
public class CargosController : ControllerBase{
    private readonly ICargoService _cargoService;
    private readonly IProductService _productService;
   

    public CargosController(ICargoService cargoService, IProductService productService){
        _cargoService = cargoService;
        _productService = productService;
        
    }
    //========================================= GET REQUESTS ==============================================================
    
    
    /// <summary>
    /// Retrieves all Cargo entities from the database.
    /// </summary>
    /// <returns>An IActionResult containing a list of CargoResponseDto objects or an error message.</returns>
    [HttpGet]
    public async Task<IActionResult> Get(){

        // Calls the GetAllAsync method from the ICargoService interface
        var result = await _cargoService.GetAllAsync();

        // Checks if the operation was successful
        if (result.Success){

            // Maps the Cargo entities to CargoResponseDto objects
            var cargosDtos = result
                .Data
                .Select(x => new CargoResponseDto(){
                    Id = x.Id,
                   Destination = x.Destination,
                   IsDangerous = x.IsDangerous,
                    TotalWeight = x.TotalWeight?? 0,
                    // Maps the Products of each Cargo to ProductResponseDto objects
                    ProductsIds = x.Products.Select(p => p.Id).ToList()
                });

            // Returns the list of CargoResponseDto objects as a successful HTTP response
            return Ok(cargosDtos);
        }

        // Returns an error message as a bad request HTTP response
        return BadRequest(result.Errors);
    }

      
    /// <summary>
    /// Retrieves a Cargo entity by its unique identifier from the database.
    /// </summary>
    /// <remarks>
    ///GET api/cargos/{id}:
    ///
    /// 
    ///     {
    ///         "00000000-0000-0000-0000-000000000031"
    ///         "00000000-0000-0000-0000-000000000032" 
    ///         "00000000-0000-0000-0000-000000000033" 
    ///     }
    /// 
    /// </remarks>
    /// <param name="id">The unique identifier of the Cargo entity to retrieve.</param>
    /// <returns>An IActionResult containing a CargoResponseDto object or an error message.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id){
        // Calls the GetByIdAsync method from the ICargoService interface

        // Checks if the Cargo entity with the given id exists in the database
        if (!await _cargoService.DoesCargoIdExistAsync(id)){
            return NotFound();
        }

        // Calls the GetByIdAsync method from the ICargoService interface to retrieve the Cargo entity
        var result = await _cargoService.GetByIdAsync(id);

        // Checks if the operation was successful
        if (result.Success){
            // Maps the Cargo entity to a CargoResponseDto object
            var cargoDto = new CargoResponseDto(){
                Id = result.Data.Id,
                Destination = result.Data.Destination,
                TotalWeight = result.Data.TotalWeight??0,
                // Maps the Products of the Cargo to ProductResponseDto objects
                ProductsIds = result.Data.Products.Select(p => p.Id).ToList()
               
              
            };

            // Returns the CargoResponseDto object as a successful HTTP response
            return Ok(cargoDto);
        }

        // Returns an error message as a bad request HTTP response
        return BadRequest(result.Errors);
    }
    
    [HttpGet("GetCargosByUser/{userId}")]
    public async Task<IActionResult> GetCargosByUser(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("Invalid user ID provided.");
        }

        var result = await _cargoService.GetCargosWithUserId(userId);

        if (result.Errors != null && result.Errors.Any())
        {
            return NotFound(result.Errors);
        }

        var cargosDtos = result
            .Data
            .Select(x => new CargoResponseDto(){
                Id = x.Id,
                Destination = x.Destination,
                IsDangerous = x.IsDangerous,
                TotalWeight = x.TotalWeight?? 0,
                AppUserId = x.AppUserId,
                ProductsIds = x.Products.Select(p => p.Id).ToList()?? new List<Guid>()
            });
        return Ok(cargosDtos);
    }

    
    //========================================= POST REQUESTS ==============================================================
    
   
    /// <summary>
    /// Handles the creation of a new Cargo entity in the database.
    /// </summary>
    /// <remarks>
    /// REQUIRES:
    /// 
    ///        Id is manditory and will be automatically generated upon creation.
    /// 
    ///        Name is required and cannot be empty.
    /// 
    ///        Products  are required and cannot be empty.
    ///
    /// 
    /// ADD CARGO:
    ///
    ///    
    ///     {
    ///         "destination": "Local Cargo",
    ///         "totalWeight": 200,
    ///         "products": [
    ///             "00000000-0000-0000-0000-000000000021",
    ///             "00000000-0000-0000-0000-000000000022",
    ///             "00000000-0000-0000-0000-000000000023"
    ///        ]
    ///     }
    /// </remarks>
    /// <param name="cargoRequestDto">The CargoRequestDto object containing the data for the new Cargo entity.</param>
    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromBody] CargoRequestDto cargoRequestDto){
        // Create a new Cargo entity with the provided data
        var cargo = new Cargo{
            Destination = cargoRequestDto.Destination,
            TotalWeight = cargoRequestDto.TotalWeight,
            IsDangerous = cargoRequestDto.IsDangerous,
            AppUserId = cargoRequestDto.AppUserId
        };

        
        var resultProducts = await _productService.GetProductsByIdsAsync(cargoRequestDto.Products);
        /*if (resultProducts.Success) {
            cargo.Products.AddRange(resultProducts.Data);
        } else {
            return BadRequest(resultProducts.Errors);
        }*/
        cargo.Products.AddRange(resultProducts.Data);
        // Create the new Cargo entity in the database
        var result = await _cargoService.AddAsync(cargo);

        // Check the outcome of the operation
        if (result.Success){
            // Prepare the response data
            var cargoDto = new CargoResponseDto{
                Id = result.Data.Id,
                Destination = result.Data.Destination,
                IsDangerous = result.Data.IsDangerous,
                AppUserId = result.Data.AppUserId,
                TotalWeight = result.Data.TotalWeight ?? 0
            };
            var resultProductsDto = await _productService.GetProductsByIdsAsync(cargoRequestDto.Products);
            if (resultProductsDto.Success){
               var productsDtos = resultProductsDto.Data.Select(p => new ProductResponseDto{
                   Name = p.Name,
                   Id = p.Id
               }).ToList();
               cargoDto.ProductsIds = productsDtos.Select(p => p.Id).ToList();
            } else {
                return BadRequest(resultProductsDto.Errors);
            }
           
            
            // Return a CreatedAtAction result with the new Cargo entity's data
            return CreatedAtAction(nameof(Get), new { id = result.Data.Id }, cargoDto);
        }

        // Return a BadRequest result with the error messages
        return BadRequest(result.Errors);
    }
    
    //========================================= PUT REQUESTS ==============================================================
    /// <summary>
    /// Update the details of an existing Cargo entity in the database.
    /// </summary>
    /// <remarks>
    /// REQUIRES:
    ///
    /// Name is required and cannot be empty.
    ///
    /// TotalWeight is optional and can be null.
    ///
    /// Products can not be empty array.
    ///
    /// 
    ///
    ///UPDATE:
    /// 
    ///     {
    ///         "id":"00000000-0000-0000-0000-000000000031",
    ///         "destination": "Ukkle",
    ///         "totalWeight": 300,
    ///         "products": [
    ///             "00000000-0000-0000-0000-000000000021",
    ///             "00000000-0000-0000-0000-000000000022"
    ///             ]
    ///     }
    /// </remarks>
   
   
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update([FromBody] CargoRequestDto cargoRequestDto){
        
        // Check if the Cargo entity with the given id exists in the database
        if (!await _cargoService.DoesCargoIdExistAsync(cargoRequestDto.Id)){
            return BadRequest($"Cargo with id :'{cargoRequestDto.Id}' does not exist !");
        }
        

        /*
        var resultProducts = await _productService.GetProductsByIdsAsync(cargoRequestDto.Products);
        if (!resultProducts.Success){
            return BadRequest(resultProducts.Errors);
        }
        */
        
        var existingCargoResult = await _cargoService.GetByIdAsync(cargoRequestDto.Id);
        if (!existingCargoResult.Success){
            return BadRequest(existingCargoResult.Errors);
        }
        
        var existingCargo = existingCargoResult.Data;
        existingCargo.Destination =  cargoRequestDto.Destination;
        existingCargo.TotalWeight = cargoRequestDto.TotalWeight;
        existingCargo.IsDangerous = cargoRequestDto.IsDangerous;
        existingCargo.AppUserId = cargoRequestDto.AppUserId;
        existingCargo.Products.Clear();
       
        
        /*if (cargoRequestDto.Products != null && cargoRequestDto.Products.Any()){
            existingCargo.Products.AddRange(resultProducts.Data);
        }*/
       
        
        var result = await _cargoService.UpdateAsync(existingCargo);
        
        // Check the outcome of the operation
        if (result.Success){
            return Ok($"Cargo with id :'{cargoRequestDto.Id}' has been updated successfully!");
        }
    
        // Return a BadRequest result with the error messages
        return BadRequest(result.Errors);
    }
     /// <summary>
    /// Retrieves  an IActionResult containing a CargoRequestDto object to can use it later in a post or put method or an error message.
    /// </summary>
    /// <remarks>
    ///GET FOR UPDATING:
    ///
    ///     
    ///     {
    ///       "00000000-0000-0000-0000-000000000031"
    ///        "00000000-0000-0000-0000-000000000032" 
    ///        "00000000-0000-0000-0000-000000000033" 
    ///     }
    /// 
    /// </remarks>
    /// <param name="id">The unique identifier of the Cargo entity to retrieve.</param>
    /// <returns> An IActionResult containing a CargoRequestDto object to can use it later in a post or put method or an error message.</returns>
    [HttpGet("for-testing-{id}")]
    public async Task<IActionResult> GetByIdForUpdate(Guid id){
        // Calls the GetByIdAsync method from the ICargoService interface

        // Checks if the Cargo entity with the given id exists in the database
        if (!await _cargoService.DoesCargoIdExistAsync(id)){
            return NotFound();
        }

        // Calls the GetByIdAsync method from the ICargoService interface to retrieve the Cargo entity
        var result = await _cargoService.GetByIdAsync(id);

        // Checks if the operation was successful
        if (result.Success){
            // Maps the Cargo entity to a CargoResponseDto object
            var cargoDto = new CargoRequestDto(){
                Id = result.Data.Id,
                Destination = result.Data.Destination,
                IsDangerous = result.Data.IsDangerous,
                TotalWeight = result.Data.TotalWeight??0,
                // Maps the Products of the Cargo to ProductResponseDto objects
               Products = result.Data.Products.Select(p => p.Id).ToList(),
                  
            };

            // Returns the CargoResponseDto object as a successful HTTP response
            return Ok(cargoDto);
        }

        // Returns an error message as a bad request HTTP response
        return BadRequest(result.Errors);
    }
    //========================================= DELETE REQUESTS ==============================================================
    /// <summary>
    /// Delete a Cargo entity by its unique identifier from the database.
    /// </summary>
    /// <remarks>
    ///DELETE:
    ///
    ///     
    ///     {
    ///         "00000000-0000-0000-0000-000000000031"
    ///         "00000000-0000-0000-0000-000000000032" 
    ///         "00000000-0000-0000-0000-000000000033" 
    ///     }
    /// 
    /// </remarks>
    /// <param name="id">The unique identifier of the Cargo entity to delete.</param>
    /// <returns>An IActionResult indicating the outcome of the operation.
    /// If the Cargo entity with the given id exists, it is deleted and a success message is returned.
    /// If the Cargo entity does not exist, a NotFound result is returned.
    /// If the deletion operation fails, a BadRequest result with the error messages is returned.
    /// </returns>
    [HttpDelete("{id}")]
   
    public async Task<IActionResult> Delete(Guid id){
        // Check if the Cargo entity with the given id exists in the database
        if (!await _cargoService.DoesCargoIdExistAsync(id)){
            return NotFound($"CARGO with id :'{id}' does not exist !");
        }

        // Retrieve the existing Cargo entity from the database
        var existingCargo = await _cargoService.GetByIdAsync(id);

        // Delete the Cargo entity from the database
        var result = await _cargoService.DeleteAsync(existingCargo.Data);

        // Check the outcome of the deletion operation
        if (result.Success){
            // Return a success message
            return Ok($"Cargo with id : {existingCargo.Data.Id} deleted");
        }

        // Return a BadRequest result with the error messages
        return BadRequest(result.Errors);
    }

}