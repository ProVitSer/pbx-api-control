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

        public string? CompanyName { get; init; }
        public string? CrmContactData { get; init; }
        public string? Tag { get; init; }
        public string? MobileTwo { get; init; }
        public string? Home { get; init; }
        public string? HomeTwo { get; init; }
        public string? Business { get; init; }
        public string? BusinessTwo { get; init; }
        public string? EmailAddress { get; init; }
        public string? Other { get; init; }
        public string? BusinessFax { get; init; }
        public string? HomeFax { get; init; }
        public string? Pager { get; init; }
        
        public CreateContactDto(
            string firstName, 
            string mobile,
            string lastName,
            string? companyName = null,
            string? crmContactData = null,
            string? tag = null,
            string? mobileTwo = null,
            string? home = null,
            string? homeTwo = null,
            string? business = null,
            string? businessTwo = null,
            string? emailAddress = null,
            string? other = null,
            string? businessFax = null,
            string? homeFax = null,
            string? pager = null)
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