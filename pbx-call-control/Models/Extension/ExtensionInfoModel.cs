using TCX.Configuration;
using PbxApiControl.Enums;

namespace PbxApiControl.Models.Extensions;

public class ExtensionInfo
{
    public string AuthID { get; set; }

    public string AuthPassword { get; set; }

    public string SipID { get; }

    public string Extension { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string Email { get; }

    public string MobileNumber { get; }

    public string OutboundCallerID { get; }

    public RecordType RecordingType { get; }

    public bool IsExtenionEnabled { get; }

    public bool DisableExternalCalls { get; }

    public bool DeliverAudio { get; }

    public bool SupportReinvite { get; }

    public bool SupportReplaces { get; }

    public VoiceMailEmailOptionsTypeEnum EmailOptions { get; }

    public bool VoiceMailEnable { get; }

    public string VoiceMailPin { get; }

    public bool VoiceMailPlayCallerID { get; }

    public bool Internal { get; }

    public int NoAnswerTimeout { get; }


    public ExtensionInfo(Extension ext)
    {
        this.AuthID = ext.AuthID ;
        this.AuthPassword = ext.AuthPassword;
        this.SipID = ext.SIPID;
        this.Extension = ext.Number;
        this.FirstName = ext.FirstName;
        this.LastName = ext.LastName;
        this.Email = ext.EmailAddress;
        this.MobileNumber = ext.GetPropertyValue("MOBILENUMBER");
        this.OutboundCallerID = ext.OutboundCallerID;
        this.RecordingType = GetRecordType(ext);
        this.IsExtenionEnabled = ext.Enabled;
        this.DisableExternalCalls = ext.Internal;
        this.DeliverAudio = ext.DeliverAudio;
        this.SupportReinvite = ext.SupportReinvite;
        this.SupportReplaces = ext.SupportReplaces;
        this.EmailOptions = (VoiceMailEmailOptionsTypeEnum)ext.VMEmailOptions;
        this.VoiceMailEnable = ext.VMEnabled;
        this.VoiceMailPin = ext.VMPIN;
        this.VoiceMailPlayCallerID = ext.VMPlayCallerID;
        this.Internal = ext.Internal;
        this.NoAnswerTimeout = ext.NoAnswerTimeout;
    }

    private static RecordType GetRecordType(Extension ext)
    {
        if (!ext.RecordCalls)
        {
            return RecordType.RecordingOff;
        }

        var externalOnly = ext.GetPropertyValue("RECORD_EXTERNAL_CALLS_ONLY");
        
        return (externalOnly == "1") ? RecordType.RecordingExternal: RecordType.RecordingAll;
    }
}