using Microsoft.EntityFrameworkCore;
using RecruitmentApp.Features.Contacts.Data.Contracts;
using RecruitmentApp.Features.Contacts.Domain;
using RecruitmentApp.Shared.Data;

namespace RecruitmentApp.Features.Contacts.Data.Repositories;

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;
    public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await DbSet.FirstOrDefaultAsync(category => category.Name.ToLower() == name.ToLower());
    }
}
