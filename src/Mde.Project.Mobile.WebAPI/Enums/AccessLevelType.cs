namespace Mde.Project.Mobile.WebAPI.Enums;

public enum AccessLevelType{
    Basic, // Basisgebruiker, beperkte toegang
    Advanced, // Geavanceerde functies
    Admin, // Beheerderstoegang
}

public static class AccessLevelTypeExtensions {
    public static string ToText(this AccessLevelType accessLevelType){

    return accessLevelType switch{
    AccessLevelType.Basic => "Basic",
    AccessLevelType.Advanced => "Advanced",
    AccessLevelType.Admin => "Admin"
};

}
}