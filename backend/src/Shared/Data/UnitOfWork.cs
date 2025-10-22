using AutoMapper;
using RecruitmentApp.Features.Contacts.Data;
using RecruitmentApp.Features.Contacts.Data.Contracts;
using RecruitmentApp.Features.Contacts.Data.Repositories;
using RecruitmentApp.Shared.Data.Contracts;

namespace RecruitmentApp.Shared.Data;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public IContactRepository Contacts { get; } = new ContactRepository(context);

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
