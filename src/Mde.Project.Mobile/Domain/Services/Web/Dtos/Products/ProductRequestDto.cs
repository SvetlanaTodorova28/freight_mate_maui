using System.ComponentModel.DataAnnotations;

namespace Mde.Project.Mobile.Domain.Services.Web.Dtos.Products;

public class ProductRequestDto:BaseDto{
    
    [Required(ErrorMessage = "{0} is required")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "{0} is required")]
    public Guid CategoryId { get; set; }

}