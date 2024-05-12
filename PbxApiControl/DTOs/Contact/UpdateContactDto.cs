
namespace PbxApiControl.DTOs.Contact;

public class UpdateContactDto
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
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

    public UpdateContactDto(
        string firstName, string lastName, string mobile, string companyName,
        string crmContactData, string tag, string mobileTwo, string home,
        string homeTwo, string business, string businessTwo, string emailAddress,
        string other, string businessFax, string homeFax, string pager)
    {
        FirstName = firstName;
        LastName = lastName;
        Mobile = mobile;
        CompanyName = companyName;
        CrmContactData = crmContactData;
        Tag = tag;
        MobileTwo = mobileTwo;
        Home = home;
        HomeTwo = homeTwo;
        Business = business;
        BusinessTwo = businessTwo;
        EmailAddress = emailAddress;
        Other = other;
        BusinessFax = businessFax;
        HomeFax = homeFax;
        Pager = pager;
    }
}