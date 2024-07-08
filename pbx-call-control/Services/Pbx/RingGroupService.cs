using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.Models.RingGroup;
using PbxApiControl.Constants;

namespace PbxApiControl.Services.Pbx
{
    public class RingGroupService : IRingGroupService
    {
        private readonly ILogger<RingGroupService> _logger;

        public RingGroupService(ILogger<RingGroupService> logger)
        {
            _logger = logger;
        }

        public string[] GetRingGroupList()
        {
            using (var disposer = PhoneSystem.Root.GetAll<RingGroup>().GetDisposer())
            {
                return disposer.Select(x => x.Number).ToArray();
            }
        }

        public string[] GetRingGroupMembers(string ringGroupNumber)
        {
            var dnByNumber = PhoneSystem.Root.GetDNByNumber(ringGroupNumber);

            if (dnByNumber is not RingGroup ringGroup)
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotRingGroup);
            }

            using (var disposer = ringGroup.Members.GetDisposer<DN>())
            {
                return disposer.Select(x => x.Number).ToArray();
            }
        }

        public string[] AddRingGroupMembers(AddRingGroupMembersDataModel data)
        {
            var dnByNumber = PhoneSystem.Root.GetDNByNumber(data.RingGroupNumber);

            if (dnByNumber is not RingGroup ringGroup)
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotRingGroup);
            }

            var updatedMembers = GetRingGroupMembers(data.RingGroupNumber)
                .Union(data.Extensions)
                .ToArray();

            UpdateRingGroupData(ringGroup, updatedMembers);

            return GetRingGroupMembers(data.RingGroupNumber);
        }

        public string[] DeleteRingGroupMembers(DeleteRingGroupMembersDataModel data)
        {
            var dnByNumber = PhoneSystem.Root.GetDNByNumber(data.RingGroupNumber);

            if (dnByNumber is not RingGroup ringGroup)
            {
                throw new InvalidOperationException(ServiceConstants.DnIsNotRingGroup);
            }

            var updatedMembers = GetRingGroupMembers(data.RingGroupNumber)
                .Except(data.Extensions)
                .ToArray();

            UpdateRingGroupData(ringGroup, updatedMembers);

            return GetRingGroupMembers(data.RingGroupNumber);
        }

        private void UpdateRingGroupData(RingGroup ringGroup, string[] actualMembers)
        {
            ringGroup.Members = ParseMembers(actualMembers);
            ringGroup.Save();
        }

        public bool IsRingGroupExists(string ringGroupNumber)
        {
            var dnByNumber = PhoneSystem.Root.GetDNByNumber(ringGroupNumber);
            return dnByNumber is RingGroup;
        }

        private static DN[] ParseMembers(string[] memberNumbers)
        {
            return memberNumbers
                .Select(x => PhoneSystem.Root.GetDNByNumber(x) as DN)
                .Where(x => x is not null)
                .Distinct()
                .ToArray();
        }
    }
}