using PbxApiControl.Models.Call;
using TCX.Configuration;

namespace PbxApiControl.Interface
{
    public interface ILogUtilService
    {
        void LogCallsState(List<CallStateModel>  callsState);
        void LogUpdateExtInfo(Extension extension);
        void LogSetExtensionProperties(Extension extension);
    }
}

