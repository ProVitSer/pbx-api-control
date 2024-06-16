
using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.Models.Extensions;
using PbxApiControl.Enums;


namespace PbxApiControl.Services.Pbx;

public class ExtensionService : IExtensionService
{
    private readonly ILogger<ExtensionService> _logger;
    public IEnumerable<string> Numbers { get; private set; }

    public ExtensionService(ILogger<ExtensionService> logger)
    {
        _logger = logger;
        Numbers = new List<string>();
    }


    public ExtensionStatus ExtensionStatus(string ext)
    {

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {

            if (!(dnByNumber is Extension))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
            }
            
            var extension = (Extension)dnByNumber;
            return new ExtensionStatus(extension);
        }
    }
    
    public ExtensionInfo ExtensionInfo(string ext)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            
            if (!(dnByNumber is Extension))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
            }
            
            var extension = (Extension)dnByNumber;

            return new ExtensionInfo(extension);
        };

    }
    
    public IEnumerable<string> AllExtensions()
    {
        using (IArrayDisposer<Extension> disposer = PhoneSystem.Root.GetAll<Extension>().GetDisposer())
        {
            this.Numbers = disposer.Select(x => x.Number).ToArray();

            return this.Numbers;
        };
    }
    
    public IEnumerable<string> RegisteredExtensions()
    {
        return RegisteredExtensions<Extension>();
    }
    
    public ExtensionDeviceInfo ExtensionDeviceInfo(string ext)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            if (!(dnByNumber is Extension))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
            }
            
            using (IArrayDisposer<RegistrarRecord> disposer = dnByNumber.GetRegistrarContactsEx().GetDisposer<RegistrarRecord>())
            {
                List<DevInfo> devices = new List<DevInfo>();
                for (int i = 0; i < disposer.Length; ++i)
                {
                    devices.Add(new DevInfo
                    {
                        Contact = disposer[i].Contact,
                        UserAgent = disposer[i].UserAgent
                    });
                }

                return new ExtensionDeviceInfo()
                {
                    Extension = ext,
                    Devices = devices
                };

            };
        };
        
    }
    
    public ExtensionInfo CreateExt(CreateExtensionDataModel data)
    {

        using (Extension extension = PhoneSystem.Root.GetTenant().CreateExtension(data.ExtensionNumber))
        {
            
            SetExtensionProperties(extension, data);
            
        };

        return ExtensionInfo(data.ExtensionNumber);
    }
    
    public bool DeleteExt(string ext)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            if (dnByNumber is Extension extension)
            {
                extension.Delete();
            }

        };

        return true;

    }
    
    public ExtensionInfo UpdateExt(UpdateExtensionDataModel data)
    {
        var extensionInfo = ExtensionInfo(data.ExtensionNumber);
            
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(data.ExtensionNumber))
        {
            if (dnByNumber is Extension extension)
            {
                extension.FirstName =  data.FirstName != null ? data.FirstName : extensionInfo.FirstName;
                extension.LastName = data.LastName != null ? data.LastName : extensionInfo.LastName;
                extension.EmailAddress = data.Email != null ? data.Email : extensionInfo.Email;
                extension.AuthID = data.AuthID != null ? data.AuthID : extensionInfo.AuthID;
                extension.AuthPassword = data.AuthPassword != null ? data.AuthPassword : extensionInfo.AuthPassword;
                extension.SetProperty("MOBILENUMBER", data.MobileNumber != null ? data.MobileNumber : extensionInfo.MobileNumber);
                SetRecordingTypeProperties(extension, data.RecordingType != null ? data.RecordingType : extensionInfo.RecordingType);
                extension.OutboundCallerID = data.OutboundCallerID != null ? data.OutboundCallerID : extensionInfo.OutboundCallerID;
                extension.Enabled = data.IsExtenionEnabled != null ? data.IsExtenionEnabled.Value: extensionInfo.IsExtenionEnabled;
                extension.Internal = data.DisableExternalCalls != null ? data.DisableExternalCalls.Value : extensionInfo.DisableExternalCalls;
                extension.DeliverAudio = data.DeliverAudio != null ? data.DeliverAudio.Value : extensionInfo.DeliverAudio;
                extension.SupportReinvite = data.SupportReinvite != null ? data.SupportReinvite.Value : extensionInfo.SupportReinvite;
                extension.SupportReplaces = data.SupportReplaces != null ? data.SupportReplaces.Value : extensionInfo.SupportReplaces;

                extension.Save();

            }

        };

        return ExtensionInfo(data.ExtensionNumber);

    }

    public ExtensionStatus SetExtForwardStatus(ExtensionForwardStatusDataMode data)
    {

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(data.ExtensionNumber))
        {
            if (!(dnByNumber is Extension))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
            }
            
            var extension = (Extension)dnByNumber;
            
            foreach (FwdProfile fwdProfile in extension.FwdProfiles)
            {
                if (fwdProfile.Name == GetForwardingRulesStatus(data.Status))
                {
                    extension.CurrentProfile = fwdProfile;
                    extension.OverrideExpiresAt = DateTime.UtcNow;
                    extension.Save();
                }
            }
            
            return new ExtensionStatus(extension);

        };

    }
    
    public ExtensionStatus SetExtQueuesStatus(ExtensionQueuesStatusDataModel data)
    {

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(data.ExtensionNumber))
        {
            
            if (!(dnByNumber is Extension))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
            }
            
            var extension = (Extension)dnByNumber;
            
            extension.QueueStatus = data.Status == QueuesStatusType.LoggedIn.ToString() ? QueueStatusType.LoggedIn : QueueStatusType.LoggedOut;
            extension.Save();
             
            return new ExtensionStatus(extension);

        };
    }
    
    public ExtensionStatus SetExtQueueStatus(ExtensionQueueStatusDataModel data)
    {

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(data.ExtensionNumber))
        {
            if (!(dnByNumber is Extension))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
            }
            
            var extension = (Extension)dnByNumber;
            
            foreach (QueueAgent queueAgent in extension.QueueMembership)
            {
                if (queueAgent.Queue.Number == data.QueueNumber)
                {
                    queueAgent.QueueStatus = data.Status == QStatusType.On.ToString() ? QueueStatusType.LoggedIn : QueueStatusType.LoggedOut;
                    extension.Save();
                }
            }
            
            return new ExtensionStatus(extension);
        };

    }
    public bool IsExtensionExists(string ext)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {

            return dnByNumber is Extension;
        };
    }
    
    private static string GetForwardingRulesStatus(string status)
    {
        switch (status)
        {
            case "DND":
                return "Out of office";
            case "Lunch":
                return "Custom 1";
            case "BusinessTrip":
                return "Custom 2";
            default:
                return status;
        };
    }
    private static void SetExtensionProperties(Extension extension, CreateExtensionDataModel data)
    {
        Guid newGuid = Guid.NewGuid();
        
        extension.AuthID = data.AuthID;
        extension.AuthPassword = data.AuthPassword;
        extension.BusyDetection = BusyDetectionType.UsePBXStatus;
        extension.DeliverAudio = data.DeliverAudio;
        extension.EmailAddress = data.Email;
        extension.Enabled = !data.IsExtenionEnabled;
        extension.LastName = data.LastName;
        extension.FirstName = data.FirstName;
        extension.HidePresence = false;
        extension.Internal = data.DisableExternalCalls;
        extension.NoAnswerTimeout = 60;
        extension.Number = data.ExtensionNumber;
        extension.OutboundCallerID = data.OutboundCallerID;
        extension.QueueStatus = QueueStatusType.LoggedOut;
        SetRecordingTypeProperties(extension, data.RecordingType);
        extension.SIPID = data.ExtensionNumber;
        extension.SupportReinvite = data.SupportReinvite;
        extension.SupportReplaces = data.SupportReplaces;
        extension.UserStatus = UserStatusType.Available;
        extension.VMEmailOptions = VMEmailOptionsType.None;
        extension.VMEnabled = true;
        extension.VMPIN = data.ExtensionNumber;
        extension.VMPlayCallerID = true;
        extension.VMPlayMsgDateTime = VMPlayMsgDateTimeType.Play24Hr;
        SetOptionalProperty("MOBILENUMBER", extension, data.MobileNumber);
        SetOptionalProperty("ALLOW_EXTERNAL_PROVIDER", extension, "0");
        SetOptionalProperty("CALL_US_ENABLE_PHONE", extension, "0");
        SetOptionalProperty("CALL_US_ENABLE_CHAT", extension, "0");
        SetOptionalProperty("CALL_US_ENABLE_VIDEO", extension, "0");
        SetOptionalProperty("ALLOW_EXTERNAL_PROVIDER", extension, "0");
        SetOptionalProperty("EXTGUID", extension, newGuid.ToString());
        SetOptionalProperty("PUSH_EXTENSION", extension, "1");
        extension.Save();
    }
    
    private static void SetIfNotNull(Action action)
    {
        action?.Invoke();
    }
    
    private static void SetOptionalProperty<T>(string propertyName, Extension extension, T value)
    {
        if (value != null)
        {
            extension.SetProperty(propertyName, value.ToString());
        }
    }

    private static void SetRecordingTypeProperties(Extension extension, string recordingType)
    {
        if (Enum.TryParse<RecordType>(recordingType, out var recordingTypeEnum))
        {
            extension.RecordCalls = true;

            switch (recordingTypeEnum)
            {
                case RecordType.RecordingExternal:
                    extension.SetProperty("RECORD_EXTERNAL_CALLS_ONLY", "1");
                    break;
                case RecordType.RecordingAll:
                    extension.SetProperty("RECORD_EXTERNAL_CALLS_ONLY", "0");
                    break;
                case RecordType.RecordingOff:
                    extension.RecordCalls = false;
                    break;
            }
        }
    }

    
    private static IEnumerable<string> RegisteredExtensions<T>() where T : class, DN
    {

        using (IArrayDisposer<T> disposer = PhoneSystem.Root.GetAll<T>().GetDisposer())
        {
            var RegNumber = disposer
                .Where(x => x.IsRegistered)
                .Select(x => x.Number)
                .ToArray();

            return RegNumber;
        };
    }
}


