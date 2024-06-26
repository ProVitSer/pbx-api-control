using PbxApiControl.Enums;
using System.ComponentModel.DataAnnotations;
using PbxApiControl.Services.Utils;


namespace PbxApiControl.Models.Extensions
{
    public class CreateExtensionDataModel
    {
        public string ExtensionNumber { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string AuthId { get; }
        public string AuthPassword { get; }
        public string MobileNumber { get; }
        public string OutboundCallerId { get; }
        [EnumDataType(typeof(RecordType))]
        public string RecordingType { get;  }
        public bool IsExtenionEnabled { get; }
        public bool DisableExternalCalls { get; }
        public bool DeliverAudio { get;  }
        public bool SupportReinvite { get; }
        public bool SupportReplaces { get; }
        
        public CreateExtensionDataModel(CreateExtensionRequest request)
        {
            ExtensionNumber = request.Extension;
            FirstName = request.FirstName;
            LastName = request.LastName;
            Email = request.Email;
            AuthId = request.AuthId ?? request.Extension;
            AuthPassword = request.AuthPassword == "" ? UtilService.GeneratePassword(12) : request.AuthPassword;
            MobileNumber = request.MobileNumber;
            OutboundCallerId = request.OutboundCallerId;
            RecordingType = request.RecordingType;
            IsExtenionEnabled = request.IsExtensionEnabled;
            DisableExternalCalls = request.DisableExternalCalls;
            DeliverAudio = request.DeliverAudio;
            SupportReinvite = request.SupportReinvite;
            SupportReplaces = request.SupportReplaces;
        }
    }
}

