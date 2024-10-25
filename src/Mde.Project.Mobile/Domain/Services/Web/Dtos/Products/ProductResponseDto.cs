

using Mde.Project.Mobile.Domain.Services.Web.Dtos.Categories;

namespace Mde.Project.Mobile.Domain.Services.Web.Dtos.Products;

public class ProductResponseDto:BaseDto{
    public string Name { get; set; }
    
    public CategoryResponseDto Category { get; set; }
    
}