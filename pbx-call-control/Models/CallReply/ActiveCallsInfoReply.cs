using PbxApiControl.Models.Call;
using PbxApiControl.Enums;

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
                    CallDirection = ConvertCallDirection(call.CallDirection),
                    Status = ConvertCallStatus(call.Status),
                    Direction = call.Direction ?? string.Empty,
                    CallStatus = call.CallStatus ?? string.Empty,
                    LocalNumber = call.LocalNumber ?? string.Empty,
                    RemoteNumber = call.RemoteNumber ?? string.Empty
                };

                reply.ActiveCallsInfoData.Add(callInfoData);
            }

            return reply;
        }
        private static ActiveCallsStatus ConvertCallStatus(CallStatus сallStatus)
        {
            return сallStatus switch
            {
                CallStatus.Talking => ActiveCallsStatus.Talking,
                CallStatus.Other => ActiveCallsStatus.Other,
                CallStatus.Dialing => ActiveCallsStatus.Dialing,
                CallStatus.Ringing => ActiveCallsStatus.Ringing,
                CallStatus.Finish => ActiveCallsStatus.Finish,
                _ => throw new ArgumentOutOfRangeException(nameof(сallStatus), $"Неизвестный статус вызова: {сallStatus}")
            };
        }
        
        private static CallDirection ConvertCallDirection(Direction callDirection)
        {
            return callDirection switch
            {
                Direction.Local => CallDirection.Local,
                Direction.Inbound => CallDirection.Inbound,
                Direction.Outbound => CallDirection.Outbound,
                _ => throw new ArgumentOutOfRangeException(nameof(callDirection), $"Неизвестный тип вызова: {callDirection}")
            };
        }
    }
}

