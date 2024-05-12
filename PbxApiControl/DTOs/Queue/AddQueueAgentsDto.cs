using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.DTOs.Queue;

public class AddQueueAgentsDto
{
    [Required]
    public string QueueNumber { get; init; }

    [Required]
    public string[] Agents { get; init; }
    
    public AddQueueAgentsDto(string queueNumber, string[] agents)
    {
        QueueNumber = queueNumber;
        Agents = agents;
    }
}