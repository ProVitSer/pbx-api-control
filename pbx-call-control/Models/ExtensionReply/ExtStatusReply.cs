using PbxApiControl.Models.Extensions;

namespace PbxApiControl.Models.ExtensionReply
{
    public static class ExtStatusReply
    {
        public static ExtensionStatusReply GetExtensionStatus(ExtensionStatus extensionStatusData)
        {
            var reply = new ExtensionStatusReply
            {
            
                FirstName = extensionStatusData.FirstName,
                LastName = extensionStatusData.LastName,
                Email = extensionStatusData.Email,
                Extension = extensionStatusData.Extension,
                Registered = extensionStatusData.Registered,
                ForwardingRulesStatus = (ExtensionForwardStatus)extensionStatusData.ForwardingRulesStatus,
                QueuesStatus = (ExtensionQueueStatus)extensionStatusData.QueuesStatus,
                Groups = { extensionStatusData.Groups },
                InRingGroups = { extensionStatusData.InRingGroups },
                LoggedInQueues = { extensionStatusData.LoggedInQueues  },
            
            };

            reply.Devices.AddRange(extensionStatusData.Devices!.Select(devInfo => new Device
            {
                UserAgent = devInfo.UserAgent,
                Contact = devInfo.Contact
            }));

            return reply;
        }
    }
}



