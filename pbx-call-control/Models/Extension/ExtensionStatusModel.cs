using PbxApiControl.Enums;
using TCX.Configuration;

namespace PbxApiControl.Models.Extensions
{

    public class ExtensionStatus
    {
    public string Extension { get; }
    public string FirstName { get; }

    public string LastName { get; }

    public string Email { get; }
    public bool Registered { get; }
    public ForwardingRules ForwardingRulesStatus { get; }
    public QueuesStatusType QueuesStatus { get; }
    public string[] Groups { get; }
    public string[] LoggedInQueues { get; }
    public string[] InRingGroups { get; }
    public List<DevInfo>? Devices { get; set; }

    public ExtensionStatus(Extension ext, ExtensionDeviceInfo extDevInfo)
    {
        this.FirstName = ext.FirstName;
        this.LastName = ext.LastName;
        this.Email = ext.EmailAddress;
        this.Extension = ext.Number;
        this.Registered = ext.IsRegistered;
        this.ForwardingRulesStatus = GetForwardingRulesStatus(ext.CurrentProfile.Name);//ext.IsOverrideActiveNow ? ext.CurrentProfileOverride.Name : GetForwardingRulesStatus(ext.CurrentProfile.Name);
        this.QueuesStatus = (ext.QueueStatus is QueueStatusType.LoggedIn) ? QueuesStatusType.LoggedIn : QueuesStatusType.LoggedOut;
        this.Groups = ext.GroupMembership.Select(x => x.Group.Name).ToArray();
        this.InRingGroups = ext.GetRingGroups().Select(x => x.Number).ToArray();
        this.LoggedInQueues = GetQueuesByStatus(ext, QueueStatusType.LoggedIn);
        this.Devices = extDevInfo.Devices;

    }
    
    private static ForwardingRules GetForwardingRulesStatus(string status)
    {
        switch (status)
        {
            case "Out of office":
                return ForwardingRules.DND;
            case "Custom 1":
                return ForwardingRules.Lunch;
            case "Custom 2":
                return ForwardingRules.BusinessTrip;
            case "Away":
                return ForwardingRules.Away;
            case "Available":
                return ForwardingRules.Available;
            default:
                return ForwardingRules.Unknown;
        }
    }

    private static string[] GetQueuesByStatus(Extension ext, QueueStatusType queueStatusType){
        
        List<string> queues = new List<string>();

        foreach (QueueAgent queueAgent in ext.QueueMembership)
        {
            
            if (queueAgent.QueueStatus.ToString() == queueStatusType.ToString())
            {
                queues.Add(queueAgent.Queue.Number);
            }
            
        }

        return queues.ToArray();
    }
    }
}