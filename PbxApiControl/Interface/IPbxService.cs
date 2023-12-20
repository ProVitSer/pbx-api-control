using System.Collections.Generic;
using PbxApiControl.DTOs.Pbx;
using TCX.Configuration;


namespace PbxApiControl.Interface;
public interface IPbxService
{
    PbxCountCallsDto PbxCountCalls();
    IEnumerable<string> GetPbxNumbers<T>() where T : class, DN;

}
