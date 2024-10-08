



using Mde.Project.Mobile.WebAPI.Enums;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;

namespace Mde.Project.Mobile.WebAPI.Core.Services;

public class EnumService:IEnumService{
    

    public  async Task<IEnumerable<string>> GetAccessLevelTypeAsync(){
        var accesLevels = Enum.GetValues(typeof(AccessLevelType))
            .Cast<AccessLevelType>()
            .Select(item => item.ToText());
        return accesLevels;
    }
}