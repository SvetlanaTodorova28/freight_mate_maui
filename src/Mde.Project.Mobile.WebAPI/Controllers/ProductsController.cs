
using Mde.Project.Mobile.WebAPI.Dtos.Cargos;
using Mde.Project.Mobile.WebAPI.Dtos.Categories;
using Mde.Project.Mobile.WebAPI.Dtos.Products;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities;


namespace Mde.Project.Mobile.WebAPI.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
/// <summary>
/// This controller handles HTTP requests related to products.
/// </summary>
public class ProductsController : Controller
{
  private readonly IProductService _productService;
  private readonly ICategoryService _categoryService;
  private readonly ICargoService _cargoService;

  /// <summary>
  /// Initializes a new instance of the <see cref="ProductsController"/> class.
  /// </summary>
  /// <param name="productService">The service for managing products.</param>
  /// <param name="categoryService">The service for managing categories.</param>
  /// <param name="cargoService">The service for managing cargos.</param>
  /// <param name="dangerousGoodService">The service for managing dangerous goods.</param>
  public ProductsController(IProductService productService, ICategoryService categoryService, 
    ICargoService cargoService)
  {
    _productService = productService;
    _categoryService = categoryService;
    _cargoService = cargoService;
    
  }

  /// <summary>
  /// Retrieves a list of all products.
  /// </summary>
  /// <returns>An <see cref="IActionResult"/> representing the result of the action.</returns>
  [HttpGet]
  public async Task<IActionResult> Get()
  {
    var result = await _productService.GetAllAsync();
    if (result.Success)
    {
      var products = result
        .Data.Select(p => new ProductResponseDto
        {
          Id = p.Id,
          Name = p.Name,
          Category = p.Category != null ? new CategoryResponseDto
          {
            Id = p.Category.Id,
            Name = p.Category.Name
          } : null
        }).ToList();
      return Ok(products);
    }
    return BadRequest(result.Errors);
  }

 
  /// <summary>
/// Retrieves a product by its unique identifier.
/// </summary>
/// <remarks>
///
/// GET PRODUCT /Products/{id}:
///
/// 
///     {
///         "00000000-0000-0000-0000-000000000021"
///         "00000000-0000-0000-0000-000000000022"
///         "00000000-0000-0000-0000-000000000023"
///     }
/// 
/// </remarks>
/// <param name="id">The unique identifier of the product to retrieve.</param>
/// <returns>An <see cref="IActionResult"/> representing the result of the action.
/// If the product is found, the action returns an <see cref="OkObjectResult"/> containing the product data.
/// If the product is not found, the action returns a <see cref="NotFoundResult"/>.
/// If an error occurs, the action returns a <see cref="BadRequestObjectResult"/> containing the error details.
/// </returns>
[HttpGet("{id}")]
public async Task<IActionResult> GetById(Guid id)
{
    if (!await _productService.DoesProductIdExistAsync(id)){
        return NotFound();
    }

    var result = await _productService.GetByIdAsync(id);
    
    if (result.Success){
        var product = new ProductResponseDto(){
          Id = result.Data.Id,
          Name = result.Data.Name,
         
          Category = new CategoryResponseDto{
            Id = result.Data.Category.Id,
            Name = result.Data.Category.Name
          }
        };
        return Ok(product);
      
    }
    return BadRequest(result.Errors);
}
  //========================================= POST REQUESTS ==============================================================

