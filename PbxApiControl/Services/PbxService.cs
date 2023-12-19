using PbxApiControl.DTOs;
using PbxApiControl.Interface;
using TCX.Configuration;

namespace PbxApiControl.Services;

#nullable enable
public class PbxService : IPbxService
{
    public PbxCountCallsDto PbxCountCalls()
    {
        var countCalls = PhoneSystem.Root.GetActiveConnectionsByCallID().Count;
        return new PbxCountCallsDto()
        {
            currentCountCalls = countCalls
        };
    }
}

