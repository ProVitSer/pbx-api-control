
using System.Collections.Generic;
using PbxApiControl.Models;
using PbxApiControl.DTOs.RingGroup;

namespace PbxApiControl.Interface;

#nullable enable

public interface IRingGroupService
{
    IEnumerable<string>? GetRingGroupList();
    IEnumerable<string>? GetRingMembers(string ringGroupNumber);

    IEnumerable<string>? AddRingGroupMembers(AddRingGroupMembersDto members);

    IEnumerable<string>? DeleteRingGroupMembers(DeleteRingGroupMembersDto members);

}
