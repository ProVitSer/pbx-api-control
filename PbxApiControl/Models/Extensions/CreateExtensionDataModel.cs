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
        public string AuthID { get; }
        public string AuthPassword { get; }
        public string MobileNumber { get; }
        public string OutboundCallerID { get; }
        [EnumDataType(typeof(RecordType))]
        public string RecordingType { get;  }
        public bool IsExtenionEnabled { get; }
        public bool AllowedExternalCalls { get; }
        public bool DeliverAudio { get;  }
        public bool SupportReinvite { get; }
        public bool SupportReplaces { get; }
        
        public CreateExtensionDataModel(CreateExtensionRequest request)
        {
            ExtensionNumber = request.Extension;
            FirstName = request.FirstName;
            LastName = request.LastName;
            Email = request.Email;
            AuthID = request.AuthId ?? request.Extension;
            AuthPassword = request.AuthPassword ?? UtilService.GeneratePassword(12);
            MobileNumber = request.MobileNumber;
            OutboundCallerID = request.OutboundCallerId;
            RecordingType = request.RecordingType;
            IsExtenionEnabled = request.IsExtensionEnabled;
            AllowedExternalCalls = request.AllowedExternalCalls;
            DeliverAudio = request.DeliverAudio;
            SupportReinvite = request.SupportReinvite;
            SupportReplaces = request.SupportReplaces;
        }
    }
}

