using PbxApiControl.Models.Call;
using Google.Protobuf.Collections;

namespace PbxApiControl.Models.CallReply
{
    public class ActiveConnectionsInfoReply
    {
        public static GetActiveConnectionsInfoReply FormatConnectionsInfoInfo(List<FullActiveConnectionInfoModel>  activeConnectionsInfo)
        {
            var reply = new GetActiveConnectionsInfoReply();

            foreach (var aci in activeConnectionsInfo)
            {
                List<ConnectionsData> connectionsDataList  = new List<ConnectionsData>();

                foreach (var ci in aci.activeConnectionInfo)
                {
                    connectionsDataList .Add(new ConnectionsData
                    {
                        Id = ci.ID,
                        CallConnectionId = ci.CallConnectionID,
                        ExternalParty = ci.ExternalParty,
                        RecordingState = ci.RecordingState,
                        PartyConnectionId = ci.PartyConnectionID,
                        ReferredBy = ci.ReferredBy,
                        IsOutbound = ci.IsOutbound,
                        IsInbound = ci.IsInbound,
                        DialedNumber = ci.DialedNumber,
                        InternalParty = ci.InternalParty,

                    });
                }
                
                var connectionsData = new RepeatedField<ConnectionsData>();
                
                connectionsData.AddRange(connectionsDataList);

                var info = new ActiveConnectionsInfo
                {
                    CallId = (int)aci.CallID,
                    ConnectionsData =
                    {
                        connectionsData
                    }
                };

                reply.ActiveConnectionsInfo.Add(info);


            }

            return reply;
        }
    }
    
}
