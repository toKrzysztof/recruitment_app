using AutoMapper;
using RecruitmentApp.Features.Contacts.Domain;
using RecruitmentApp.Features.Contacts.Dto;

namespace RecruitmentApp.Configuration;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Contact, ContactDetailsDto>();
        CreateMap<ContactDetailsDto, Contact>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Subcategory, opt => opt.Ignore());
        CreateMap<Contact, ContactDto>();
        CreateMap<ContactDto, Contact>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();
        CreateMap<Subcategory, SubcategoryDto>();
        CreateMap<SubcategoryDto, Subcategory>();
    }
}
