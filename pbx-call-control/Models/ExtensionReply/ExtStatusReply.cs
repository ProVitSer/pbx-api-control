﻿using PbxApiControl.Models.Extensions;

namespace PbxApiControl.Models.ExtensionReply
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
                InRingGroups = { extensionStatusData.InRingGroups },
                LoggedInQueues = { extensionStatusData.LoggedInQueues  },
            };
        }
    }
}



