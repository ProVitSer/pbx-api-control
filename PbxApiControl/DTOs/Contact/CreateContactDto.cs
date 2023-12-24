using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.DTOs.Contact;

public class CreateContactDto
{
    [Required]
    public string FirstName { get; init; }

    [Required]
    public string LastName { get; init; }


    [Required]
    public string Mobile { get; init; }

    public string CompanyName { get; init; }
    public string CrmContactData { get; init; }
    public string Tag { get; init; }
    public string MobileTwo { get; init; }
    public string Home { get; init; }
    public string HomeTwo { get; init; }
    public string Business { get; init; }
    public string BusinessTwo { get; init; }
    public string EmailAddress { get; init; }
    public string Other { get; init; }
    public string BusinessFax { get; init; }
    public string HomeFax { get; init; }
    public string Pager { get; init; }

}