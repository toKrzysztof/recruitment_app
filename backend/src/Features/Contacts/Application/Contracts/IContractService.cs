using RecruitmentApp.Features.Contacts.Dto;
using RecruitmentApp.Shared.Application;

namespace RecruitmentApp.Features.Contacts.Application.Contracts;

public interface IContactService
{
    public Task<ServiceResult<ContactDetailsDto>> AddContact(ContactDetailsDto contactDetailsDto);
    public Task<ServiceResult<ContactDetailsDto>> UpdateContact(ContactDetailsDto contactDetailsDto);
}
