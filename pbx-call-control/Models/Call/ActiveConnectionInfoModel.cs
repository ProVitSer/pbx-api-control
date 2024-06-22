using TCX.Configuration;
using PbxApiControl.Services.Pbx;


namespace PbxApiControl.Models.Call;

public class ActiveConnectionInfoModel
{
    public int ID  { get; set;}
    
    public int CallConnectionID  { get; set;}
    
    public string ExternalParty { get; set;}
    
    public string RecordingState { get; set;}
    
    public int PartyConnectionID  { get; set;}
    
    public int ReferredBy  { get; set;}
    
    public bool IsOutbound { get; set;}
    
    public bool IsInbound { get; set;}
    
    public string DialedNumber  { get; set;}
    
    public string InternalParty  { get; set;}
    
    
    public ActiveConnectionInfoModel(ActiveConnection activeConnection)
    {

        this.ID = activeConnection.ID;
        this.CallConnectionID = activeConnection.CallConnectionID;
        this.ExternalParty = activeConnection.ExternalParty ?? string.Empty;
        this.RecordingState = activeConnection.RecordingState.ToString() ?? string.Empty;
        this.PartyConnectionID = activeConnection.PartyConnectionID;
        this.ReferredBy = activeConnection.ReferredBy;
        this.IsOutbound = activeConnection.IsOutbound;
        this.IsInbound = activeConnection.IsInbound;
        this.DialedNumber = activeConnection.DialedNumber ?? string.Empty;
        this.InternalParty = activeConnection.InternalParty.ToString() ?? string.Empty;

        
    }
    
}

