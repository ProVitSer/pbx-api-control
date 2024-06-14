using PbxApiControl.Models.Extensions;

namespace PbxApiControl.Models.Reply
{
    public static class ExtensionStatusReply
    {
        public static GetExtensionStatusReply FormatExtensionStatus(ExtensionStatus extensionInfo)
        {
            return new GetExtensionStatusReply
            {
                Extension = extensionInfo.Extension,
                Registered = extensionInfo.Registered,
                ForwardingRulesStatus = extensionInfo.ForwardingRulesStatus,
                QueuesStatus = extensionInfo.QueuesStatus,
                Groups = { extensionInfo.Groups },
                Queues = { extensionInfo.Queues },
                RingGroups = { extensionInfo.RingGroups }
            };
        }
    }
}



