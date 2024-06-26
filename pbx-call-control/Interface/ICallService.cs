using PbxApiControl.Models.Call;

namespace PbxApiControl.Interface;

public interface  ICallService
{
    int CountCalls();
    List<CallStateModel>  ActiveCallsInfo();
    BaseCallResultModel MakeCall(string to, string from);
    BaseCallResultModel HangupCall(string extension);
    BaseCallResultModel TransferCallByCallId(uint callId, int partyConnectionId, string destinationNumber);
    List<FullActiveConnectionInfoModel> FullActiveConnectionsInfo();
}
