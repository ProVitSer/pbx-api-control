using TCX.Configuration;

namespace PbxApiControl.Models.Call
{
    public class FullActiveConnectionInfoModel
    {
        public uint CallId  { get; }
    
        public List<ActiveConnectionInfoModel> ActiveConnectionInfo  { get; }
    
        internal FullActiveConnectionInfoModel(uint callid)
        {
            this.CallId  = callid;
            this.ActiveConnectionInfo  = new List<ActiveConnectionInfoModel>();

        }

        public void AddActiveConnectionInfo(ActiveConnection activeConnection)
        {
            this.ActiveConnectionInfo .Add(new ActiveConnectionInfoModel(activeConnection));

        }
    }
}

