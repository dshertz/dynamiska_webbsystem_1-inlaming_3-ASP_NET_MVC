namespace westcoast_education.web.ViewModels;

public class PersonListViewModel
{
    public int PersonId { get; set; }
    public string PersonCategory { get; set; } = "";
    public long SocialSecurityNumber { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Address { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
}