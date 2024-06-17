namespace PbxApiControl.Models.RingGroup;

public class DeleteRingGroupMembersDataModel
{
    public string RingGroupNumber { get;  set; }
    public List<string> Extensions { get; set;  }

        
    public DeleteRingGroupMembersDataModel(DeleteMemberInRingGroupRequest request)
    {
        RingGroupNumber = request.RingGroupNumber;
        Extensions = new List<string>(request.Numbers);

    }
}