using Microsoft.EntityFrameworkCore;
using RecruitmentApp.Features.Contacts.Api;
using RecruitmentApp.Features.Contacts.Data.Contracts;
using RecruitmentApp.Features.Contacts.Domain;
using RecruitmentApp.Shared.Data;
using RecruitmentApp.Shared.Data.Extensions;

namespace RecruitmentApp.Features.Contacts.Data.Repositories;

public class ContactRepository : RepositoryBase<Contact>, IContactRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ContactRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<Contact>> GetAllAsync(GetAllContactsQueryParams queryParams)
    {
        var query = DbSet.AsQueryable();

        // TODO - uncomment once Categories and Subcategories are added
        // if (queryParams.Category != null)
        // {
        //     query = query.Where(contact => contact.Category == queryParams.Category);
        // }
        //
        // if (queryParams.Subcategory != null)
        // {
        //     query = query.Where(contact => contact.Subcategory == queryParams.Subcategory);
        // }

        if (queryParams.LastName != null)
        {
          query = query.Where(contact => contact.LastName.ToLower().Contains(queryParams.LastName.ToLower()));
        }

        if (queryParams.FirstName != null)
        {
          query = query.Where(contact => contact.FirstName.ToLower().Contains(queryParams.FirstName.ToLower()));
        }

        if (queryParams.Email != null)
        {
          query = query.Where(contact => contact.Email.ToLower().Contains(queryParams.Email.ToLower()));
        }

        if (queryParams.Phone != null)
        {
          query = query.Where(contact => contact.PhoneNumber.Contains(queryParams.Phone));
        }

        var selectQuery = queryParams.SortBy switch
        {
          ContactsSortOrder.Lastname => queryParams.Sort == "asc"
              ? query.OrderBy(contact => contact.LastName)
              : query.OrderByDescending(contact => contact.LastName),
          ContactsSortOrder.Firstname => queryParams.Sort == "asc"
              ? query.OrderBy(contact => contact.FirstName)
              : query.OrderByDescending(contact => contact.FirstName),
          ContactsSortOrder.Email => queryParams.Sort == "asc"
              ? query.OrderBy(contact => contact.Email)
              : query.OrderByDescending(contact => contact.Email),
          ContactsSortOrder.Phone => queryParams.Sort == "asc"
              ? query.OrderBy(contact => contact.PhoneNumber)
              : query.OrderByDescending(contact => contact.PhoneNumber),
          // ContactsSortOrder.Category => queryParams.Sort == "asc" ? query.OrderBy(contact => contact.Category) : query.OrderByDescending(contact => contact.Category),
          // ContactsSortOrder.Subcategory => queryParams.Sort == "asc" ? query.OrderBy(contact => contact.Subcategory) : query.OrderByDescending(contact => contact.Subcategory)
          _ => query
        };

        var contacts = selectQuery
        .ToPagedListAsync(queryParams.PageNumber, queryParams.PageSize);

        return await contacts;
    }

    public async Task<Contact?> GetByEmailAsync(string email)
    {
        return await DbSet.FirstOrDefaultAsync(contact => contact.Email.ToLower() == email.ToLower());
    }
}
