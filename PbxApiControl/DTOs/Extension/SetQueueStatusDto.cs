using System.ComponentModel.DataAnnotations;
using PbxApiControl.Enums;

namespace PbxApiControl.DTOs.Extension;

public class SetQueuetatusDto
{
    [Required]
    public string ExtensionNumber { get; init; }

    [Required]
    public string QueueNumber { get; init; }

    [Required]
    [EnumDataType(typeof(QStatusType))]
    public string Status { get; init; }
}


