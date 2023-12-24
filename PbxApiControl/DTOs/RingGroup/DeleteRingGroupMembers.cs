using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.DTOs.RingGroup;

public class DeleteRingGroupMembersDto
{
    [Required]
    public string RingGroupNumber { get; init; }

    [Required]
    public string[] Members { get; init; }
}