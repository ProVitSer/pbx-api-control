
using System.Collections.Generic;
using PbxApiControl.Models;
using PbxApiControl.DTOs.Queue;


namespace PbxApiControl.Interface;

#nullable enable

public interface IQueueService
{
    IEnumerable<string>? GetQueueList();
    QueueAgents[] GetQueueAgents(string queueNumber);
    QueueAgents[] GetFreeQueueAgents(string queueNumber);
    QueueAgents[] GetBusyQueueAgents(string queueNumber);
    QueueAgents[] AddRingGroupMembers(AddQueueAgentsDto dto);
    QueueAgents[] DeleteRingGroupMembers(DeleteQueueAgentsDto dto);
}
