using PbxApiControl.Enums;
using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.Models.Extensions
{
    public class ExtensionForwardStatusDataMode
    {
        public string ExtensionNumber { get; }

        [EnumDataType(typeof(ForwardingRules))]
        public string Status { get; }
        
        public ExtensionForwardStatusDataMode(SetExtensionForwardStatusRequest request)
        {
            ExtensionNumber = request.Extension;
            Status = request.Status;
        }
    }
}

