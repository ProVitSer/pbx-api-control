namespace PbxApiControl.Models.Contact;

public class ContaclListDataModel
{
    public ContactDataModel[] Contacts { get; }

    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalCount { get; }
    
  
    public ContaclListDataModel(int PageNumber, int PageSize, int  TotalCount, ContactDataModel[] Contacts)
    {
        this.Contacts = Contacts;
        this.PageNumber = PageNumber;
        this.PageSize = PageSize;
        this.TotalCount = TotalCount;
    }
}