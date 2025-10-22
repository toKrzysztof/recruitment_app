using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.Features.Authentication.Dto;
using RecruitmentApp.Features.Authentication.Service;
using RecruitmentApp.Shared.Api;

namespace RecruitmentApp.Features.Authentication.Api.Controllers;

public class AuthController : ApiControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenService _tokenService;
    
    public AuthController(UserManager<IdentityUser> userManager, ITokenService tokenService)
    {
        this._userManager = userManager;
        this._tokenService = tokenService;
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
        
        var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

        if (identityResult.Succeeded)
        {
            return Ok("Registration Successful.");
        }
        
        return BadRequest("Registration Failed.");
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);

        if (user == null)
        {
            return BadRequest("Invalid username or password.");
        }
        
        var passwordValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (!passwordValid)
        {
            return BadRequest("Invalid username or password.");
        }
        
        var token = _tokenService.CreateJwtToken(user);

        var response = new LoginResponseDto
        {
            JwtToken = token
        };
        
        return Ok(response);
    }
}