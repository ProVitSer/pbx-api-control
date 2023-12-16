using System;
using System.Collections.Generic;
using System.Linq;
using PbxApiControl.DTOs.Pbx;
using TCX.Configuration;

namespace PbxApiControl.Services;

#nullable enable

public interface IPbxService
{
    CallsDto PbxCountCalls();
    string PbxActiveCalls();

}

public class PbxService : IPbxService
{
    public CallsDto PbxCountCalls()
    {
        var countCalls = PhoneSystem.Root.GetActiveConnectionsByCallID().Count;
        Console.WriteLine(countCalls);
        return new CallsDto()
        {
            currentCountCalls = countCalls
        };
    }

    public string PbxActiveCalls()


    {



        // foreach (KeyValuePair<uint, List<ActiveConnection>> acList in PhoneSystem.Root.GetActiveConnectionsByCallID())
        // {

        //     // Console.WriteLine("acList.Key", acList.Key);

        //     // foreach (ActiveConnection ac in acList.Value)
        //     // {
        //     //     Console.WriteLine(ac);
        //     //     Console.WriteLine(ac.DN.Number);
        //     //     Console.WriteLine(ac.DN.GetType());
        //     //     Console.WriteLine(ac.ExternalParty);
        //     //     Console.WriteLine(ac.Status.ToString());
        //     //     Console.WriteLine(ac.DN);
        //     //     Console.WriteLine(ac["inbound_did_rule"]);

        //     //     Console.WriteLine("adad");

        //     // }

        // }
        return "";

    }
}
