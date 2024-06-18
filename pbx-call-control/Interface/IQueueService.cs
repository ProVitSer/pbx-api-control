using PbxApiControl.Models.Queue;

namespace PbxApiControl.Interface;
public interface IQueueService
{
    bool IsQueueExists(string queueNumber);
    string[] QueueList();
    QueueAgentsDataModels[] QueueAgents(string queueNumber);
    QueueAgentsDataModels[] FreeQueueAgents(string queueNumber);
    QueueAgentsDataModels[] BusyQueueAgents(string queueNumber);
    QueueDataModel AddQueueAgents(string queueNumber, string[] agentsNumbers);
    QueueDataModel DeleteQueueAgents(string queueNumber, string[] agentsNumbers);
}