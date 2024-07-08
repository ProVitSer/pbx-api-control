using PbxApiControl.Interface;
using PbxApiControl.Models.Contact;
using TCX.Configuration;
using PbxApiControl.Constants;

namespace PbxApiControl.Services.Pbx
{
    public class ContactService : IContactService
    {
        private readonly ILogger<ContactService> _logger;

        public ContactService(ILogger<ContactService> logger)
        {
            _logger = logger;
        }

        public ContactDataModel GetContactInfoById(string contactId)
        {
            if (!IsContactIdExists(contactId))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
            }

            var id = TryParse(contactId);

            using (PhoneBookEntry phoneBookEntry = PhoneSystem.Root.GetByID<PhoneBookEntry>(id))
            {
                return new ContactDataModel(id, phoneBookEntry);
            }
        }

        public ContactDataModel UpdateContactById(UpdateContactDataModel data)
        {
            if (!IsContactIdExists(data.ContactId))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
            }

            var id = TryParse(data.ContactId);

            using (PhoneBookEntry phoneBookEntry = PhoneSystem.Root.GetByID<PhoneBookEntry>(id))
            {
                if (data.FirstName != null) phoneBookEntry.FirstName = data.FirstName;
                if (data.LastName != null) phoneBookEntry.LastName = data.LastName;
                if (data.Mobile != null) phoneBookEntry.PhoneNumber = data.Mobile;
                if (data.CompanyName != null) phoneBookEntry.CompanyName = data.CompanyName;
                if (data.CrmContactData != null) phoneBookEntry.CrmContactData = data.CrmContactData;
                if (data.Tag != null) phoneBookEntry.Tag = data.Tag;
                if (data.MobileTwo != null) phoneBookEntry.AddressNumberOrData0 = data.MobileTwo;
                if (data.Home != null) phoneBookEntry.AddressNumberOrData1 = data.Home;
                if (data.HomeTwo != null) phoneBookEntry.AddressNumberOrData2 = data.HomeTwo;
                if (data.Business != null) phoneBookEntry.AddressNumberOrData3 = data.Business;
                if (data.BusinessTwo != null) phoneBookEntry.AddressNumberOrData4 = data.BusinessTwo;
                if (data.EmailAddress != null) phoneBookEntry.AddressNumberOrData5 = data.EmailAddress;
                if (data.Other != null) phoneBookEntry.AddressNumberOrData6 = data.Other;
                if (data.BusinessFax != null) phoneBookEntry.AddressNumberOrData7 = data.BusinessFax;
                if (data.HomeFax != null) phoneBookEntry.AddressNumberOrData8 = data.HomeFax;
                if (data.Pager != null) phoneBookEntry.AddressNumberOrData9 = data.Pager;

                phoneBookEntry.Save();
                return new ContactDataModel(id, phoneBookEntry);
            }
        }

        public ContaclListDataModel ContactList(int pageNumber, int pageSize)
        {
            using (IArrayDisposer<PhoneBookEntry> phoneBookEntries = PhoneSystem.Root.GetAll<PhoneBookEntry>().GetDisposer())
            {
                var contacts = phoneBookEntries
                    .Select(x => new ContactDataModel(x.ID, x))
                    .ToArray();

                var totalCount = contacts.Length;
                var items = contacts
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToArray();

                return new ContaclListDataModel(pageNumber, pageSize, totalCount, items);
            }
        }

        public ContactDataModel DeleteContactById(string contactId)
        {
            if (!IsContactIdExists(contactId))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
            }

            var id = TryParse(contactId);

            using (PhoneBookEntry phoneBookEntry = PhoneSystem.Root.GetByID<PhoneBookEntry>(id))
            {
                var contact = new ContactDataModel(id, phoneBookEntry);
                phoneBookEntry.Delete();
                phoneBookEntry.Save();
                return contact;
            }
        }

        public bool IsContactIdExists(string contactId)
        {
            var id = TryParse(contactId);
            using (PhoneBookEntry phoneBookEntry = PhoneSystem.Root.GetByID<PhoneBookEntry>(id))
            {
                return !(phoneBookEntry == null);
            }
        }

        private int TryParse(string strNum)
        {
            if (int.TryParse(strNum, out int result))
            {
                return result;
            }
            else
            {
                throw new FormatException(ServiceConstants.TryParseError);
            }
        }
    }
}