using PbxApiControl.Models.Queue;
using PbxApiControl.Enums;

namespace PbxApiControl.Models.QueueReply
{
    public class QueueAgentsInfoReply
    {
        public static QueueInfo FormatQueueInfo(QueueAgentsDataModels data)
        {
            return new QueueInfo
            {
                Extension = data.Extension,
                FirstName = data.FirstName,
                LastName = data.LastName,
                AgentQueueStatus = ConvertAgentQueueStatus(data.AgentQueueStatus)
            };
        }
        
        private static AgentQueueStatus ConvertAgentQueueStatus(QueuesStatusType agentQueueStatus)
        {
            return agentQueueStatus switch
            {
                QueuesStatusType.LoggedOut => AgentQueueStatus.LoggedOut,
                QueuesStatusType.LoggedIn => AgentQueueStatus.LoggedIn,
                _ => throw new ArgumentOutOfRangeException(nameof(agentQueueStatus), $"Unknown agent status: {agentQueueStatus}")
            };
        }
    }  
}

