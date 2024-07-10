using PbxApiControl.Enums;

namespace PbxApiControl.Models.Extensions
{
    public class ExtensionForwardStatusDataModel
    {
        public string ExtensionNumber { get; }
        public ForwardingRules FwStatus { get; }
        
        public ExtensionForwardStatusDataModel(SetExtensionForwardStatusRequest request)
        {
            ExtensionNumber = request.Extension;
            FwStatus = (ForwardingRules)request.FwStatus;
        }
    }
}

