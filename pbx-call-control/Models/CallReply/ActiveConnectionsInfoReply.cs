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

                foreach (var ci in aci.ActiveConnectionInfo)
                {
                    connectionsDataList .Add(new ConnectionsData
                    {
                        Id = ci.Id,
                        CallConnectionId = ci.CallConnectionId,
                        ExternalParty = ci.ExternalParty,
                        RecordingState = ci.RecordingState,
                        PartyConnectionId = ci.PartyConnectionId,
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
                    CallId = (int)aci.CallId,
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
