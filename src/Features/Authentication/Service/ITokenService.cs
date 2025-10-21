using Microsoft.AspNetCore.Identity;

namespace RecruitmentApp.Features.Authentication.Service;

public interface ITokenService
{
    string CreateJWTToken(IdentityUser user);
}