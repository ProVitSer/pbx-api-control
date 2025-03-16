using PbxApiControl.Interface;
using PbxApiControl.Models.Call;
using TCX.Configuration;

namespace PbxApiControl.Services.Utils
{
    public class LogUtilService : ILogUtilService
    {
        private readonly ILogger<LogUtilService> _logger;

        public LogUtilService(ILogger<LogUtilService> logger)
        {
            _logger = logger;
        }

        public void LogCallsState(List<CallStateModel> callsState)
        {
            foreach (var call in callsState)
            {
                _logger.LogInformation(
                    "Call ID: {CallID}, Direction: {CallDirection}, Status: {Status}, Local: {LocalNumber}, Remote: {RemoteNumber}",
                    call.CallID,
                    call.CallDirection,
                    call.Status,
                    call.LocalNumber,
                    call.RemoteNumber);
            }
        }

        public void LogUpdateExtInfo(Extension extension)
        {
            var extensionDetails = FormatExtensionDetails(extension, "Extension details");
            _logger.LogInformation(extensionDetails);
        }

        public void LogSetExtensionProperties(Extension extension)
        {
            var extensionProperties = FormatExtensionDetails(extension, "Extension properties");
            _logger.LogInformation(extensionProperties);
        }

        private string FormatExtensionDetails(Extension extension, string header)
        {
            var properties = new Dictionary<string, object?>
            {
                ["AuthID"] = extension.AuthID,
                ["AuthPassword"] = extension.AuthPassword,
                ["BusyDetection"] = extension.BusyDetection,
                ["DeliverAudio"] = extension.DeliverAudio,
                ["EmailAddress"] = extension.EmailAddress,
                ["Enabled"] = extension.Enabled,
                ["LastName"] = extension.LastName,
                ["FirstName"] = extension.FirstName,
                ["HidePresence"] = extension.HidePresence,
                ["Internal"] = extension.Internal,
                ["NoAnswerTimeout"] = extension.NoAnswerTimeout,
                ["Number"] = extension.Number,
                ["OutboundCallerID"] = extension.OutboundCallerID,
                ["QueueStatus"] = extension.QueueStatus,
                ["RecordingType"] = extension.GetPropertyValue("RECORD_EXTERNAL_CALLS_ONLY"),
                ["SIPID"] = extension.SIPID,
                ["SupportReinvite"] = extension.SupportReinvite,
                ["SupportReplaces"] = extension.SupportReplaces,
                ["UserStatus"] = extension.UserStatus,
                ["VMEmailOptions"] = extension.VMEmailOptions,
                ["VMEnabled"] = extension.VMEnabled,
                ["VMPIN"] = extension.VMPIN,
                ["VMPlayCallerID"] = extension.VMPlayCallerID,
                ["VMPlayMsgDateTime"] = extension.VMPlayMsgDateTime,
                ["MobileNumber"] = extension.GetPropertyValue("MOBILENUMBER"),
                ["ALLOW_EXTERNAL_PROVIDER"] = extension.GetPropertyValue("ALLOW_EXTERNAL_PROVIDER"),
                ["CALL_US_ENABLE_PHONE"] = extension.GetPropertyValue("CALL_US_ENABLE_PHONE"),
                ["CALL_US_ENABLE_CHAT"] = extension.GetPropertyValue("CALL_US_ENABLE_CHAT"),
                ["CALL_US_ENABLE_VIDEO"] = extension.GetPropertyValue("CALL_US_ENABLE_VIDEO"),
                ["EXTGUID"] = extension.GetPropertyValue("EXTGUID"),
                ["PUSH_EXTENSION"] = extension.GetPropertyValue("PUSH_EXTENSION")
            };

            return $"{header}: {string.Join(", ", properties.Select(p => $"{p.Key}={p.Value ?? "null"}"))}";
        }
    }
}
