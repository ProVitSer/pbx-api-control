using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.DTOs.RingGroup;

public class AddRingGroupMembersDto
{
    [Required]
    public string RingGroupNumber { get; init; }

    [Required]
    public string[] Members { get; init; }
}