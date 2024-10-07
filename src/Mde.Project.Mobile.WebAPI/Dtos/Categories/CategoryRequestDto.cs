using System.ComponentModel.DataAnnotations;
using Mde.Project.Mobile.WebAPI.Dtos;

namespace WebApplication1.Dtos.Categories;

public class CategoryRequestDto:BaseDto{
    
    [Required(ErrorMessage = "{0}  is required")]
    public string Name { get; set; }
}