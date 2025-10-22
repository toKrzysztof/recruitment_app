using Microsoft.AspNetCore.Identity;

namespace RecruitmentApp.Features.Authentication.Service;

public interface ITokenService
{
    string CreateJwtToken(IdentityUser user);
}