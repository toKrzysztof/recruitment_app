using RecruitmentApp.Features.Authentication.Dto;
using RecruitmentApp.Shared.Application;

namespace RecruitmentApp.Features.Authentication.Application.Contracts;

public interface IAuthService
{
    public Task<ServiceResult<RegisterResponseDto>> Register(RegisterRequestDto registerRequestDto);
    public Task<ServiceResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto);
}
