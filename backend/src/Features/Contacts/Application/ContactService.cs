using AutoMapper;
using RecruitmentApp.Features.Contacts.Application.Contracts;
using RecruitmentApp.Features.Contacts.Domain;
using RecruitmentApp.Features.Contacts.Dto;
using RecruitmentApp.Shared.Application;
using RecruitmentApp.Shared.Constants;
using RecruitmentApp.Shared.Data.Contracts;

namespace RecruitmentApp.Features.Contacts.Application;

public class ContactService : IContactService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ContactService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceResult<ContactDetailsDto>> AddContact(ContactDetailsDto contactDetailsDto)
    {
        var validationErrors = new List<string>();
        var existingContact = await _unitOfWork.Contacts.GetByEmailAsync(contactDetailsDto.Email);

        if (existingContact != null)
        {
            validationErrors.Add("This email is already taken.");
        }

        if (contactDetailsDto.DateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            validationErrors.Add("Date of birth cannot be from the future.");
        }

        var password = contactDetailsDto.Password;

        if (password.Length < 8)
        {
            validationErrors.Add($"The password must be at least 8 characters long.");
        }

        if (!password.Any(char.IsDigit))
        {
            validationErrors.Add("The password must contain at least one numeric character.");
        }

        if (password.All(char.IsLetterOrDigit))
        {
            validationErrors.Add("The password must contain at least one non-alphanumeric character.");
        }

        if (validationErrors.Any())
        {
            return ServiceResult<ContactDetailsDto>.Failure(validationErrors);
        }

        var contact = _mapper.Map<Contact>(contactDetailsDto);

        _unitOfWork.Contacts.Add(contact);

        if (!await _unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<ContactDetailsDto>.Success(_mapper.Map<ContactDetailsDto>(contact));
    }

    public async Task<ServiceResult<ContactDetailsDto>> UpdateContact(ContactDetailsDto contactDetailsDto)
    {
        var contact = _mapper.Map<Contact>(contactDetailsDto);
        var contactExtists = await _unitOfWork.Contacts.ExistsAsync(contact.Id);

        if (!contactExtists)
        {
             return ServiceResult<ContactDetailsDto>.Failure([nameof(GenericErrorMessage.NotFound)]);
        }

        _unitOfWork.Contacts.Update(contact);

        if (!await _unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<ContactDetailsDto>.Success(_mapper.Map<ContactDetailsDto>(contact));
    }
}
