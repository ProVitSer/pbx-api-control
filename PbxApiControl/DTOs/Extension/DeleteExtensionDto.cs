using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.DTOs.Extension;

public class DeleteExtensionDto
{
    [Required]
    public string ExtensionNumber { get; init; }
}