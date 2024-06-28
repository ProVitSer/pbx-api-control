namespace PbxApiControl.Models.Contact
{
    public class ContaclListDataModel
    {
        public ContactDataModel[] Contacts { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public int TotalCount { get; }
    
  
        public ContaclListDataModel(int pageNumber, int pageSize, int  totalCount, ContactDataModel[] contacts)
        {
            this.Contacts = contacts;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
        }
    }
}

