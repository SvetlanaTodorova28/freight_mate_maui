
using Mde.Project.Mobile.WebAPI.Dtos.Categories;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using WebApplication1.Dtos.Categories;


namespace Mde.Project.Mobile.WebAPI.Api.Controllers;



    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase{
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService){
            _categoryService = categoryService;
        }
        
        /// <summary>
        /// Retrieves all Categories entities from the database.
        /// </summary>
        /// <returns>An IActionResult containing a list of CategoryResponseDto objects or an error message.</returns>
        [HttpGet]
        public async Task<IActionResult> Get(){
            /// <summary>
            /// This property always returns a value &lt; 1.
            /// </summary>
            // Calls the GetAllAsync method from the ICategoryService interface
            var result = await _categoryService.GetAllAsync();

            // Checks if the operation was successful
            if (result.Success){
                // Maps the Category entities to CategoryResponseDto objects
                var categoriesDtos = result
                    .Data
                    .Select(x => new CategoryResponseDto(){
                        Id = x.Id,
                        Name = x.Name,
                    });

                // Returns a Ok with the list of CategoryResponseDto objects
                return Ok(categoriesDtos);
            }

            // Returns a BadRequest with the list of error messages
            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Retrieves a Category entity by its unique identifier from the database.
        /// </summary>
        /// <remarks>
        ///GET api/categories/{id}:
        /// 
        ///     {
        ///         "00000000-0000-0000-0000-000000000011"
        ///         "00000000-0000-0000-0000-000000000012"
        ///         "00000000-0000-0000-0000-000000000013"
        ///     }
        /// 
        /// </remarks>
        /// <param name="id">The unique identifier of the Category entity to retrieve.</param>
        /// <returns>An IActionResult containing a CategoryResponseDto object or an error message.</returns>
        [HttpGet("{id}")]
       
        public async Task<IActionResult> GetById(Guid id){
            // Check if the Category entity with the given id exists
            if (!await _categoryService.DoesCategoryIdExistAsync(id)){
                return NotFound();
            }

            // Retrieve the Category entity with the given id
            var result = await _categoryService.GetByIdAsync(id);

            // Check if the retrieval operation was successful
            if (result.Success){
                // Create a CategoryResponseDto object from the retrieved Category entity data
                var categoryDto = new CategoryResponseDto(){
                    Id = result.Data.Id,
                    Name = result.Data.Name,
                };

                // Return an Ok result with the CategoryResponseDto object
                return Ok(categoryDto);
            }

            // Return a BadRequest result with the list of error messages
            return BadRequest(result.Errors);
        }
        //========================================= POST REQUESTS ==============================================================
        
        /// <summary>
        /// Handles the creation of a new Category entity in the database.
        /// </summary>
        /// <remarks>
        /// REQUIRES:
        /// 
        ///        Id is manditory and will be automatically generated upon creation.
        /// 
        ///        Name is required and cannot be empty.
        /// 
        /// ADD CATEGORY:
        ///
        ///    
        ///     {
        ///         "name": "Animal Foods"
        ///     }
        /// </remarks>
        /// <param name="cargoRequestDto">The CategoryRequestDto object containing the data for the new Category entity.</param>
        [HttpPost]
        [Authorize(Roles = "Consignee")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequestDto categoryRequestDto){
            
            // Create a new Category entity from the CreateCategoryDto object
            var category = new Category{
                Id = categoryRequestDto.Id,
                Name = categoryRequestDto.Name
            };

            // Save the Category entity to the database
            var result = await _categoryService.AddAsync(category);

            // Check if the save operation was successful
            if (result.Success){
                // Create a CategoryResponseDto object from the saved Category entity data
                var categoryDto = new CategoryResponseDto(){
                    Id = result.Data.Id,
                    Name = result.Data.Name
                };

               
                return CreatedAtAction(nameof(GetById), new { id = categoryDto.Id }, categoryDto);
            }

            // Return a BadRequest result with the list of error messages
            return BadRequest(result.Errors);
        }
        
        //========================================= PUT REQUESTS ==============================================================
        /// <summary>
        /// Update the details of an existing Carrier entity in the database.
        /// </summary>
        /// <remarks>
        /// REQUIRES:
        /// 
        ///        Id is manditory and will be automatically generated upon creation.
        /// 
        ///        Name is required and cannot be empty.
        ///
        /// UPDATE CATEGORY:
        /// 
        ///     {
        ///         "name": "Please",
        ///         "id": "00000000-0000-0000-0000-000000000011"
        ///     }
        /// 
        /// </remarks>
        [HttpPut("{id}")]
        [Authorize(Policy = GlobalConstants.ConsigneeRoleName)]
        public async Task<IActionResult> Update(CategoryRequestDto categoryRequestDto){
            if (!await _categoryService.DoesCategoryIdExistAsync(categoryRequestDto.Id)){
                return BadRequest($"Category with id :'{categoryRequestDto.Id}' does not exist!");
            }

            var existingCategoryResult = await _categoryService.GetByIdAsync(categoryRequestDto.Id);
            if (!existingCategoryResult.Success){
                return BadRequest(existingCategoryResult.Errors);
            }
        
            var existingCategory = existingCategoryResult.Data;
            existingCategory.Name = categoryRequestDto.Name;
           
            var result = await _categoryService.UpdateAsync(existingCategory);
        
            // Check the outcome of the operation
            if (result.Success){
                return Ok($"Category with id :'{categoryRequestDto.Id}' has been updated successfully!");
            }
    
            // Return a BadRequest result with the error messages
            return BadRequest(result.Errors);
        }
        
        /// <summary>
        /// Retrieves  an IActionResult containing a CartegoryRequestDto object to can use it later in a post or put method or an error message.
        /// </summary>
        /// <remarks>
        ///GET CATEGORY FOR UPDATE:
        ///
        ///     
        ///     {
        ///        "00000000-0000-0000-0000-000000000011"
        ///        "00000000-0000-0000-0000-000000000012" 
        ///        "00000000-0000-0000-0000-000000000013" 
        ///     }
        /// 
        /// </remarks>
        /// <param name="id">The unique identifier of the Cartegory entity to retrieve.</param>
        /// <returns> An IActionResult containing a CartegoryRequestDto object to can use it later in a post or put method or an error message.</returns>
        [HttpGet("for-testing-{id}")]
       
        public async Task<IActionResult> GetByIdForUpdate(Guid id){
            // Calls the GetByIdAsync method from the ICategoryService interface

            // Checks if the Category entity with the given id exists in the database
            if (!await _categoryService.DoesCategoryIdExistAsync(id)){
                return NotFound();
            }

            // Calls the GetByIdAsync method from the ICarrierService interface to retrieve the Category entity
            var result = await _categoryService.GetByIdAsync(id);
            
            // Returns an error message as a bad request HTTP response
            return BadRequest(result.Errors);
        }
        //========================================= DELETE REQUESTS ==============================================================
        /// <summary>
        /// Delete a Category entity by its unique identifier from the database.
        /// </summary>
        /// <remarks>
        ///DELETE CATEGORY:
        ///
        /// 
        ///     {
        ///         "00000000-0000-0000-0000-000000000011"
        ///         "00000000-0000-0000-0000-000000000012" 
        ///         "00000000-0000-0000-0000-000000000013" 
        ///     }
        /// 
        /// </remarks>
        /// <param name="id">The unique identifier of the Category entity to delete.</param>
        /// <returns>An IActionResult indicating the outcome of the operation.
        /// If the Category entity with the given id exists, it is deleted and a success message is returned.
        /// If the Category entity does not exist, a NotFound result is returned.
        /// If the deletion operation fails, a BadRequest result with the error messages is returned.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id){
            // Check if the Category entity with the given id exists in the database
            if (!await _categoryService.DoesCategoryIdExistAsync(id))
            {
                return NotFound($"Category with id :'{id}' does not exist !");
            }

            // Retrieve the existing Category entity from the database
            var existingCategory = await _categoryService.GetByIdAsync(id);

            // Delete the Category entity from the database
            var result = await _categoryService.DeleteAsync(existingCategory.Data);

            // Check the outcome of the deletion operation
            if (result.Success){
                // Return a success message
                return Ok($"Category with id : {existingCategory.Data.Id} deleted");
            }

            // Return a BadRequest result with the error messages
            return BadRequest(result.Errors);
        }
    }
    
   
