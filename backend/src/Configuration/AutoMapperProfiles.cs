using AutoMapper;
using RecruitmentApp.Features.Contacts.Domain;
using RecruitmentApp.Features.Contacts.Dto;

namespace RecruitmentApp.Configuration;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Contact, ContactDetailsDto>();
        CreateMap<ContactDetailsDto, Contact>();
    }
}
