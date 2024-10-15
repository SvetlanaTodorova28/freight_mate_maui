using System.ComponentModel.DataAnnotations;

namespace Mde.Project.Mobile.Core.Service.Web.Dto;

public class CategoryRequestDto:BaseDto{
    
    [Required(ErrorMessage = "{0}  is required")]
    public string Name { get; set; }
}