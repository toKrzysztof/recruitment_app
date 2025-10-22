using Microsoft.EntityFrameworkCore;
using RecruitmentApp.Features.Contacts.Domain;

namespace RecruitmentApp.Features.Contacts.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Contact> Contacts { get; set; }
}
