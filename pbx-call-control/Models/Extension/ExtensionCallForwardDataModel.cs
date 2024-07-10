using PbxApiControl.Enums;

namespace PbxApiControl.Models.Extensions
{
    public class ExtensionCallForwardDataModel
    {
        public string ExtensionNumber { get; }
        public ForwardingRules FwStatus { get; }
        public ForwardingToEnum FwTo { get; }
        public ForwardingCallTypeEnum FwCall { get; }
        public ExtensionStateTypeEnum? ExtensionState { get; }
        public string? Number { get; }

        
        public ExtensionCallForwardDataModel(SetExtensionCallForwardStatusRequest request)
        {
            ExtensionNumber = request.Extension;
            FwStatus = (ForwardingRules)request.FwStatus;
            FwTo = (ForwardingToEnum)request.FwTo;
            FwCall = (ForwardingCallTypeEnum)request.FwCall;
            ExtensionState = (ExtensionStateTypeEnum)request.ExtensionState;
            Number = request.Number;
        }
    }
}