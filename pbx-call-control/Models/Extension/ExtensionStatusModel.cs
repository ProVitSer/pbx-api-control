using PbxApiControl.Enums;
using TCX.Configuration;

namespace PbxApiControl.Models.Extensions
{
    public class ExtensionStatus
{
    public string Extension { get; }

    public bool Registered { get; }

    public string ForwardingRulesStatus { get; }
    public string QueuesStatus { get; }

    public string[] Groups { get; }
    public string[] AllQueues { get; }
    public string[] LoggedOutQueues { get; }
    
    public string[] LoggedInQueues { get; }

    public string[] RingGroups { get; }

    public ExtensionStatus(Extension ext)
    {
        this.Extension = ext.Number;
        this.Registered = ext.IsRegistered;
        this.ForwardingRulesStatus = ext.IsOverrideActiveNow ? ext.CurrentProfileOverride.Name : GetForwardingRulesStatus(ext.CurrentProfile.Name);
        this.QueuesStatus = (ext.QueueStatus is QueueStatusType.LoggedIn) ? QueuesStatusType.LoggedIn.ToString() : QueuesStatusType.LoggedOut.ToString();
        this.Groups = ext.GroupMembership.Select(x => x.Group.Name).ToArray();
        this.RingGroups = ext.GetRingGroups().Select(x => x.Number).ToArray();
        this.AllQueues = ext.QueueMembership.Select(x => x.Queue.Number).ToArray();
        this.LoggedOutQueues = GetQueuesByStatus(ext, QueueStatusType.LoggedOut);
        this.LoggedInQueues = GetQueuesByStatus(ext, QueueStatusType.LoggedIn);

    }

    
    
    private static string GetForwardingRulesStatus(string status)
    {
        switch (status)
        {
            case "Out of office":
                return ForwardingRules.DND.ToString();
            case "Custom 1":
                return ForwardingRules.Lunch.ToString();
            case "Custom 2":
                return ForwardingRules.BusinessTrip.ToString();
            default:
                return status;
        }
    }

    private static string[] GetQueuesByStatus(Extension ext, QueueStatusType queueStatusType){
        
        List<string> queues = new List<string>();

        foreach (QueueAgent queueAgent in ext.QueueMembership)
        {

            if (queueAgent.QueueStatus == queueStatusType)
            {
                queues.Add(queueAgent.Queue.Number);
            }
            
        }

        return queues.ToArray();
    }
}
}