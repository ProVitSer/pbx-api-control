using PbxApiControl.Enums;
using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.Models.Extensions
{
    public class ExtensionQueueStatusDataModel
    {
        public string ExtensionNumber { get; }
        public string QueueNumber { get;}
        public QueuesStatusType Status { get; }
        
        public ExtensionQueueStatusDataModel(SetExtensionStatusInQueueRequest request)
        {
            ExtensionNumber = request.Extension;
            QueueNumber = request.QueueNumber;
            Status = (QueuesStatusType)request.Status;
        }
    }
}

