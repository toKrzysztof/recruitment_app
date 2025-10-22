using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecruitmentApp.Features.Authentication.Service;
using RecruitmentApp.Features.Contacts.Data;
using RecruitmentApp.Features.Contacts.Data.Contracts;
using RecruitmentApp.Features.Contacts.Data.Repositories;
using RecruitmentApp.Shared.Api;
using RecruitmentApp.Shared.Api.Contracts;

namespace RecruitmentApp.Extensions;

public static class ApplicationServicesExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")
            );
        });
        
        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")
            );
        });
        
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IHttpService, HttpService>();
        services.AddScoped<IContactRepository, ContactRepository>();
    }

    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters =new TokenValidationParameters
            {
                ValidateIssuer = true, 
                ValidateAudience = true, 
                ValidateLifetime = true, 
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        configuration["Jwt:Key"]))
            });
    }

    public static void ConfigureTokenManagement(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<IdentityUser>()
            .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("ChangeMe")
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();
    }
    
    public static void ConfigurePasswordPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;
        });
    }
}