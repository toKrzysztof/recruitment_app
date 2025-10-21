using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace RecruitmentApp.Features.Authentication.Service;

public class TokenService : ITokenService
{
    private readonly IConfiguration configuration;
    
    TokenService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    
    public string CreateJWTToken(IdentityUser user)
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Email, user.Email) };

        var jwtKey = configuration["Jwt:Key"];
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}