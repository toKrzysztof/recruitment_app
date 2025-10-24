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
        var validationErrors = await ValidateAddContact(contactDetailsDto);

        if (validationErrors.Any())
        {
            return ServiceResult<ContactDetailsDto>.Failure(validationErrors);
        }

        var contact = _mapper.Map<Contact>(contactDetailsDto);

        var category = await _unitOfWork.Categories.GetByName(contactDetailsDto.Category.Name);

        if (category == null)
            throw new Exception("Category was expected to be found in the database but was not.");

        contact.Category = category;

        if (contactDetailsDto.Subcategory != null)
        {
            var existingSubcategory = await _unitOfWork.Subcategories.GetByName(contactDetailsDto.Subcategory.Name);

            if (existingSubcategory != null)
            {
                existingSubcategory.Category = category;
                contact.Subcategory = existingSubcategory;
            }
            else
            {
                var subcategoryToCreate = _mapper.Map<Subcategory>(contactDetailsDto.Subcategory);
                subcategoryToCreate.Category = category;

                var createdSubcategory = _unitOfWork
                    .Subcategories
                    .Add(subcategoryToCreate);

                contact.Subcategory = createdSubcategory;
            }
        }

        _unitOfWork.Contacts.Add(contact);

        if (!await _unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<ContactDetailsDto>.Success(_mapper.Map<ContactDetailsDto>(contact));
    }

    public async Task<ServiceResult<ContactDetailsDto>> UpdateContact(ContactDetailsDto contactDetailsDto)
    {
        var validationErrors = await ValidateUpdateContact(contactDetailsDto);

        if (validationErrors.Any())
        {
            return ServiceResult<ContactDetailsDto>.Failure(validationErrors);
        }

        var contact = await _unitOfWork.Contacts.GetByIdAsync(contactDetailsDto.Id);
        if (contact == null)
            throw new Exception("Contact with the given id was expected to be found in the database but was not.");

        _mapper.Map(contactDetailsDto, contact);
        _unitOfWork.Contacts.Update(contact);

        if (!await _unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<ContactDetailsDto>.Success(_mapper.Map<ContactDetailsDto>(contact));
    }

    private async Task<List<string>> ValidateUpdateContact(ContactDetailsDto contactDetailsDto)
    {
        var validationErrors = new List<string>();

        if (contactDetailsDto.Id == 0)
        {
            validationErrors.Add("Contact id is required, but missing.");
            return validationErrors;
        }

        var contactToUpdate = await _unitOfWork.Contacts.GetByIdAsync(contactDetailsDto.Id);
        if (contactToUpdate == null)
        {
            validationErrors.Add(nameof(GenericErrorMessage.NotFound));
            return validationErrors;
        }

        var existingEmail = await _unitOfWork.Contacts.GetByEmailAsync(contactDetailsDto.Email);

        // check if contact data change request email is already in use but by a different user (email must be unique)
        // if the requested email is used but it's not used by the user requesting the contact data change, communicate that the email is already taken
        if (existingEmail != null && existingEmail.Email != contactToUpdate.Email)
        {
            validationErrors.Add("This email is already taken by another user.");
        }

        if (contactDetailsDto.Subcategory != null && contactDetailsDto.Subcategory.Category.Name != CategoryConstants.Other)
        {
            validationErrors.Add("Creating subcategories is only permitted of category 'Other'.");
        }

        var password = contactDetailsDto.Password;
        var passwordValidationErrors = ValidatePassword(password);
        passwordValidationErrors.ForEach(validationErrors.Add);

        return validationErrors;
    }


    private async Task<List<string>> ValidateAddContact(ContactDetailsDto contactDetailsDto)
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
        var passwordValidationErrors = ValidatePassword(password);
        passwordValidationErrors.ForEach(validationErrors.Add);

        if (!CategoryConstants.Categories.Contains((contactDetailsDto.Category.Name)))
        {
            validationErrors.Add($"The category {contactDetailsDto.Category.Name} does not exist.");
        }

        if (contactDetailsDto.Subcategory != null && contactDetailsDto.Subcategory.Category.Name != CategoryConstants.Other)
        {
            validationErrors.Add("Creating subcategories is only permitted of category 'Other'.");
        }

        return validationErrors;
    }

    private List<string> ValidatePassword(string password)
    {
        var validationErrors = new List<string>();

        if (password.Length < 8)
        {
            validationErrors.Add("The password must be at least 8 characters long.");
        }

        if (!password.Any(char.IsDigit))
        {
            validationErrors.Add("The password must contain at least one numeric character.");
        }

        if (password.All(char.IsLetterOrDigit))
        {
            validationErrors.Add("The password must contain at least one non-alphanumeric character.");
        }

        return validationErrors;
    }
}
