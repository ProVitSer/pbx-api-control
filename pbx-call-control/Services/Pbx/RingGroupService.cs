
using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.Models.RingGroup;


namespace PbxApiControl.Services.Pbx;
public class RingGroupService : IRingGroupService
{
    private readonly ILogger<ExtensionService> _logger;
    
    public RingGroupService(ILogger<ExtensionService> logger)
    {
        _logger = logger;
    }
    
    public string[] GetRingGroupList()
    {
        using (IArrayDisposer<RingGroup> disposer = PhoneSystem.Root.GetAll<RingGroup>().GetDisposer())
        {
            var ringGroupNumbers = disposer.Select(x => x.Number).ToArray();

            return ringGroupNumbers;
        };
    }
    
    public string[] GetRingGroupMembers(string ringGroupNumber)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ringGroupNumber))
        {
            
            if (!(dnByNumber is RingGroup))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotRingGroup);
            }
            
            var ringGroup = (RingGroup)dnByNumber;

            using (IArrayDisposer<DN> disposer = ringGroup.Members.GetDisposer<DN>())
            {
                return disposer.Select(x => x.Number).ToArray();
            };
        };
    }
    
    
    public string[] AddRingGroupMembers(AddRingGroupMembersDataModel data)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(data.RingGroupNumber))
        {
            
            if (!(dnByNumber is RingGroup))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotRingGroup);
            }
            
            UpdateRingGroupData((RingGroup)dnByNumber,
                GetRingGroupMembers(data.RingGroupNumber).Union(data.Extensions).ToArray());
            
            return GetRingGroupMembers(data.RingGroupNumber);

        };
    }
    
    public string[] DeleteRingGroupMembers(DeleteRingGroupMembersDataModel data)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(data.RingGroupNumber))
        {
            
            if (!(dnByNumber is RingGroup))
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotRingGroup);
            }

            UpdateRingGroupData((RingGroup)dnByNumber,
                GetRingGroupMembers(data.RingGroupNumber).Except(data.Extensions).ToArray());
            
            return GetRingGroupMembers(data.RingGroupNumber);

        };
    }

    private void UpdateRingGroupData(RingGroup ringGroup, string[] actualMembers)
    {
        ringGroup.Members = ParseMembers(actualMembers);
            
        ringGroup.Save();
    }


    public bool IsRingGroupExists(string ringGroupNumber)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ringGroupNumber))
        {
     
            return dnByNumber is RingGroup;
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