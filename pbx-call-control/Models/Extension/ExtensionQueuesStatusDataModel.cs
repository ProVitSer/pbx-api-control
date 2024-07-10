using PbxApiControl.Enums;
using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.Models.Extensions
{
    public class ExtensionQueuesStatusDataModel
    {
        public string ExtensionNumber { get; }
        public QueuesStatusType Status { get; }
        
        public ExtensionQueuesStatusDataModel(SetExtensionGlobalQueuesStatusRequest request)
        {
            ExtensionNumber = request.Extension;
            Status = (QueuesStatusType)request.Status;
        }
    }
}

