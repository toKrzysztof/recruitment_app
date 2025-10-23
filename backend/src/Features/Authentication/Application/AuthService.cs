using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RecruitmentApp.Features.Authentication.Application.Contracts;
using RecruitmentApp.Features.Authentication.Dto;
using RecruitmentApp.Shared.Application;

namespace RecruitmentApp.Features.Authentication.Application;

public class AuthService : IAuthService
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
        var validationErrors = new List<string>();

        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.Email,
            Email = registerRequestDto.Email
        };

        var existingUser = await _userManager.FindByEmailAsync(registerRequestDto.Email);

        if (existingUser != null)
        {
            validationErrors.Add("This email is already taken.");
        }

        var password = registerRequestDto.Password;

        if (password.Length < 8)
        {
            validationErrors.Add($"The password must be at least 8 characters long.");
        }

        if (!password.Any(char.IsDigit))
        {
            validationErrors.Add("The password must contain at least one numeric character.");
        }

        if (password.All(char.IsLetterOrDigit))
        {
            validationErrors.Add("The password must contain at least one non-alphanumeric character.");
        }

        if (validationErrors.Any())
        {
            return ServiceResult<RegisterResponseDto>.Failure(validationErrors);
        }

        var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

        if (identityResult.Succeeded)
        {
            var registerResponseDto = new RegisterResponseDto
            {
                Id = identityUser.Id,
            };

            return ServiceResult<RegisterResponseDto>.Success(registerResponseDto);
        }

        return ServiceResult<RegisterResponseDto>.Failure(["Registration Failed."]);
    }

    public async Task<ServiceResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

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
