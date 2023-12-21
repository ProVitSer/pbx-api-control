using System.ComponentModel.DataAnnotations;
using PbxApiControl.Enums;

namespace PbxApiControl.DTOs.Extension;

public class SetQueuestatusDto
{
    [Required]
    public string ExtensionNumber { get; init; }

    [Required]
    [EnumDataType(typeof(QueuesStatusType))]

    public string Status { get; init; }
}