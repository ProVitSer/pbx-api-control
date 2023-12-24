using System.Collections.Generic;
using System.Linq;
using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.DTOs.RingGroup;

namespace PbxApiControl.Services;

#nullable enable
public class RingGroupService : IRingGroupService
{

    private readonly IPbxService _pbxService;

    public RingGroupService(IPbxService pbxService)
    {
        _pbxService = pbxService;
    }


    public IEnumerable<string>? GetRingGroupList()
    {
        return _pbxService.GetPbxNumbers<RingGroup>();
    }

    public IEnumerable<string>? GetRingMembers(string ringGroupNumber)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ringGroupNumber))
        {
            if (!(dnByNumber is RingGroup ringGroup))
                return null;
            using (IArrayDisposer<DN> disposer = ringGroup.Members.GetDisposer<DN>())
            {
                return disposer.Select(x => x.Number).ToArray();
            };
        };
    }

    public IEnumerable<string>? AddRingGroupMembers(AddRingGroupMembersDto members)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(members.RingGroupNumber))
        {
            if (!(dnByNumber is RingGroup ringGroup))
                return null;

            var actualMembers = GetRingMembers(members.RingGroupNumber);
            if (actualMembers == null)
            {
                ringGroup.Members = ParseMembers(members.Members);
            }
            else
            {
                ringGroup.Members = ParseMembers(actualMembers.Union(members.Members).ToArray());

            }

            ringGroup.Save();
            return GetRingMembers(members.RingGroupNumber);
        };
    }


    public IEnumerable<string>? DeleteRingGroupMembers(DeleteRingGroupMembersDto members)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(members.RingGroupNumber))
        {
            if (!(dnByNumber is RingGroup ringGroup))
                return null;

            var actualMembers = GetRingMembers(members.RingGroupNumber);
            if (actualMembers == null)
            {
                return null;
            }
            else
            {
                ringGroup.Members = ParseMembers(actualMembers.Except(members.Members).ToArray());
            }

            ringGroup.Save();
            return GetRingMembers(members.RingGroupNumber);
        };
    }


    private static DN[] ParseMembers(string[] memberNumbers)
    {
        DN[] dn = memberNumbers
            .Select(x => PhoneSystem.Root.GetDNByNumber(x) as Extension)
            .Where(x => x != null)
            .Distinct()
            .ToArray();

        return dn;
    }


}