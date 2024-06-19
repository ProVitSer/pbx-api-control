using System.Diagnostics;
using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.Models.Queue;
using PbxApiControl.Enums;

namespace PbxApiControl.Services.Pbx;

public class QueueService : IQueueService
{
    private readonly ILogger<QueueService> _logger;
    
    public QueueService(ILogger<QueueService> logger)
    {
        _logger = logger;
    }
    
    
    public string[] QueueList()
    {
        using (IArrayDisposer<Queue> disposer = PhoneSystem.Root.GetAll<Queue>().GetDisposer())
        {
            var queueNumbers = disposer.Select(x => x.Number).ToArray();

            return queueNumbers;
        };
    }

    public QueueAgentsDataModels[] QueueAgents(string queueNumber)
    {
        
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(queueNumber))
        {
            if (!(dnByNumber is Queue))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotQueue);
            }
            
            var queue = (Queue)dnByNumber;

            
            QueueAgentsDataModels[] queueAgents = queue.QueueAgents
                .Select(agent =>
                {
                    Extension extension = agent.DN as Extension;
                    
                    return new QueueAgentsDataModels(extension);
                })
                .ToArray();

            return queueAgents;

        };
    }

    public QueueAgentsDataModels[] FreeQueueAgents(string queueNumber)
    {
        return GetQueueAgentsByStatus(queueNumber, ActiveConnectionsStatus.Idle);
    }
    
    public QueueAgentsDataModels[] BusyQueueAgents(string queueNumber)
    {
        return GetQueueAgentsByStatus(queueNumber, ActiveConnectionsStatus.Busy);
    }

    public QueueDataModel AddQueueAgents(string queueNumber, string[] agentsNumbers)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(queueNumber))
        {
            if (!(dnByNumber is Queue))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotQueue);
            }
            
            var queue = (Queue)dnByNumber;

            var qAgents = QueueAgents(queueNumber);

            var updateAgents = agentsNumbers.Union(qAgents.Select(qa => qa.Extension).ToArray()).ToArray();

            QueueAgent[] agents = updateAgents.Select(x => PhoneSystem.Root.GetDNByNumber(x) as Extension)
                .Where(x => x != null)
                .Distinct()
                .Select(x => queue.CreateAgent(x))
                .ToArray();

            queue.QueueAgents = agents;
            queue.Save();

            return new QueueDataModel(queueNumber, updateAgents);
        };
    }

    public QueueDataModel DeleteQueueAgents(string queueNumber, string[] agentsNumbers)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(queueNumber))
        {
            if (!(dnByNumber is Queue))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotQueue);
            }
            
            var queue = (Queue)dnByNumber;

            var qAgents = QueueAgents(queueNumber);

            var updateAgents = agentsNumbers.Except(qAgents.Select(qa => qa.Extension).ToArray()).ToArray();

            QueueAgent[] agents = updateAgents.Select(x => PhoneSystem.Root.GetDNByNumber(x) as Extension)
                .Where(x => x != null)
                .Distinct()
                .Select(x => queue.CreateAgent(x))
                .ToArray();

            queue.QueueAgents = agents;
            queue.Save();

            return new QueueDataModel(queueNumber, updateAgents);
        };
    }

    public bool IsQueueExists(string queueNumber)
    {

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(queueNumber))
        {
            return dnByNumber is Queue;
        };
    }
    
    private QueueAgentsDataModels[] GetQueueAgentsByStatus(string queueNumber, ActiveConnectionsStatus status)
    {

        QueueAgentsDataModels[] queueAgents = QueueAgents(queueNumber);
        
        if (queueAgents.Length == 0) return queueAgents;
        
        
        List<QueueAgentsDataModels> queueAgentsList = new List<QueueAgentsDataModels>();
        
        foreach (QueueAgentsDataModels queueA in queueAgents)
        {
            if (queueA.AgentQueueStatus == QueuesStatusType.LoggedIn && GetExtensionStatus(queueA.Extension) == status)
            {
                queueAgentsList.Add(queueA);
            }
            
            
        }
        return queueAgentsList.ToArray();

    }
    
    private ActiveConnectionsStatus GetExtensionStatus(string ext)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            if (dnByNumber == null || !(dnByNumber is Extension)) return ActiveConnectionsStatus.DoesNotExists;

            using (IArrayDisposer<ActiveConnection> disposer = dnByNumber.GetActiveConnections().GetDisposer())
            {
                return disposer.Length == 0 ? ActiveConnectionsStatus.Idle : ActiveConnectionsStatus.Busy;
            };
        }

    }

}