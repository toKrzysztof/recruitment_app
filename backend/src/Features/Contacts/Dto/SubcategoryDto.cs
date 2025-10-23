namespace RecruitmentApp.Features.Contacts.Dto;

public class SubcategoryDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required CategoryDto CategoryDto { get; set; }
}
