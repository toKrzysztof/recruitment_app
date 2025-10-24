using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.Features.Contacts.Application.Contracts;
using RecruitmentApp.Features.Contacts.Domain;
using RecruitmentApp.Features.Contacts.Dto;
using RecruitmentApp.Shared.Api;
using RecruitmentApp.Shared.Api.Contracts;
using RecruitmentApp.Shared.Constants;
using RecruitmentApp.Shared.Data.Contracts;

namespace RecruitmentApp.Features.Contacts.Api.Controllers;

public class ContactsController : ApiControllerBase
{
    private readonly IContactService _contactService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ContactsController(IContactService contactService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _contactService = contactService;
        _mapper = mapper;
    }

    // GET: api/contacts
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Contact>))]
    public async Task<IActionResult> GetAllContacts([FromQuery] GetAllContactsQueryParams queryParams, IHttpService httpService)
    {
        var pagedList = await _unitOfWork.Contacts.GetAllAsync(queryParams);
        httpService.AddPaginationHeader(Response, PaginationHeader.FromPagedList(pagedList));

        var contactDtos = _mapper.Map<ContactDto[]>(pagedList.Items);

        return Ok((contactDtos));
    }

    // GET: api/contacts/5
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetContact(int id)
    {
        var contact = await _unitOfWork.Contacts.GetByIdAsync(id);

        if (contact == null)
        {
            return NotFound();
        }

        var contactDetailsDto = _mapper.Map<ContactDetailsDto>(contact);

        return Ok(contactDetailsDto);
    }

    // POST: api/contacts
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PostContact(ContactDetailsDto contactDetailsDto)
    {
        var serviceResponse = await _contactService.AddContact(contactDetailsDto);

        if (!serviceResponse.IsSuccess) return BadRequest(serviceResponse.Errors);

        return Ok(serviceResponse.Data);
    }

    // PUT: api/contacts/5
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutContact(int id, ContactDetailsDto contactDetailsDto)
    {
        contactDetailsDto.Id = id;
        var serviceResponse = await _contactService.UpdateContact(contactDetailsDto);

        if (serviceResponse.Errors.Any(e => e == nameof(GenericErrorMessage.NotFound)))
        {
            return NotFound();
        }

        if (!serviceResponse.IsSuccess)
        {
            return BadRequest(serviceResponse.Errors);
        }

        return CreatedAtAction(nameof(GetContact), new { id = serviceResponse.Data.Id }, contactDetailsDto);
    }

    // DELETE: api/contacts/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var contact = await _unitOfWork.Contacts.GetByIdAsync(id);
        if (contact == null)
        {
            return NotFound();
        }

        _unitOfWork.Contacts.Delete(contact);

        if (!await _unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return NoContent();
    }
}
