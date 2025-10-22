using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecruitmentApp.Features.Contacts.Data;

public class AuthDbContext : IdentityDbContext
{   
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
}