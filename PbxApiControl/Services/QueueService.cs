using System.Collections.Generic;
using System.Linq;
using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.Models;
using PbxApiControl.Enums;
using PbxApiControl.DTOs.Queue;
using System;



namespace PbxApiControl.Services;

#nullable enable
public class QueueService : IQueueService
{
    private readonly IPbxService _pbxService;

    public QueueService(IPbxService pbxService)
    {
        _pbxService = pbxService;
    }
    public IEnumerable<string>? GetQueueList()
    {
        return _pbxService.GetPbxNumbers<Queue>();

    }

    public QueueAgents[] GetQueueAgents(string queueNumber)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(queueNumber))
        {
            if (!(dnByNumber is Queue queue)) return Array.Empty<QueueAgents>();

            QueueAgents[] queueAgents = queue.QueueAgents
                .Select(agent =>
                {
                    Extension? extension = agent.DN as Extension;
                    return new QueueAgents
                    {
                        Extension = extension?.Number,
                        FirstName = extension?.FirstName,
                        LastName = extension?.LastName,
                        LoggedIn = agent.QueueStatus == QueueStatusType.LoggedIn
                    };
                })
                .ToArray();

            return queueAgents;

        };
    }

    public QueueAgents[] GetBusyQueueAgents(string queueNumber)
    {
        return GetQueueAgentsByStatus(queueNumber, ExtStatus.Busy);
    }

    public QueueAgents[] GetFreeQueueAgents(string queueNumber)
    {
        return GetQueueAgentsByStatus(queueNumber, ExtStatus.Idle);

    }

    private QueueAgents[] GetQueueAgentsByStatus(string queueNumber, ExtStatus status)
    {
        QueueAgents[] queueAgents = GetQueueAgents(queueNumber);
        if (queueAgents.Length == 0) return queueAgents;


        List<QueueAgents> queueAgentsList = new List<QueueAgents>();
        foreach (QueueAgents queueA in queueAgents)
        {
            if (queueA.LoggedIn && _pbxService.GetExtensionStatus(queueA.Extension) == status)
                queueAgentsList.Add(queueA);
        }
        return queueAgentsList.ToArray();

    }

    public QueueAgents[] AddRingGroupMembers(AddQueueAgentsDto dto)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(dto.QueueNumber))
        {
            if (!(dnByNumber is Queue queue)) return Array.Empty<QueueAgents>();

            var qAgents = GetQueueAgents(dto.QueueNumber);

            var updateAgents = dto.Agents.Union(qAgents.Select(qa => qa.Extension).ToArray()).ToArray();

            QueueAgent[] agents = updateAgents.Select(x => PhoneSystem.Root.GetDNByNumber(x) as Extension)
                        .Where(x => x != null)
                        .Distinct()
                        .Select(x => queue.CreateAgent(x))
                        .ToArray();

            queue.QueueAgents = agents;
            queue.Save();

            return GetQueueAgents(dto.QueueNumber);
        };

    }
    public QueueAgents[] DeleteRingGroupMembers(DeleteQueueAgentsDto dto)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(dto.QueueNumber))
        {
            if (!(dnByNumber is Queue queue)) return Array.Empty<QueueAgents>();

            var qAgents = GetQueueAgents(dto.QueueNumber);

            var updateAgents = dto.Agents.Except(qAgents.Select(qa => qa.Extension).ToArray()).ToArray();

            QueueAgent[] agents = updateAgents.Select(x => PhoneSystem.Root.GetDNByNumber(x) as Extension)
                        .Where(x => x != null)
                        .Distinct()
                        .Select(x => queue.CreateAgent(x))
                        .ToArray();

            queue.QueueAgents = agents;
            queue.Save();

            return GetQueueAgents(dto.QueueNumber);
        };
    }

}