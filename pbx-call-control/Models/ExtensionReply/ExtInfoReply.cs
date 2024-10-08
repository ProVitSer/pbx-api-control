﻿using PbxApiControl.Models.Extensions;

namespace PbxApiControl.Models.ExtensionReply
{
    public class ExtInfoReply
    {
        public static ExtensionInfoReply GetExtensionInfoReply(ExtensionInfo extensionInfo)
        {
            
            return new ExtensionInfoReply
            {
                Extension = extensionInfo.Extension,
                FirstName = extensionInfo.FirstName,
                LastName = extensionInfo.LastName,
                Email = extensionInfo.Email,
                AuthId = extensionInfo.AuthID,
                AuthPassword = extensionInfo.AuthPassword,
                MobileNumber = extensionInfo.MobileNumber ?? "",
                SipId = extensionInfo.SipID ?? "",
                OutboundCallerId = extensionInfo.OutboundCallerID,
                RecordingType = (RecordingType)extensionInfo.RecordingType,
                IsExtensionEnabled = extensionInfo.IsExtensionEnabled,
                DisableExternalCalls = extensionInfo.DisableExternalCalls,
                DeliverAudio = extensionInfo.DeliverAudio,
                SupportReinvite = extensionInfo.SupportReinvite,
                SupportReplaces = extensionInfo.SupportReplaces,
                EmailOptions = (EmailOptionsType)extensionInfo.EmailOptions,
                VoiceMailEnable = extensionInfo.VoiceMailEnable,
                VoiceMailPin = extensionInfo.VoiceMailPin,
                VoiceMailPlayCallerId = extensionInfo.VoiceMailPlayCallerID,
                Internal = extensionInfo.Internal,
                NoAnswerTimeout = extensionInfo.NoAnswerTimeout
            };
        }
    }
}

