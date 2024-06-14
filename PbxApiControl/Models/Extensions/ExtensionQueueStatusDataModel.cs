using PbxApiControl.Enums;
using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.Models.Extensions
{
    public class ExtensionQueueStatusDataModel
    {
        public string ExtensionNumber { get; }
        public string QueueNumber { get;}

        [EnumDataType(typeof(QStatusType))]
        public string Status { get; }
        
        public ExtensionQueueStatusDataModel(SetExtensionStatusInQueueRequest request)
        {
            ExtensionNumber = request.Extension;
            QueueNumber = request.QueueNumber;
            Status = request.Status;
        }
    }
}

