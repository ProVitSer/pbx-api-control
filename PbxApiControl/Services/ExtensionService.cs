using System.Collections.Generic;
using System.Linq;
using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.Models;
using PbxApiControl.DTOs.Extension;
using PbxApiControl.Enums;
using System;



namespace PbxApiControl.Services;

#nullable enable
public class ExtensionService : IExtensionService
{
    private readonly IPbxService _pbxService;

    public ExtensionService(IPbxService pbxService)
    {
        _pbxService = pbxService;
    }

    public IEnumerable<string> GetAllExtensions()
    {
        return _pbxService.GetPbxNumbers<Extension>();
    }

    public IEnumerable<string> GetRegisteredExtensions()
    {
        return GetRegisteredExtensions<Extension>();
    }


    public ExtensionStatus? GetExtensionStatus(string ext)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            if (dnByNumber is Extension extension)
            {
                return new ExtensionStatus(extension);

            }
            return null;
        };

    }

    public ExtensionInfo? GetExtensionInfo(string ext)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            if (dnByNumber is Extension extension)
            {
                return new ExtensionInfo(extension);

            }
            return null;
        };
    }

    public ExtensionInfo? CreateExtension(BaseExtensionDto dto)
    {
        if (CheckExtension(dto.ExtensionNumber))
        {
            return null;
        }

        CreateExtn(dto);

        return GetExtensionInfo(dto.ExtensionNumber);

    }


    public bool? DeleteExtension(DeleteExtensionDto dto)
    {
        if (!CheckExtension(dto.ExtensionNumber))
        {
            return null;
        }

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(dto.ExtensionNumber))
        {
            if (dnByNumber is Extension extension)
            {
                extension.Delete();
            }

        };

        return true;

    }

    public ExtensionInfo? UpdateExtension(BaseExtensionDto dto)
    {
        if (!CheckExtension(dto.ExtensionNumber))
        {
            return null;
        }

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(dto.ExtensionNumber))
        {
            if (dnByNumber is Extension extension)
            {
                SetExtensionProperties(extension, dto);


            }

        };

        return GetExtensionInfo(dto.ExtensionNumber);

    }

    public bool? SetExtensionForwardStatus(SetForwardStatusDto dto)
    {

        if (!CheckExtension(dto.ExtensionNumber))
        {
            return null;
        }


        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(dto.ExtensionNumber))
        {
            if (dnByNumber is Extension extension)
            {
                foreach (FwdProfile fwdProfile in extension.FwdProfiles)
                {
                    if (fwdProfile.Name == GetForwardingRulesStatus(dto.Status))
                    {
                        extension.CurrentProfile = fwdProfile;
                        extension.OverrideExpiresAt = DateTime.UtcNow;
                        extension.Save();
                        return true;
                    }
                }

            }
            return false;
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


    private static void CreateExtn(BaseExtensionDto dto)
    {
        using (Extension extension = PhoneSystem.Root.GetTenant().CreateExtension(dto.ExtensionNumber))
        {
            SetExtensionProperties(extension, dto);
        };
    }

    private static void SetExtensionProperties(Extension extension, BaseExtensionDto dto)
    {


        Console.WriteLine(dto.FirstName);
        Console.WriteLine(dto.LastName);

        SetIfNotNull(() => extension.FirstName = dto.FirstName);
        SetIfNotNull(() => extension.LastName = dto.LastName);
        SetIfNotNull(() => extension.EmailAddress = dto.Email);
        SetIfNotNull(() => extension.AuthID = dto.AuthID ?? dto.ExtensionNumber);
        SetIfNotNull(() => extension.AuthPassword = dto.AuthPassword ?? UtilService.GeneratePassword(12));
        SetOptionalProperty("MOBILENUMBER", extension, dto.MobileNumber);
        SetRecordingTypeProperties(extension, dto.RecordingType);
        SetIfNotNull(() => extension.OutboundCallerID = dto.OutboundCallerID);
        SetIfNotNull(() => extension.Enabled = dto.IsExtenionEnabled);
        SetIfNotNull(() => extension.Internal = dto.AllowedExternalCalls);
        SetIfNotNull(() => extension.DeliverAudio = dto.DeliverAudio);
        SetIfNotNull(() => extension.SupportReinvite = dto.SupportReinvite);
        SetIfNotNull(() => extension.SupportReplaces = dto.SupportReplaces);

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

    private static bool CheckExtension(string ext)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            return dnByNumber is Extension;
        };
    }


    private IEnumerable<string> GetRegisteredExtensions<T>() where T : class, DN
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

    public bool? SetExtensionQueuesStatus(SetQueuestatusDto dto)
    {
        if (!CheckExtension(dto.ExtensionNumber))
        {
            return null;
        };

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(dto.ExtensionNumber))
        {
            if (dnByNumber is Extension extension)
            {
                extension.QueueStatus = dto.Status == QueuesStatusType.LoggedIn.ToString() ? QueueStatusType.LoggedIn : QueueStatusType.LoggedOut;
                extension.Save();
            }
        };

        return true;
    }

    public bool? SetExtensionQueueStatus(SetQueuetatusDto dto)
    {
        if (!CheckExtension(dto.ExtensionNumber))
        {
            return null;
        };

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(dto.ExtensionNumber))
        {
            if (dnByNumber is Extension extension)
            {
                foreach (QueueAgent queueAgent in extension.QueueMembership)
                {
                    if (queueAgent.Queue.Number == dto.QueueNumber)
                    {
                        queueAgent.QueueStatus = dto.Status == QStatusType.On.ToString() ? QueueStatusType.LoggedIn : QueueStatusType.LoggedOut;
                        extension.Save();
                        return true;
                    }


                }
            }
            return false;
        };


    }
}