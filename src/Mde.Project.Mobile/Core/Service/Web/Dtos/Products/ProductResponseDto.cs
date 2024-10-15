using System.ComponentModel.DataAnnotations;

namespace Mde.Project.Mobile.Core.Service.Web.Dto;

public class ProductResponseDto:BaseDto{
    public string Name { get; set; }
    
    public CategoryResponseDto Category { get; set; }
    
}