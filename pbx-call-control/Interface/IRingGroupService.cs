using PbxApiControl.Models.RingGroup;

namespace PbxApiControl.Interface
{
    public interface IRingGroupService
    {
    
        bool IsRingGroupExists(string ringGroupNumber);
        string[] GetRingGroupList();
        string[] GetRingGroupMembers(string ringGroupNumber);
    
        string[] AddRingGroupMembers(AddRingGroupMembersDataModel data);
    
        string[] DeleteRingGroupMembers(DeleteRingGroupMembersDataModel data);
    
    }

}

