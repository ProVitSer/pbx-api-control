using PbxApiControl.Models;
using System.Collections.Generic;


namespace PbxApiControl.Interface;
public interface IGetActiveConnectionService
{
    int CountCalls { get; }
    List<CallState> Calls { get; }
}
