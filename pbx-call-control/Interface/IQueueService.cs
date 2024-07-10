using PbxApiControl.Models.Queue;

namespace PbxApiControl.Interface
{
    public interface IQueueService
    {
        bool IsQueueExists(string queueNumber);
        QueueInfoModel[] QueueList();
        QueueAgentsDataModels[] QueueAgents(string queueNumber);
        QueueAgentsDataModels[] FreeQueueAgents(string queueNumber);
        QueueAgentsDataModels[] BusyQueueAgents(string queueNumber);
        QueueDataModel AddQueueAgents(string queueNumber, string[] agentsNumbers);
        QueueDataModel RemoveQueueAgents(string queueNumber, string[] agentsNumbers);
    }
}
