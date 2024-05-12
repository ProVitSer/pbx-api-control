using System.ComponentModel.DataAnnotations;
using PbxApiControl.Enums;

namespace PbxApiControl.DTOs.Extension;

public class SetQueueStatusDto
{
    [Required]
    public string ExtensionNumber { get; init; }

    [Required]
    [EnumDataType(typeof(QueuesStatusType))]
    public QueuesStatusType Status { get; init; }
    
    public SetQueueStatusDto(string extensionNumber, QueuesStatusType status)
    {
        ExtensionNumber = extensionNumber;
        Status = status;
    }
}