using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentApp.Features.Contacts.Data;
using RecruitmentApp.Features.Contacts.Data.Contracts;
using RecruitmentApp.Features.Contacts.Domain;
using RecruitmentApp.Shared.Api;
using RecruitmentApp.Shared.Api.Contracts;

namespace RecruitmentApp.Features.Contacts.Api.Controllers;

public class ContactsController : ApiControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IContactRepository _contactRepository;

    public ContactsController(ApplicationDbContext context, IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
        _context = context;
    }

    // GET: api/contacts
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Contact>))]
    public async Task<ActionResult> GetAllContacts([FromQuery] GetAllContactsQueryParams queryParams, IHttpService httpService)
    {
        var pagedList = await _contactRepository.GetAllAsync(queryParams);
        httpService.AddPaginationHeader(Response, PaginationHeader.FromPagedList(pagedList));
        return Ok(pagedList.Items);
    }

    // GET: api/contacts/5
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);

        if (contact == null)
        {
            return NotFound();
        }

        return contact;
    }

    // POST: api/contacts
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Contact>> PostContact(Contact contact)
    {
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
    }

    // PUT: api/contacts/5
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutContact(int id, Contact contact)
    {
        if (id != contact.Id)
        {
            return BadRequest();
        }

        _context.Entry(contact).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ContactExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/contacts/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
        {
            return NotFound();
        }

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ContactExists(int id)
    {
        return _context.Contacts.Any(e => e.Id == id);
    }
}
