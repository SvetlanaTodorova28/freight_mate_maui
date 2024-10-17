namespace Utilities;

public class GlobalConstants{
    public const string AdminRoleId = "00000000-0000-0000-0000-000000000060";
    public const string AdminId = "00000000-0000-0000-0000-100000000000"; 
    public const string AdminRoleName = "Admin";
    public const string AdminUserName = "Admin@fedex.com";
    public const string AdminUserPassword = "Admin1234";
    
    
    public const string ConsignorRoleId = "00000000-0000-0000-0000-000000000062";
    public const string ConsignorRoleName = "Consignor";
    public const string ConsignorPassword = "Consignor1234";
        
    public const string ConsigneeRoleId = "00000000-0000-0000-0000-000000000063";
    public const string ConsigneeRoleName = "Consignee";
    public const string ConsigneePassword = "Consignee1234";
    
    public const string DriverRoleId = "00000000-0000-0000-0000-000000000061";
    public const string DriverRoleName = "Driver";
    public const string DriverPassword = "Driver1234";
    
    public const string AdvancedAccessLevelClaimType = "AdvancedAccessLevel";
    public const string AdvancedAccessLevelClaimValue = "Advanced";
    public const string AdvancedAccessLevelPolicy = "AdvancedAccessLevel";
    
    public const string TokenExpirationDaysConfig = "JWTConfiguration:TokenExpirationDays";
    public const string SigningKeyConfig = "JWTConfiguration:SigningKey";
    public const string IssuerConfig = "JWTConfiguration:Issuer";
    public const string AudienceConfig = "JWTConfiguration:Audience";
    
    public const string Base = "https://3724-81-82-247-212.ngrok-free.app";
    public const string HttpClient = "FrightMateClient";
    public const string Auth = Base + "accounts/login";
    public const string Cargos = Base + "cargos/";
}