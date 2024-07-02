using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.Models.Queue;
using PbxApiControl.Enums;

namespace PbxApiControl.Services.Pbx
{
    public class QueueService : IQueueService
    {
        private readonly ILogger<QueueService> _logger;

        public QueueService(ILogger<QueueService> logger)
        {
            _logger = logger;
        }

        public string[] QueueList()
        {
            using (var disposer = PhoneSystem.Root.GetAll<Queue>().GetDisposer())
            {
                return disposer.Select(x => x.Number).ToArray();
            }
        }

        public QueueAgentsDataModels[] QueueAgents(string queueNumber)
        {
            var dnByNumber = PhoneSystem.Root.GetDNByNumber(queueNumber);

            if (dnByNumber is not Queue queue)
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotQueue);
            }

            return queue.QueueAgents
                .Select(agent => new QueueAgentsDataModels(agent.DN as Extension))
                .ToArray();
        }

        public QueueAgentsDataModels[] FreeQueueAgents(string queueNumber)
        {
            return GetQueueAgentsByStatus(queueNumber, ActiveConnectionsStatus.Idle);
        }

        public QueueAgentsDataModels[] BusyQueueAgents(string queueNumber)
        {
            return GetQueueAgentsByStatus(queueNumber, ActiveConnectionsStatus.Busy);
        }

        public QueueDataModel AddQueueAgents(string queueNumber, string[] agentNumbers)
        {
            var dnByNumber = PhoneSystem.Root.GetDNByNumber(queueNumber);

            if (dnByNumber is not Queue queue)
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotQueue);
            }

            var currentAgents = QueueAgents(queueNumber);
            var updatedAgents = agentNumbers.Union(currentAgents.Select(qa => qa.Extension)).ToArray();

            UpdateQueueAgents(queue, updatedAgents);

            return new QueueDataModel(queueNumber, updatedAgents);
        }

        public QueueDataModel RemoveQueueAgents(string queueNumber, string[] agentNumbers)
        {
            var dnByNumber = PhoneSystem.Root.GetDNByNumber(queueNumber);

            if (dnByNumber is not Queue queue)
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotQueue);
            }

            var currentAgents = QueueAgents(queueNumber);
            var updatedAgents = currentAgents
                .Where(qa => !agentNumbers.Contains(qa.Extension))
                .Select(x => x.Extension)
                .ToArray();

            UpdateQueueAgents(queue, updatedAgents);

            return new QueueDataModel(queueNumber, updatedAgents);
        }

        private void UpdateQueueAgents(Queue queue, string[] agents)
        {
            var queueAgents = agents
                .Select(x => PhoneSystem.Root.GetDNByNumber(x) as Extension)
                .Where(x => x != null)
                .Distinct()
                .Select(x => queue.CreateAgent(x))
                .ToArray();

            queue.QueueAgents = queueAgents;
            queue.Save();
        }

        public bool IsQueueExists(string queueNumber)
        {
            var dnByNumber = PhoneSystem.Root.GetDNByNumber(queueNumber);
            return dnByNumber is Queue;
        }

        private QueueAgentsDataModels[] GetQueueAgentsByStatus(string queueNumber, ActiveConnectionsStatus status)
        {
            var queueAgents = QueueAgents(queueNumber);
            foreach (var call in queueAgents)
            {
                _logger.LogInformation("Extension: {Extension}, FirstName: {FirstName},  AgentQueueStatus: {AgentQueueStatus}",
                    call.Extension,
                    call.FirstName,
                    call.AgentQueueStatus);
            }
            return queueAgents
                .Where(qa => qa.AgentQueueStatus == QueuesStatusType.LoggedIn && GetExtensionStatus(qa.Extension) == status)
                .ToArray();
        }

        private ActiveConnectionsStatus GetExtensionStatus(string extensionNumber)
        {
            var dnByNumber = PhoneSystem.Root.GetDNByNumber(extensionNumber);

            if (dnByNumber is not Extension)
            {
                return ActiveConnectionsStatus.DoesNotExists;
            }

            using (var disposer = dnByNumber.GetActiveConnections().GetDisposer())
            {
                return disposer.Length == 0 ? ActiveConnectionsStatus.Idle : ActiveConnectionsStatus.Busy;
            }
        }
    }
}