using System.ComponentModel.DataAnnotations;
using PbxApiControl.Enums;

namespace PbxApiControl.DTOs.Extension;

public class SetForwardStatusDto
{
    [Required]
    public string ExtensionNumber { get; init; }

    [Required]
    [EnumDataType(typeof(ForwardingRules))]
    public string Status { get; init; }
}