using System.Collections.Generic;
using System.Linq;
using PbxApiControl.DTOs.Pbx;
using PbxApiControl.Enums;
using PbxApiControl.Interface;
using TCX.Configuration;

namespace PbxApiControl.Services;

#nullable enable
public class PbxService : IPbxService
{

    public IEnumerable<string> Numbers { get; private set; }

    public PbxService()
    {
        Numbers = new List<string>();
    }

    public PbxCountCallsDto PbxCountCalls()
    {
        var countCalls = PhoneSystem.Root.GetActiveConnectionsByCallID().Count;
        return new PbxCountCallsDto()
        {
            currentCountCalls = countCalls
        };
    }
    public IEnumerable<string> GetPbxNumbers<T>() where T : class, DN
    {

        using (IArrayDisposer<T> disposer = PhoneSystem.Root.GetAll<T>().GetDisposer())
        {
            this.Numbers = disposer.Select(x => x.Number).ToArray();

            return this.Numbers;
        };
    }

    public ExtStatus GetExtensionStatus(string ext)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            if (dnByNumber == null || !(dnByNumber is Extension)) return ExtStatus.DoesNotExists;

            using (IArrayDisposer<ActiveConnection> disposer = dnByNumber.GetActiveConnections().GetDisposer())
            {
                return disposer.Length == 0 ? ExtStatus.Idle : ExtStatus.Busy;
            };

        };
    }

}

