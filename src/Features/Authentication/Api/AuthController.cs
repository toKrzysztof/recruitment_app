using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.Features.Contacts.Dto;
using RecruitmentApp.Shared.Api.Base;

namespace RecruitmentApp.Features.Authentication.Api;

public class AuthController : ApiControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    
    public AuthController(UserManager<IdentityUser> userManager)
    {
        this.userManager = userManager;
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Username
        };
        
        var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

        if (identityResult.Succeeded)
        {
            return Ok("Registration Successful.");
        }
        
        return BadRequest("Registration Failed.");
    }
}