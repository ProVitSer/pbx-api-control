
namespace PbxApiControl.Models.RingGroup
{
    public class AddRingGroupMembersDataModel
    {
        public string RingGroupNumber { get;  set; }
        public List<string> Extensions { get; set;  }

        
        public AddRingGroupMembersDataModel(AddMemberInRingGroupRequest request)
        {
            RingGroupNumber = request.RingGroupNumber;
            Extensions = new List<string>(request.Numbers);

        }
    }
}

