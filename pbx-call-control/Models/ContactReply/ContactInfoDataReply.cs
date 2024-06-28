using PbxApiControl.Models.Contact;

namespace PbxApiControl.Models.ContactReply
{
    public class ContactInfoReply
    {
        public static ContactInfoDataReply FormatContact(ContactDataModel data)
        {
            return new ContactInfoDataReply
            {
                ContactId = data.ContactId.ToString(),
                FirstName = data.FirstName,
                LastName = data.LastName,
                CompanyName = data.CompanyName,
                CrmContactData = data.CrmContactData,
                Tag = data.Tag,
                Mobile = data.Mobile,
                MobileTwo = data.MobileTwo,
                Home = data.Home,
                HomeTwo = data.HomeTwo,
                Business = data.Business,
                BusinessTwo = data.BusinessTwo,
                EmailAddress = data.EmailAddress,
                Other = data.Other,
                BusinessFax = data.BusinessFax,
                HomeFax = data.HomeFax,
                Pager = data.Pager,
            };
        }
    }
}