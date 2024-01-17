using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace westcoast_education.web.ViewModels;

public class PersonPostViewModel
{
    [Required(ErrorMessage = "Kategori är obligatiskt")]
    [DisplayName("Kategori")]
    public string PersonCategory { get; set; } = "";
    
    [Required(ErrorMessage = "Personnummer är obligatoriskt")]
    [DisplayName("Personnummer")]
    [Range(190000000000, long.MaxValue, ErrorMessage = "Personnummer är obligatoriskt [felaktigt personnummer]")]
    public long? SocialSecurityNumber { get; set; }
    
    [Required(ErrorMessage = "Namn är obligatiskt")]
    [DisplayName("Namn")]
    public string Name { get; set; } = "";
    
    [Required(ErrorMessage = "E-post är obligatiskt")]
    [DisplayName("E-post")]
    public string Email { get; set; } = "";
    
    [Required(ErrorMessage = "Adress är obligatiskt")]
    [DisplayName("Adress")]
    public string Address { get; set; } = "";
    
    [Required(ErrorMessage = "Telefon är obligatiskt")]
    [DisplayName("Telefon")]
    public string PhoneNumber { get; set; } = "";
}