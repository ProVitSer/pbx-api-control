using TCX.Configuration;
using PbxApiControl.Enums;
using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.Models.Queue;
public class QueueAgentsDataModels
{
    public string Extension { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public QueuesStatusType AgentQueueStatus { get;  }
    
    
    public QueueAgentsDataModels(Extension extension)
    {
        Extension = extension.Number;
        FirstName = extension.FirstName;
        LastName = extension.LastName;
        AgentQueueStatus = (QueuesStatusType)extension.QueueStatus;
    }
}