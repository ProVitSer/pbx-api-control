using PbxApiControl.Models;
using PbxApiControl.DTOs.Contact;
using System.Collections.Generic;


namespace PbxApiControl.Interface;

#nullable enable

public interface IContactService
{

    ContactInfo? GetContactInfoById(string contactId);

    ContactInfo? CreateContact(CreateContactDto dto);

    ContactInfo? DeleteContactById(string contactId);

    ContactInfo? UpdateContactById(string contactId, UpdateContactDto dto);

}
