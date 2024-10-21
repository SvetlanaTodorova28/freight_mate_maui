using System.ComponentModel.DataAnnotations;

namespace Mde.Project.Mobile.Core.Service.Web.Dto;

public class CargoResponseDto:BaseDto{
    public string Name { get; set; }
    
    public double TotalWeight { get; set; }
   
    public List<ProductResponseDto> Products { get; set; }
    
}