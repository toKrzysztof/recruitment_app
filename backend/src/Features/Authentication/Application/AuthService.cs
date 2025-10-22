using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RecruitmentApp.Features.Authentication.Dto;
using RecruitmentApp.Shared.Application;

namespace RecruitmentApp.Features.Authentication.Application;

public class AuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<IdentityUser> userManager, IMapper mapper, ITokenService tokenService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<ServiceResult<RegisterResponseDto>> Register(RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.Email,
            Email = registerRequestDto.Email
        };

        var existingUser = await _userManager.FindByEmailAsync(registerRequestDto.Email);

        if (existingUser != null)
        {
            return ServiceResult<RegisterResponseDto>.Failure(["This email is already taken."]);
        }

        var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

        if (identityResult.Succeeded)
        {
            var registerResponseDto = _mapper.Map<RegisterResponseDto>(identityUser);
            return ServiceResult<RegisterResponseDto>.Success(registerResponseDto);
        }

        return ServiceResult<RegisterResponseDto>.Failure(["Registration Failed."]);
    }

    public async Task<ServiceResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);

        if (user == null)
        {
            return ServiceResult<LoginResponseDto>.Failure(["Invalid username or password."]);
        }

        var passwordValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (!passwordValid)
        {
            return ServiceResult<LoginResponseDto>.Failure(["Invalid username or password."]);
        }

        var token = _tokenService.CreateJwtToken(user);

        var loginResponseDto = new LoginResponseDto
        {
            JwtToken = token
        };

        return ServiceResult<LoginResponseDto>.Success(loginResponseDto);
    }
}
