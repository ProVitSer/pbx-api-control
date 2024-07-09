using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.Models.Extensions;
using PbxApiControl.Enums;
using PbxApiControl.Constants;

namespace PbxApiControl.Services.Pbx
{
    public class ExtensionService : IExtensionService
    {
        private readonly ILogger<ExtensionService> _logger;
        private readonly ILogUtilService _logUtilService;

        public ExtensionService(ILogger<ExtensionService> logger, ILogUtilService logUtilService)
        {
            _logger = logger;
            _logUtilService = logUtilService;
        }

        public ExtensionStatus ExtensionStatus(string ext)
        {
            using (var dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
            {
                if (!(dnByNumber is Extension))
                {
                    throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
                }

                var extension = (Extension)dnByNumber;
                
                return new ExtensionStatus(extension, ExtensionDeviceInfo(ext));
            }
        }

        public ExtensionInfo ExtensionInfo(string ext)
        {
            using (var dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
            {
                if (!(dnByNumber is Extension))
                {
                    throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
                }

                var extension = (Extension)dnByNumber;
                
                return new ExtensionInfo(extension);
            }
        }

        public IEnumerable<string> AllExtensions()
        {
            using (var disposer = PhoneSystem.Root.GetAll<Extension>().GetDisposer())
            {
                return disposer.Select(x => x.Number).ToArray();
            }
        }

        public IEnumerable<string> RegisteredExtensions()
        {
            return RegisteredExtensions<Extension>();
        }

        public ExtensionDeviceInfo ExtensionDeviceInfo(string ext)
        {
            using (var dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
            {
                if (!(dnByNumber is Extension))
                {
                    throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
                }

                var extension = (Extension)dnByNumber;
                
                using (var disposer = dnByNumber.GetRegistrarContactsEx().GetDisposer<RegistrarRecord>())
                {
                    var devices = disposer.Select(record => new DevInfo
                    {
                        Contact = record.Contact,
                        UserAgent = record.UserAgent
                    }).ToList();

                    return new ExtensionDeviceInfo()
                    {
                        Extension = ext,
                        FirstName = extension.FirstName,
                        LastName = extension.LastName,
                        Email = extension.EmailAddress,
                        Devices = devices
                    };
                }
            }
        }

        public ExtensionInfo CreateExt(CreateExtensionDataModel data)
        {
            using (var extension = PhoneSystem.Root.GetTenant().CreateExtension(data.ExtensionNumber))
            {
                SetExtensionProperties(extension, data);
            }

            return ExtensionInfo(data.ExtensionNumber);
        }

        public bool DeleteExt(string ext)
        {
            using (var dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
            {
                if (dnByNumber is Extension extension)
                {
                    extension.Delete();
                }
            }

            return true;
        }

        public ExtensionInfo UpdateExt(UpdateExtensionDataModel data)
        {
            var extensionInfo = ExtensionInfo(data.ExtensionNumber);

            using (var dnByNumber = PhoneSystem.Root.GetDNByNumber(data.ExtensionNumber))
            {
                if (dnByNumber is Extension extension)
                {
                    extension.FirstName = data.FirstName ?? extensionInfo.FirstName;
                    extension.LastName = data.LastName ?? extensionInfo.LastName;
                    extension.EmailAddress = data.Email ?? extensionInfo.Email;
                    extension.AuthID = data.AuthId ?? extensionInfo.AuthID;
                    extension.AuthPassword = data.AuthPassword ?? extensionInfo.AuthPassword;
                    extension.SetProperty("MOBILENUMBER", data.MobileNumber ?? extensionInfo.MobileNumber);
                    SetRecordingTypeProperties(extension, data.RecordingType ?? (RecordType)extensionInfo.RecordingType);
                    extension.OutboundCallerID = data.OutboundCallerId ?? extensionInfo.OutboundCallerID;
                    extension.Enabled = data.IsExtenionEnabled ?? extensionInfo.IsExtenionEnabled;
                    extension.Internal = data.DisableExternalCalls ?? extensionInfo.DisableExternalCalls;
                    extension.DeliverAudio = data.DeliverAudio ?? extensionInfo.DeliverAudio;
                    extension.SupportReinvite = data.SupportReinvite ?? extensionInfo.SupportReinvite;
                    extension.SupportReplaces = data.SupportReplaces ?? extensionInfo.SupportReplaces;

                    _logUtilService.LogUpdateExtInfo(extension);
                    
                    extension.Save();
                }
            }
            


            return ExtensionInfo(data.ExtensionNumber);
        }

        public ExtensionStatus SetExtForwardStatus(ExtensionForwardStatusDataModel data)
        {
            using (var dnByNumber = PhoneSystem.Root.GetDNByNumber(data.ExtensionNumber))
            {
                if (!(dnByNumber is Extension))
                {
                    throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
                }

                var extension = (Extension)dnByNumber;

                foreach (var fwdProfile in extension.FwdProfiles)
                {
                    if (fwdProfile.Name == GetForwardingRulesStatus(data.FwStatus))
                    {
                        extension.CurrentProfile = fwdProfile;
                        extension.OverrideExpiresAt = DateTime.UtcNow;
                        extension.Save();
                    }
                }

                return ExtensionStatus(data.ExtensionNumber);
            }
        }
        
       public ExtensionStatus SetExtCallForwarding(ExtensionCallForwardDataModel data)
        {
            using (var dnByNumber = PhoneSystem.Root.GetDNByNumber(data.ExtensionNumber))
            {
                if (!(dnByNumber is Extension))
                {
                    throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
                }

                var extension = (Extension)dnByNumber;

                var frs = GetForwardingRulesStatus(data.FwStatus);
                
                var newfwdProfile = extension.CurrentProfile;

                foreach (var fwdProfile in extension.FwdProfiles)
                {
                    if (fwdProfile.Name == frs) 
                    {
                        newfwdProfile = UpdateRoute(extension, fwdProfile, data);
                    }
                }

                extension.CurrentProfile = newfwdProfile;
                extension.OverrideExpiresAt = DateTime.UtcNow;
                extension.Save();

                return ExtensionStatus(data.ExtensionNumber);
            }
        }

        private FwdProfile UpdateRoute(Extension extension, FwdProfile fwdProfile, ExtensionCallForwardDataModel data)
        {
            var dest = CreateDestination(extension, data);

            if (fwdProfile.TypeOfRouting == RoutingType.Available)
            {
                UpdateAvailableRoute(fwdProfile.AvailableRoute, dest, data);
            }
            else if (fwdProfile.TypeOfRouting == RoutingType.Away)
            {
                UpdateAwayRoute(fwdProfile.AwayRoute, dest, data);
            }

            return fwdProfile;
        }

        private DestinationStruct CreateDestination(Extension extension, ExtensionCallForwardDataModel data)
        {
            var dest = new DestinationStruct
            {
                To = GetDestinationType(data.FwTo)
            };

            switch (data.FwTo)
            {
                case ForwardingToEnum.Extension:
                case ForwardingToEnum.Queue:
                case ForwardingToEnum.IVR:
                case ForwardingToEnum.RingGroup:
                    if (string.IsNullOrEmpty(data.Number))
                    {
                        throw new InvalidOperationException(ServiceConstants.DataError);
                    }
                    dest.Internal = PhoneSystem.Root.GetDNByNumber(data.Number);
                    break;
                case ForwardingToEnum.Mobile:
                    dest.External = extension.GetPropertyValue("MOBILENUMBER");
                    break;
                case ForwardingToEnum.External:
                    dest.External = data.Number;
                    break;
                case ForwardingToEnum.VoiceMail:
                    dest.Internal = PhoneSystem.Root.GetDNByNumber(data.ExtensionNumber);
                    break;
            }

            return dest;
        }

        private void UpdateAwayRoute(AwayRouting route, DestinationStruct dest, ExtensionCallForwardDataModel data)
        {
            switch (data.FwCall)
            {
                case  ForwardingCallTypeEnum.ExternalCall:
                    route.External.AllHours = dest;
                    route.External.OutOfOfficeHours = dest;
                    break;
                case ForwardingCallTypeEnum.InternalCall:
                    route.Internal.AllHours = dest;
                    route.Internal.OutOfOfficeHours = dest;
                    break;
                case ForwardingCallTypeEnum.BothCall:
                    route.External.AllHours = dest;
                    route.External.OutOfOfficeHours = dest;
                    route.Internal.AllHours = dest;
                    route.Internal.OutOfOfficeHours = dest;
                    break;
            }
        }

        private void UpdateAvailableRoute(AvailableRouting route, DestinationStruct dest, ExtensionCallForwardDataModel data)
        {
            void UpdateNoAnswer()
            {
                var noAnswerRoute = route.NoAnswer.Internal;
                
                if (data.FwCall == ForwardingCallTypeEnum.ExternalCall)
                {
                    route.NoAnswer.AllCalls = dest;
                    route.NoAnswer.Internal = noAnswerRoute;
                }
                
                if (data.FwCall == ForwardingCallTypeEnum.InternalCall )
                {
                    route.NoAnswer.Internal = dest;
                }
            }

            void UpdateBusyNotRegistered()
            {
                var busyRoute = route.Busy.Internal;
                var notRegisteredRoute = route.NotRegistered.Internal;
                
                if (data.FwCall == ForwardingCallTypeEnum.ExternalCall )
                {
                    route.Busy.AllCalls = dest;
                    route.Busy.Internal = busyRoute;
                    route.NotRegistered.AllCalls = dest;
                    route.NotRegistered.Internal = notRegisteredRoute;
                }
                
                if (data.FwCall == ForwardingCallTypeEnum.InternalCall)
                {
                    route.Busy.Internal = dest;
                    route.NotRegistered.Internal = dest;
                }
            }

            switch (data.ExtensionState)
            {
                case ExtensionStateTypeEnum.NoAnswer:
                    UpdateNoAnswer();
                    break;
                case ExtensionStateTypeEnum.BusyNotRegistered:
                    UpdateBusyNotRegistered();
                    break;
            }

            if (data.FwCall == ForwardingCallTypeEnum.BothCall)
            {
                route.NoAnswer.AllCalls = dest;
                route.Busy.AllCalls = dest;
                route.NotRegistered.AllCalls = dest;
                route.Busy.Internal = dest;
                route.NoAnswer.Internal = dest;
                route.NotRegistered.Internal = dest;
            }
        }
        
        private static DestinationType GetDestinationType(ForwardingToEnum fwType)
        {
            return fwType switch
            {
                ForwardingToEnum.Extension => DestinationType.Extension,
                ForwardingToEnum.External => DestinationType.External,
                ForwardingToEnum.Mobile => DestinationType.External,
                ForwardingToEnum.Queue => DestinationType.Queue,
                ForwardingToEnum.RingGroup => DestinationType.RingGroup, 
                ForwardingToEnum.IVR => DestinationType.IVR,
                ForwardingToEnum.EndCall => DestinationType.None,
                ForwardingToEnum.VoiceMail => DestinationType.VoiceMail,
                _ => throw new ArgumentOutOfRangeException(nameof(fwType), $"Unsupported forwarding type: {fwType}")

            };
        }

            

        public ExtensionStatus SetExtQueuesStatus(ExtensionQueuesStatusDataModel data)
        {
            using (var dnByNumber = PhoneSystem.Root.GetDNByNumber(data.ExtensionNumber))
            {
                if (!(dnByNumber is Extension))
                {
                    throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
                }

                var extension = (Extension)dnByNumber;

                extension.QueueStatus = data.Status.ToString() == QueuesStatusType.LoggedIn.ToString()
                    ? QueueStatusType.LoggedIn
                    : QueueStatusType.LoggedOut;

                foreach (var queueAgent in extension.QueueMembership)
                {
                    queueAgent.QueueStatus = data.Status.ToString() == QueuesStatusType.LoggedIn.ToString()
                        ? QueueStatusType.LoggedIn
                        : QueueStatusType.LoggedOut;
                }
                extension.Save();

                return ExtensionStatus(data.ExtensionNumber);
            }
        }

        public ExtensionStatus SetExtQueueStatus(ExtensionQueueStatusDataModel data)
        {
            using (var dnByNumber = PhoneSystem.Root.GetDNByNumber(data.ExtensionNumber))
            {
                if (!(dnByNumber is Extension))
                {
                    throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
                }

                var extension = (Extension)dnByNumber;

                foreach (var queueAgent in extension.QueueMembership)
                {
                    if (queueAgent.Queue.Number == data.QueueNumber)
                    {
                        queueAgent.QueueStatus = data.Status.ToString() == QueueStatusType.LoggedIn.ToString()
                            ? QueueStatusType.LoggedIn
                            : QueueStatusType.LoggedOut;
                        extension.Save();
                    }
                }

                return ExtensionStatus(data.ExtensionNumber);
            }
        }

        public bool IsExtensionExists(string ext)
        {
            using (var dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
            {
                return dnByNumber is Extension;
            }
        }

        private static string GetForwardingRulesStatus(ForwardingRules status)
        {
            return status switch
            {
                ForwardingRules.DND => "Out of office",
                ForwardingRules.Lunch => "Custom 1",
                ForwardingRules.BusinessTrip => "Custom 2",
                ForwardingRules.Available => "Available",
                ForwardingRules.Away => "Away",

            };
        }
        
        private void SetExtensionProperties(Extension extension, CreateExtensionDataModel data)
        {
            var newGuid = Guid.NewGuid();

            extension.AuthID = data.AuthId;
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
            extension.OutboundCallerID = data.OutboundCallerId;
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

            _logUtilService.LogSetExtensionProperties(extension);
            
            extension.Save();
        }

        private static void SetOptionalProperty<T>(string propertyName, Extension extension, T value)
        {
            if (value != null)
            {
                extension.SetProperty(propertyName, value.ToString());
            }
        }

        private static void SetRecordingTypeProperties(Extension extension, RecordType recordingType)
        {
                extension.RecordCalls = true;

                switch (recordingType)
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

        private static IEnumerable<string> RegisteredExtensions<T>() where T : class, DN
        {
            using (var disposer = PhoneSystem.Root.GetAll<T>().GetDisposer())
            {
                return disposer
                    .Where(x => x.IsRegistered)
                    .Select(x => x.Number)
                    .ToArray();
            }
        }
    }
}