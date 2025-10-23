using AutoMapper;
using DefaultNamespace;
using RecruitmentApp.Features.Contacts.Domain;

namespace RecruitmentApp.Configuration;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Contact, ContactDetailsDto>();
        CreateMap<ContactDetailsDto, Contact>();
    }
}
