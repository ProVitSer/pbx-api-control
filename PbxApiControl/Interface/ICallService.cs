using PbxApiControl.DTOs.Calls;
using PbxApiControl.Models;


namespace PbxApiControl.Interface;
public interface ICallService
{
    MakeCall MakeCall(MakeCallDto dto);
    BaseResult HangupCall(HangupCallDto dto);

    BaseResult TransferCall(TransferCallDto dto);


}