  /// <summary>
  /// Handles the creation of a new Product entity in the database.
  /// </summary>
  /// <remarks>
  /// REQUIREMENTS:
  /// 
  /// Name is required and cannot be empty.
  ///
  ///
  /// CategoryId is required and cannot be empty.
  ///
  ///
  /// Cargos can be empty array or a list of valid CargoIds.
  ///       
  /// ADD PRODUCT:
  ///
  ///    
  ///     {
  ///         "name": "Laptop",
  ///         "categoryId":"00000000-0000-0000-0000-000000000011",
  ///         "cargos": [
  ///             "00000000-0000-0000-0000-000000000031",
  ///             "00000000-0000-0000-0000-000000000032"
  ///          ]   
  ///     }
  /// 
  /// </remarks>
  /// <param name="productRequestDto">The ProductRequestDto object containing the data for the new Product entity.</param>
  [HttpPost]
  [Authorize(Policy = GlobalConstants.AdvancedAccessLevelPolicy)]
  public async Task<IActionResult> Add([FromBody] ProductRequestDto productRequestDto){

    var product = new Product{
      Name = productRequestDto.Name,
      CategoryId = productRequestDto.CategoryId
    };
    var cargosResult = await _cargoService.GetCargosByIdsAsync(productRequestDto.Cargos);
    if (!cargosResult.Success){
      return BadRequest(cargosResult.Errors);
    }

    product.Cargos.AddRange(cargosResult.Data);
    
    var result = await _productService.AddAsync(product);
    
    if (result.Success){
      var resultCategory = await _categoryService.GetByIdAsync(result.Data.CategoryId);
      if (resultCategory.Success){
        var productResponseDto = new ProductResponseDto{
          Id = result.Data.Id,
          Name = result.Data.Name,
          Category = new CategoryResponseDto{
            Id = resultCategory.Data.Id,
            Name = resultCategory.Data.Name
          }
        };
      
        //don't must to check if cargos exist, because they are created by other requests
        var cargosIdsForResponse = result.Data.Cargos.Select(c => c.Id);
        var resultCargos = await _cargoService
          .GetCargosByIdsAsync(cargosIdsForResponse);
        if (resultCargos.Success){
          productResponseDto.Cargos = resultCargos.Data.Select(c => new CargoResponseDto{
            Id = c.Id,
            Destination = c.Destination
          }).ToList();
        }else {
          return BadRequest(resultCargos.Errors);
        }
        return CreatedAtAction("GetById", new { id = result.Data.Id }, productResponseDto);
      }
    }
    return BadRequest(result.Errors);
  }
  
  //========================================= PUT REQUESTS ==============================================================
  
