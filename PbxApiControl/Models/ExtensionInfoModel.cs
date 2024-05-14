using PbxApiControl.Enums;
using TCX.Configuration;

namespace PbxApiControl.Models;

public class ExtensionInfo
{
    public string Extension { get; }

    public bool Registered { get; }

    public string ForwardingRulesStatus { get; }
    public string QueuesStatus { get; }

    public string[] Groups { get; }
    public string[] Queues { get; }

    public string[] RingGroups { get; }

    public ExtensionInfo(Extension ext)
    {
        this.Extension = ext.Number;
        this.Registered = ext.IsRegistered;
        this.ForwardingRulesStatus = ext.IsOverrideActiveNow ? ext.CurrentProfileOverride.Name : GetForwardingRulesStatus(ext.CurrentProfile.Name);
        this.QueuesStatus = (ext.QueueStatus is QueueStatusType.LoggedIn) ? QueuesStatusType.LoggedIn.ToString() : QueuesStatusType.LoggedOut.ToString();
        this.Groups = ext.GroupMembership.Select(x => x.Group.Name).ToArray();
        this.Queues = ext.QueueMembership.Select(x => x.Queue.Number).ToArray();
        this.RingGroups = ext.GetRingGroups().Select(x => x.Number).ToArray();

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
        };
    }
}