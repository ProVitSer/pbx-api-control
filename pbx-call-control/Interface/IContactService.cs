using PbxApiControl.Models.Contact;

namespace PbxApiControl.Interface;

public interface IContactService
{
    bool IsContactIdExists(string contactId);

    ContactDataModel GetContactInfoById(string contactId);
    
    ContactDataModel UpdateContactById(UpdateContactDataModel data);
    
    ContaclListDataModel ContactList(int pageNumber, int pageSize);

    ContactDataModel DeleteContactById(string contactId);

}