  /// <summary>
  /// Handles the update of an existing Product entity in the database. 
  /// </summary>
  /// <remarks>
  ///REQUIREMENTS:
  ///
  ///
  /// Name is required and cannot be empty.
  ///
  ///
  /// CategoryId is required and cannot be empty.
  ///
  ///
  /// Cargos can be empty array or a list of valid CargoIds.
  ///
  ///
  /// UPDATE PRODUCT:
  /// 
  ///      {
  ///         "name": "Stuf",
  ///         "categoryId": "00000000-0000-0000-0000-000000000011",
  ///         "cargos": ["00000000-0000-0000-0000-000000000031",
  ///               "00000000-0000-0000-0000-000000000032"
  ///                   ],
  ///         "id": "00000000-0000-0000-0000-000000000023"
  ///      }
  ///
  /// </remarks>
  /// 
  /// <param name="productUpdateRequestDto"></param>
  /// <returns></returns>
   [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] ProductRequestDto productUpdateRequestDto){
        
        // Check if the Product entity with the given id exists in the database
        if (!await _productService.DoesProductIdExistAsync(productUpdateRequestDto.Id)){
            return BadRequest($"Product with id :'{productUpdateRequestDto.Id}' does not exist !");
        }

        if (!await _categoryService.DoesCategoryIdExistAsync(productUpdateRequestDto.CategoryId)){
          return BadRequest($"Category with id :'{productUpdateRequestDto.CategoryId}' does not exist !");
        }
       
    
        
        
        
        var existingProductResult = await _productService.GetByIdAsync(productUpdateRequestDto.Id);
        if (!existingProductResult.Success){
            return BadRequest(existingProductResult.Errors);
        }
        
        var existingProduct = existingProductResult.Data;
        existingProduct.Name = productUpdateRequestDto.Name;
       
        
        existingProduct.CategoryId = productUpdateRequestDto.CategoryId;
        
    
        if (productUpdateRequestDto.Cargos == null){
          existingProduct.Cargos.Clear();
        }else{
          var resultCargos = await _cargoService.GetCargosByIdsAsync(productUpdateRequestDto.Cargos);
          if (!resultCargos.Success){
            return BadRequest(resultCargos.Errors);
          }
          existingProduct.Cargos.Clear();
          existingProduct.Cargos.AddRange(resultCargos.Data);
        }
    
        
       
        var result = await _productService.UpdateAsync(existingProduct);
        
        // Check the outcome of the operation
        if (result.Success){
            return Ok($"Product with id :'{productUpdateRequestDto.Id}' has been updated successfully!");
        }
    
        // Return a BadRequest result with the error messages
        return BadRequest(result.Errors);
    }
    //========================================= HELP GET  METHOD ==============================================================
  /// <summary>
  /// Retrieves  an IActionResult containing a ProductRequestDto object to can use it later in a post or put method or an error message.
  /// </summary>
  /// <remarks>
  ///GET FOR UPDATE:
  ///
  ///     
  ///     {
  ///        "00000000-0000-0000-0000-000000000021"
  ///        "00000000-0000-0000-0000-000000000022" 
  ///        "00000000-0000-0000-0000-000000000023" 
  ///     }
  /// 
  /// </remarks>
  /// <param name="id">The unique identifier of the Product entity to retrieve.</param>
  /// <returns> An IActionResult containing a ProductRequestDto object to can use it later in a post or put method or an error message.</returns>
    [HttpGet("for-testing-{id}")]
    public async Task<IActionResult> GetByIdForUpdate(Guid id){
        // Calls the GetByIdAsync method from the ICargoService interface

        // Checks if the Cargo entity with the given id exists in the database
        if (!await _productService.DoesProductIdExistAsync(id)){
            return NotFound();
        }

        // Calls the GetByIdAsync method from the ICargoService interface to retrieve the Cargo entity
        var result = await _productService.GetByIdAsync(id);

        // Checks if the operation was successful
        if (result.Success){
            // Maps the Cargo entity to a CargoResponseDto object
            var cargoDto = new ProductRequestDto(){
                Id = result.Data.Id,
                Name = result.Data.Name,
                CategoryId = result.Data.CategoryId,
                Cargos = result.Data.Cargos.Select(c => c.Id).ToList() // gets ids of cargos for mapping
                
            };

            // Returns the CargoResponseDto object as a successful HTTP response
            return Ok(cargoDto);
        }

        // Returns an error message as a bad request HTTP response
        return BadRequest(result.Errors);
    }
  //========================================= DELETE REQUESTS ==============================================================
  /// <summary>
  /// Delete a Product entity by its unique identifier from the database.
  /// </summary>
  /// <remarks>
  ///DELETE PRODUCT:
  ///
  ///     
  ///     {
  ///         "00000000-0000-0000-0000-000000000021"
  ///         "00000000-0000-0000-0000-000000000022" 
  ///     }
  /// 
  /// </remarks>
  /// <param name="id">The unique identifier of the Product entity to delete.</param>
  /// <returns>An IActionResult indicating the outcome of the operation.
  /// If the Product entity with the given id exists, it is deleted and a success message is returned.
  /// If the Product entity does not exist, a NotFound result is returned.
  /// If the deletion operation fails, a BadRequest result with the error messages is returned.
  /// </returns>
  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(Guid id){
    // Check if the Product entity with the given id exists in the database
    if (!await _productService.DoesProductIdExistAsync(id)){
      return NotFound($"Product with id :'{id}' does not exist !");
    }

    // Retrieve the existing Product entity from the database
    var existingProduct = await _productService.GetByIdAsync(id);

    // Delete the Product entity from the database
    var result = await _productService.DeleteAsync(existingProduct.Data);

    // Check the outcome of the deletion operation
    if (result.Success){
      // Return a success message
      return Ok($"Destination with id : {existingProduct.Data.Id} deleted");
    }

    // Return a BadRequest result with the error messages
    return BadRequest(result.Errors);
  }
}