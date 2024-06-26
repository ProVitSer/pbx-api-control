using TCX.Configuration;


namespace PbxApiControl.Models.Contact
{
    public class ContactDataModel
    {
        public int ContactId { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string CompanyName { get; }

        public string CrmContactData { get; }

        public string Tag { get; }

        public string Mobile { get; }

        public string MobileTwo { get; }

        public string Home { get; }

        public string HomeTwo { get; }

        public string Business { get; }

        public string BusinessTwo { get; }

        public string EmailAddress { get; }

        public string Other { get; }

        public string BusinessFax { get; }

        public string HomeFax { get; }

        public string Pager { get; }

        public ContactDataModel(int contactId, PhoneBookEntry c)
        {
            this.ContactId = contactId;
            this.FirstName = c.FirstName;
            this.LastName = c.LastName;
            this.CompanyName = c.CompanyName;
            this.CrmContactData = c.CrmContactData;
            this.Tag = c.Tag;
            this.Mobile = c.PhoneNumber;
            this.MobileTwo = c.AddressNumberOrData0;
            this.Home = c.AddressNumberOrData1;
            this.HomeTwo = c.AddressNumberOrData2;
            this.Business = c.AddressNumberOrData3;
            this.BusinessTwo = c.AddressNumberOrData4;
            this.EmailAddress = c.AddressNumberOrData5;
            this.Other = c.AddressNumberOrData6;
            this.BusinessFax = c.AddressNumberOrData7;
            this.HomeFax = c.AddressNumberOrData8;
            this.Pager = c.AddressNumberOrData9;
        }
    }
}