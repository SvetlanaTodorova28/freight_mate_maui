namespace Utilities;

public class GlobalConstants{
    public const string AdminRoleId = "00000000-0000-0000-0000-000000000060";
    public const string AdminId = "00000000-0000-0000-0000-100000000000"; 
    public const string AdminRoleName = "Admin";
    public const string AdminUserName = "Admin@fedex.com";
    public const string AdminUserPassword = "Admin1234";
    
    
    public const string AdvancedRoleId = "00000000-0000-0000-0000-000000000061";
    public const string AdvancedRoleName = "Advanced";
    public const string AdvancedPassword = "Advanced1234";
    
    
    public const string BasicRoleId = "00000000-0000-0000-0000-000000000062";
    public const string BasicRoleName = "Basic";
    public const string BasicPassword = "Basic1234";
    
    public const string TokenExpirationDaysConfig = "JWTConfiguration:TokenExpirationDays";
    public const string SigningKeyConfig = "JWTConfiguration:SigningKey";
    public const string IssuerConfig = "JWTConfiguration:Issuer";
    public const string AudienceConfig = "JWTConfiguration:Audience";
    
    public const string Base = "https://6b0f-141-135-237-156.ngrok-free.app";
    public const string BaseAzure = "https://next-web-app-hhgwfndcemcgajaf.northeurope-01.azurewebsites.net/index.html";
    public const string HttpClient = "FrightMateClient";
    public const string Auth = Base + "accounts/login";
    public const string Cargos = Base + "cargos/";
    
    public const string BaseUrlFireBase = "https://fcm.googleapis.com/v1/projects/mde-project-mobile/messages:send";
    public const string HttpClientFireBase = "FireBase";

    public const string Key_Speech =
        "";

    public const string Key_Translation =
        "";
}