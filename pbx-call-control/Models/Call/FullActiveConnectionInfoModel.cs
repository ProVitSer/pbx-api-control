using TCX.Configuration;
using PbxApiControl.Services.Pbx;


namespace PbxApiControl.Models.Call;

public class FullActiveConnectionInfoModel
{
    public uint CallID { get; set;}
    
    public List<ActiveConnectionInfoModel> activeConnectionInfo { get; set; }
    
    internal FullActiveConnectionInfoModel(uint callid)
    {
        this.CallID = callid;
        this.activeConnectionInfo = new List<ActiveConnectionInfoModel>();

    }

    public void AddActiveConnectionInfo(ActiveConnection activeConnection)
    {
        this.activeConnectionInfo.Add(new ActiveConnectionInfoModel(activeConnection));

    }
}