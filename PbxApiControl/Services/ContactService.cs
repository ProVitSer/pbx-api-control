using System.Collections.Generic;
using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.DTOs.Contact;
using PbxApiControl.Models;
using System;

namespace PbxApiControl.Services;

#nullable enable
public class ContactService : IContactService
{

    private readonly IPbxService _pbxService;

    public ContactService(IPbxService pbxService)
    {
        _pbxService = pbxService;
    }

    public ContactInfo? GetContactInfoById(string contactId)
    {
        int id;
        if (int.TryParse(contactId, out int result))
        {
            id = result;
        }
        else
        {
            id = -1;
        }

        using (PhoneBookEntry phoneBookEntry = PhoneSystem.Root.GetByID<PhoneBookEntry>(id))
        {
            if (phoneBookEntry == null)
            {
                return null;
            }
            return new ContactInfo(id, phoneBookEntry);
        };

    }

    public ContactInfo? CreateContact(CreateContactDto dto)
    {
        using (PhoneBookEntry phoneBookEntry = PhoneSystem.Root.GetTenant().CreatePhoneBookEntry())
        {
            phoneBookEntry.FirstName = dto.FirstName;
            phoneBookEntry.LastName = dto.LastName;
            phoneBookEntry.PhoneNumber = dto.Mobile;

            SetIfNotNull(() => phoneBookEntry.CompanyName = dto.CompanyName);
            SetIfNotNull(() => phoneBookEntry.CrmContactData = dto.CrmContactData);
            SetIfNotNull(() => phoneBookEntry.Tag = dto.Tag);
            SetIfNotNull(() => phoneBookEntry.AddressNumberOrData0 = dto.MobileTwo);
            SetIfNotNull(() => phoneBookEntry.AddressNumberOrData1 = dto.Home);
            SetIfNotNull(() => phoneBookEntry.AddressNumberOrData2 = dto.HomeTwo);
            SetIfNotNull(() => phoneBookEntry.AddressNumberOrData3 = dto.Business);
            SetIfNotNull(() => phoneBookEntry.AddressNumberOrData4 = dto.BusinessTwo);
            SetIfNotNull(() => phoneBookEntry.AddressNumberOrData5 = dto.EmailAddress);
            SetIfNotNull(() => phoneBookEntry.AddressNumberOrData6 = dto.Other);
            SetIfNotNull(() => phoneBookEntry.AddressNumberOrData7 = dto.BusinessFax);
            SetIfNotNull(() => phoneBookEntry.AddressNumberOrData8 = dto.HomeFax);
            SetIfNotNull(() => phoneBookEntry.AddressNumberOrData9 = dto.Pager);

            phoneBookEntry.Save();
            return GetContactInfoById(phoneBookEntry.ID.ToString());
        };
    }


    public ContactInfo? UpdateContactById(string contactId, UpdateContactDto dto)
    {
        int id;
        if (int.TryParse(contactId, out int result))
        {
            id = result;
        }
        else
        {
            id = -1;
        }

        using (PhoneBookEntry phoneBookEntry = PhoneSystem.Root.GetByID<PhoneBookEntry>(id))
        {
            if (phoneBookEntry == null)
            {
                return null;
            }
            if (dto.FirstName != null) phoneBookEntry.FirstName = dto.FirstName;
            if (dto.LastName != null) phoneBookEntry.LastName = dto.LastName;
            if (dto.Mobile != null) phoneBookEntry.PhoneNumber = dto.Mobile;
            if (dto.CompanyName != null) phoneBookEntry.CompanyName = dto.CompanyName;
            if (dto.CrmContactData != null) phoneBookEntry.CrmContactData = dto.CrmContactData;
            if (dto.Tag != null) phoneBookEntry.Tag = dto.Tag;
            if (dto.MobileTwo != null) phoneBookEntry.AddressNumberOrData0 = dto.MobileTwo;
            if (dto.Home != null) phoneBookEntry.AddressNumberOrData1 = dto.Home;
            if (dto.HomeTwo != null) phoneBookEntry.AddressNumberOrData2 = dto.HomeTwo;
            if (dto.Business != null) phoneBookEntry.AddressNumberOrData3 = dto.Business;
            if (dto.BusinessTwo != null) phoneBookEntry.AddressNumberOrData4 = dto.BusinessTwo;
            if (dto.EmailAddress != null) phoneBookEntry.AddressNumberOrData5 = dto.EmailAddress;
            if (dto.Other != null) phoneBookEntry.AddressNumberOrData6 = dto.Other;
            if (dto.BusinessFax != null) phoneBookEntry.AddressNumberOrData7 = dto.BusinessFax;
            if (dto.HomeFax != null) phoneBookEntry.AddressNumberOrData8 = dto.HomeFax;
            if (dto.Pager != null) phoneBookEntry.AddressNumberOrData9 = dto.Pager;



            phoneBookEntry.Save();
            return GetContactInfoById(phoneBookEntry.ID.ToString());
        };
    }

    public ContactInfo? DeleteContactById(string contactId)
    {
        int id;
        if (int.TryParse(contactId, out int result))
        {
            id = result;
        }
        else
        {
            id = -1;
        }

        using (PhoneBookEntry phoneBookEntry = PhoneSystem.Root.GetByID<PhoneBookEntry>(id))
        {
            if (phoneBookEntry == null)
            {
                return null;
            }

            phoneBookEntry.Delete();
            return new ContactInfo(id, phoneBookEntry);
        };

    }

    private static void SetIfNotNull(Action action)
    {
        action?.Invoke();
    }

}