using PbxApiControl.DTOs;
using PbxApiControl.Interface;
using TCX.Configuration;

namespace PbxApiControl.Services;

#nullable enable
public class PbxService : IPbxService
{
    public CallsDto PbxCountCalls()
    {
        var countCalls = PhoneSystem.Root.GetActiveConnectionsByCallID().Count;
        return new CallsDto()
        {
            currentCountCalls = countCalls
        };
    }
}

