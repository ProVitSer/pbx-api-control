using System.ComponentModel.DataAnnotations;
using TCX.Configuration;

namespace PbxApiControl.Models.Extensions
{
    public class ExtensionCallForwardDataModel
    {
        public string ExtensionNumber { get; }
        public string FwStatus { get; }
        public string FwTo { get; }
        public string fwCall { get; }
        public string? ExtensionState { get; }
        public string? Number { get; }

        
        public ExtensionCallForwardDataModel(SetExtensionCallForwardStatusRequest request)
        {
            ExtensionNumber = request.Extension;
            FwStatus = request.FwStatus;
            FwTo = request.FwTo;
            fwCall = request.FwCall;
            ExtensionState = request.ExtensionState;
            Number = request.Number;
        }
    }
}