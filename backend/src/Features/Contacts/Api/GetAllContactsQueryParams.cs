namespace RecruitmentApp.Features.Contacts.Api;

public class GetAllContactsQueryParams
{
    public ContactsSortOrder SortBy { get; init; } = ContactsSortOrder.Lastname;
    public string Sort { get; init; } = "asc";
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 5;

    public string? FirstName { get; init; }

    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Category { get; init; }
    public string? Subcategory { get; init; }
}

public enum ContactsSortOrder
{
    Firstname, Lastname, Email, Phone, Category, Subcategory
}
