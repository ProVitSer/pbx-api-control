namespace PbxApiControl.Models.Contact
{
    public class UpdateContactDataModel
    {
        public string ContactId { get; }

        public string? FirstName { get; }

        public string? LastName { get; }

        public string? CompanyName { get; }

        public string? CrmContactData { get; }

        public string? Tag { get; }

        public string? Mobile { get; }

        public string? MobileTwo { get; }

        public string? Home { get; }

        public string? HomeTwo { get; }

        public string? Business { get; }

        public string? BusinessTwo { get; }

        public string? EmailAddress { get; }

        public string? Other { get; }

        public string? BusinessFax { get; }

        public string? HomeFax { get; }

        public string? Pager { get; }

        public UpdateContactDataModel(UpdateContactInfoRequest request)
        {
            this.ContactId = request.ContactId;
            this.FirstName = request.FirstName;
            this.LastName = request.LastName;
            this.CompanyName = request.CompanyName;
            this.CrmContactData = request.CrmContactData;
            this.Tag = request.Tag;
            this.Mobile = request.Mobile;
            this.MobileTwo = request.MobileTwo;
            this.Home = request.Home;
            this.HomeTwo = request.HomeTwo;
            this.Business = request.Business;
            this.BusinessTwo = request.BusinessTwo;
            this.EmailAddress = request.EmailAddress;
            this.Other = request.Other;
            this.BusinessFax = request.BusinessFax;
            this.HomeFax = request.HomeFax;
            this.Pager = request.Pager;
        }
    }
}

