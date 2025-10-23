using Microsoft.EntityFrameworkCore;
using RecruitmentApp.Features.Contacts.Data.Contracts;
using RecruitmentApp.Features.Contacts.Domain;
using RecruitmentApp.Shared.Data;

namespace RecruitmentApp.Features.Contacts.Data.Repositories;

public class SubcategoryRepository : RepositoryBase<Subcategory>, ISubcategoryRepository
{
    private readonly ApplicationDbContext _dbContext;
    public SubcategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Subcategory?> GetByName(string name)
    {
        return await DbSet.FirstOrDefaultAsync(subcategory => subcategory.Name.ToLower() == name.ToLower());
    }
}
