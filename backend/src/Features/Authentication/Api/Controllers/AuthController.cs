using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.Features.Authentication.Application.Contracts;
using RecruitmentApp.Features.Authentication.Dto;
using RecruitmentApp.Shared.Api;

namespace RecruitmentApp.Features.Authentication.Api.Controllers;

public class AuthController : ApiControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var serviceResponse = await _authService.Register(registerRequestDto);

        if (!serviceResponse.IsSuccess) return BadRequest(serviceResponse.Errors);

        return Ok(serviceResponse.Data);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var serviceResponse = await _authService.Login(loginRequestDto);

        if (!serviceResponse.IsSuccess) return BadRequest(serviceResponse.Errors);

        return Ok(serviceResponse.Data);
    }
}
