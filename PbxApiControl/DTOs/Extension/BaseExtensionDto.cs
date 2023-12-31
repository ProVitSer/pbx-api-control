using System.ComponentModel.DataAnnotations;
using PbxApiControl.Enums;

namespace PbxApiControl.DTOs.Extension
{
    public class BaseExtensionDto
    {
        [Required]
        public string ExtensionNumber { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string AuthID { get; init; }
        public string AuthPassword { get; init; }
        public string MobileNumber { get; init; }
        public string OutboundCallerID { get; init; }

        [EnumDataType(typeof(RecordType))]
        public string RecordingType { get; init; }
        public bool IsExtenionEnabled { get; init; }
        public bool AllowedExternalCalls { get; init; }
        public bool DeliverAudio { get; init; }
        public bool SupportReinvite { get; init; }
        public bool SupportReplaces { get; init; }
    }
}