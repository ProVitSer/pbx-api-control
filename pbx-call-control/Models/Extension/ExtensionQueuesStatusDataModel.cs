using PbxApiControl.Enums;
using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.Models.Extensions
{
    public class ExtensionQueuesStatusDataModel
    {
        public string ExtensionNumber { get; }

        [EnumDataType(typeof(QueuesStatusType))]
        public string Status { get; }
        
        public ExtensionQueuesStatusDataModel(SetExtensionGlobalQueuesStatusRequest request)
        {
            ExtensionNumber = request.Extension;
            Status = request.Status;
        }
    }
}

