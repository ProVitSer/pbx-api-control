using PbxApiControl.Models.Extensions;

namespace PbxApiControl.Models.Reply
{
    public class CreateExtensionInfoReply
    {
        public static CreateExtensionReply FormatCreateExtensionInfo(ExtensionInfo createExtension)
        {
            return new CreateExtensionReply
            {
    
                AuthId = createExtension.AuthID,
                AuthPassword = createExtension.AuthPassword,
                SipId = createExtension.SipID,
                Extension = createExtension.Extension,
                FirstName = createExtension.FirstName,
                LastName = createExtension.LastName,
                Email = createExtension.Email,
                MobileNumber = createExtension.MobileNumber,
                OutboundCallerId = createExtension.OutboundCallerID,
                RecordingType = createExtension.RecordingType,
                IsExtenionEnabled = createExtension.IsExtenionEnabled,
                DisableExternalCalls = createExtension.DisableExternalCalls,
                DeliverAudio = createExtension.DeliverAudio,
                SupportReinvite = createExtension.SupportReinvite,
                SupportReplaces = createExtension.SupportReplaces,
                EmailOptions = createExtension.EmailOptions,
                VoiceMailEnable = createExtension.VoiceMailEnable,
                VoiceMailPin = createExtension.VoiceMailPin,
                VoiceMailPlayCallerId = createExtension.VoiceMailPlayCallerID,
                Internal = createExtension.Internal,
                NoAnswerTimeout = createExtension.NoAnswerTimeout
            };

        }
    }
}

