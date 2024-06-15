using PbxApiControl.Models.Extensions;

namespace PbxApiControl.Models.Reply
{
    public class ExtensionInfoReply
    {
    
        public static GetExtensionInfoReply FormatExtensionInfo(ExtensionInfo extensionInfo)
        {
            return new GetExtensionInfoReply
            {
                AuthId = extensionInfo.AuthID,
                AuthPassword = extensionInfo.AuthPassword,
                SipId = extensionInfo.SipID,
                Extension = extensionInfo.Extension,
                FirstName = extensionInfo.FirstName,
                LastName = extensionInfo.LastName,
                Email = extensionInfo.Email,
                MobileNumber = extensionInfo.MobileNumber,
                OutboundCallerId = extensionInfo.OutboundCallerID,
                RecordingType = extensionInfo.RecordingType,
                IsExtenionEnabled = extensionInfo.IsExtenionEnabled,
                DisableExternalCalls = extensionInfo.DisableExternalCalls,
                DeliverAudio = extensionInfo.DeliverAudio,
                SupportReinvite = extensionInfo.SupportReinvite,
                SupportReplaces = extensionInfo.SupportReplaces,
                EmailOptions = extensionInfo.EmailOptions,
                VoiceMailEnable = extensionInfo.VoiceMailEnable,
                VoiceMailPin = extensionInfo.VoiceMailPin,
                VoiceMailPlayCallerId = extensionInfo.VoiceMailPlayCallerID,
                Internal = extensionInfo.Internal,
                NoAnswerTimeout = extensionInfo.NoAnswerTimeout
            };
        }
    }
}

