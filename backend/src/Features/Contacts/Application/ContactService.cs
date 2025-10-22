using AutoMapper;
using DefaultNamespace;
using RecruitmentApp.Features.Contacts.Application.Contracts;
using RecruitmentApp.Features.Contacts.Domain;
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
        if (contactDetailsDto.DateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            return ServiceResult<ContactDetailsDto>.Failure(["Date of birth cannot be from the future."]);
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
