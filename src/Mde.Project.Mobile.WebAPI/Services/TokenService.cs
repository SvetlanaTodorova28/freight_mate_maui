using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Utilities;

namespace Mde.Project.Mobile.WebAPI.Services;

public class TokenService:ITokenService{
    private readonly IClaimsService _claimservice;
    private readonly IConfiguration _configuration;

    public TokenService(IClaimsService claimservice,
        IConfiguration configuration){
        
        _claimservice = claimservice;
        _configuration = configuration;
    }

    public async Task<JwtSecurityToken> GenerateTokenAsync(AppUser user){
       
        var claims = await _claimservice.GenerateClaimsForUser(user);
         
        var expirationDays = _configuration.GetValue<int>(GlobalConstants.TokenExpirationDaysConfig);
        var siginingKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>(GlobalConstants.SigningKeyConfig));
        var token = new JwtSecurityToken (
            issuer: _configuration.GetValue<string>(GlobalConstants.IssuerConfig),
            audience: _configuration.GetValue<string>(GlobalConstants.AudienceConfig),
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(expirationDays)),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(siginingKey), SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

}