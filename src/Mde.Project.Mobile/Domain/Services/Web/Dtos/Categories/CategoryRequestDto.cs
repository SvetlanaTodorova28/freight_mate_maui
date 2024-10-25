using System.ComponentModel.DataAnnotations;

namespace Mde.Project.Mobile.Domain.Services.Web.Dtos.Categories;

public class CategoryRequestDto:BaseDto{
    
    [Required(ErrorMessage = "{0}  is required")]
    public string Name { get; set; }
}