using PbxApiControl.Models.Extensions;

namespace PbxApiControl.Models.Reply
{
    public static class ExtStatusReply
    {
        public static ExtensionStatusReply GetExtensionStatus(ExtensionStatus extensionStatusData)
        {
            return new ExtensionStatusReply
            {
                Extension = extensionStatusData.Extension,
                Registered = extensionStatusData.Registered,
                ForwardingRulesStatus = extensionStatusData.ForwardingRulesStatus,
                QueuesStatus = extensionStatusData.QueuesStatus,
                Groups = { extensionStatusData.Groups },
                RingGroups = { extensionStatusData.RingGroups },
                AllQueues =  { extensionStatusData.AllQueues },
                LoggedInQueues = { extensionStatusData.LoggedInQueues  },
                LoggedOutQueues = { extensionStatusData.LoggedOutQueues  }
            };
        }
    }
}



