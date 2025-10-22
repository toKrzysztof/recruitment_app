using Microsoft.AspNetCore.Identity;

namespace RecruitmentApp.Features.Authentication.Application;

public interface ITokenService
{
    string CreateJwtToken(IdentityUser user);
}
