using PbxApiControl.Models.Call;

namespace PbxApiControl.Models.CallReply
{
    public class ActiveCallsInfoReply
    {
        public static GetActiveCallsInfoReply FormatActiveCallsInfo(List<CallStateModel>  callState)
        {
            var reply = new GetActiveCallsInfoReply();

            foreach (var call in callState)
            {
                var callInfoData = new GetActiveCallsInfoData
                {
                    CallId = (int)call.CallID,
                    CallDirection = call.CallDirection.ToString(),
                    Status = call.Status.ToString(),
                    Direction = call.Direction ?? string.Empty,
                    CallStatus = call.CallStatus ?? string.Empty,
                    LocalNumber = call.LocalNumber ?? string.Empty,
                    RemoteNumber = call.RemoteNumber ?? string.Empty
                };

                reply.ActiveCallsInfoData.Add(callInfoData);
            }

            return reply;
        }
    }
}

