using PbxApiControl.Enums;

namespace PbxApiControl.Models;
public class CallState
{
    public uint CallID { get; }
    internal Direction CallDirection { get; set; }
    internal CallStatus Status { get; set; }

    public string Direction => this.CallDirection.ToString();

    public string CallStatus => this.Status.ToString();

    public string LocalNumber { get; set; }
    public string RemoteNumber { get; set; }
    internal CallState(uint callid) => this.CallID = callid;
}

