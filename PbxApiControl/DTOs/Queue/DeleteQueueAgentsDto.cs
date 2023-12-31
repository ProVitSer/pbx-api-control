using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.DTOs.Queue;

public class DeleteQueueAgentsDto
{
    [Required]
    public string QueueNumber { get; init; }

    [Required]
    public string[] Agents { get; init; }
}