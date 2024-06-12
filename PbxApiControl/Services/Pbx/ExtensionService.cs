
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


    public ExtensionStatus? ExtensionStatus(string ext)
    {

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            if (dnByNumber is Extension extension)
            {
                return new ExtensionStatus(extension);

            }
        };

        return null;
    }
    
    public ExtensionInfo? ExtensionInfo(string ext)
    {

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            if (dnByNumber is Extension extension)
            {
                return new ExtensionInfo(extension);
            }
        };

        return null;
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
    
    public ExtensionDeviceInfo? ExtensionDeviceInfo(string ext)
    {
        if (!CheckExtension(ext))
        {
            return null;
        };

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            if (dnByNumber is Extension extension)
            {
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

            }
        };

        return null;

    }
    
    public ExtensionInfo? CreateExt(CreateExtensionDataModel data)
    {
        var tt = CheckExtension(data.ExtensionNumber);
        
        if (CheckExtension(data.ExtensionNumber))
        {
            return null;
        }

        using (Extension extension = PhoneSystem.Root.GetTenant().CreateExtension(data.ExtensionNumber))
        {

            SetExtensionProperties(extension, data);
        };

        return ExtensionInfo(data.ExtensionNumber);

    }

    private static void SetExtensionProperties(Extension extension, CreateExtensionDataModel data)
    {

        SetIfNotNull(() => extension.FirstName = data.FirstName);
        SetIfNotNull(() => extension.LastName = data.LastName);
        SetIfNotNull(() => extension.EmailAddress = data.Email);
        SetIfNotNull(() => extension.AuthID = data.AuthID);
        SetIfNotNull(() => extension.AuthPassword = data.AuthPassword);
        SetOptionalProperty("MOBILENUMBER", extension, data.MobileNumber);
        SetRecordingTypeProperties(extension, data.RecordingType);
        SetIfNotNull(() => extension.OutboundCallerID = data.OutboundCallerID);
        SetIfNotNull(() => extension.Enabled = data.IsExtenionEnabled);
        SetIfNotNull(() => extension.Internal = data.AllowedExternalCalls);
        SetIfNotNull(() => extension.DeliverAudio = data.DeliverAudio);
        SetIfNotNull(() => extension.SupportReinvite = data.SupportReinvite);
        SetIfNotNull(() => extension.SupportReplaces = data.SupportReplaces);

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
    
    private static bool CheckExtension(string ext)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            return dnByNumber is Extension;
        };
    }

}


