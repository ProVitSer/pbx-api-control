using PbxApiControl.Enums;
using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.Models.Extensions
{
    public class UpdateExtensionDataModel
    {
        public string ExtensionNumber { get; }
        public string? FirstName { get;  }
        public string? LastName { get; }
        public string? Email { get; }
        public string? AuthID { get; }
        public string? AuthPassword { get; }
        public string? MobileNumber { get; }
        public string? OutboundCallerID { get; }
        [EnumDataType(typeof(RecordType))]
        public string? RecordingType { get;  }
        public bool? IsExtenionEnabled { get; }
        public bool? DisableExternalCalls { get; }
        public bool? DeliverAudio { get;  }
        public bool? SupportReinvite { get; }
        public bool? SupportReplaces { get; }
        
        public UpdateExtensionDataModel(UpdateExtensionInfoRequest request)
        {
            ExtensionNumber = request.Extension;
            FirstName = request.FirstName;
            LastName = request.LastName;
            Email = request.Email;
            AuthID = request.AuthId;
            AuthPassword = request.AuthPassword;
            MobileNumber = request.MobileNumber;
            OutboundCallerID = request.OutboundCallerId;
            RecordingType = request.RecordingType;
            IsExtenionEnabled = request.IsExtensionEnabled;
            DisableExternalCalls = request.DisableExternalCalls;
            DeliverAudio = request.DeliverAudio;
            SupportReinvite = request.SupportReinvite;
            SupportReplaces = request.SupportReplaces;
        }
    }
}

