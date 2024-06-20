﻿using PbxApiControl.Models.Call;

namespace PbxApiControl.Interface;

public interface  ICallService
{
    int CountCalls();
    BaseCallResultModel MakeCall(string to, string from);
    BaseCallResultModel HangupCall(string extension);
    BaseCallResultModel TransferCall(string extension, string destinationNumber);
}
