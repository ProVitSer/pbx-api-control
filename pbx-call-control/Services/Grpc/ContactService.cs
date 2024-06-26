using Grpc.Core;
using PbxApiControl.Interface;
using PbxApiControl.Models.ContactReply;
using PbxApiControl.Models.Contact;

namespace PbxApiControl.Services.Grpc {
    public class ContactService: ContactPbxService.ContactPbxServiceBase {
        private readonly ILogger < ContactService > _logger;
        private readonly IContactService _contactService;

        public ContactService(ILogger < ContactService > logger, IContactService contactService) {
            _logger = logger;
            _contactService = contactService;
        }

        public override Task < ContactInfoDataReply > GetContactInfoById(GetContactInfoByIdRequest request, ServerCallContext context) {
            try {

                var isContactExists = _contactService.IsContactIdExists(request.ContactId);

                if (!isContactExists) {
                    throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ContactIdNotFound));

                }

                var contact = _contactService.GetContactInfoById(request.ContactId);

                return Task.FromResult(ContactInfoReply.FormatContact(contact));
            } catch (Exception e) {
                _logger.LogError("GetContactInfoById: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }

        public override Task < ContactInfoDataReply > UpdateContactInfoById(UpdateContactInfoRequest request, ServerCallContext context) {
            try {

                var isContactExists = _contactService.IsContactIdExists(request.ContactId);

                if (!isContactExists) {
                    throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ContactIdNotFound));

                }

                var contact = _contactService.UpdateContactById(new UpdateContactDataModel(request));

                return Task.FromResult(ContactInfoReply.FormatContact(contact));
            } catch (Exception e) {
                _logger.LogError("UpdateContactInfoById: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }

        public override Task < ContactListReply > GetContactList(GetContactListRequest request, ServerCallContext context) {
            try {
                var contactsList = _contactService.ContactList(request.PageNumber, request.PageSize);

                return Task.FromResult(new ContactListReply {
                    Contacts = {
                            contactsList.Contacts.Select(x => ContactInfoReply.FormatContact(x)).ToArray(),

                        },
                        PageSize = contactsList.PageSize,
                        PageNumber = contactsList.PageNumber,
                        TotalCount = contactsList.TotalCount
                });
            } catch (Exception e) {
                _logger.LogError("GetContactList: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }

        public override Task < ContactInfoDataReply > DeleteContactById(DeleteContactByIdRequest request, ServerCallContext context) {
            try {
                var isContactExists = _contactService.IsContactIdExists(request.ContactId);

                if (!isContactExists) {
                    throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ContactIdNotFound));

                }

                var contact = _contactService.DeleteContactById(request.ContactId);

                return Task.FromResult(ContactInfoReply.FormatContact(contact));
            } catch (Exception e) {
                _logger.LogError("DeleteContactById: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }
    }
}