using System;
using System.Collections.Generic;
using PbxApiControl.DTOs.Calls;
using PbxApiControl.Interface;
using PbxApiControl.Models;
using PbxApiControl.Constants;
using TCX.Configuration;


namespace PbxApiControl.Services;


#nullable enable
public class CallService : ICallService
{
    public MakeCall MakeCall(MakeCallDto dto)
    {
        try
        {

            if (!CheckExtension(dto.From)) return MakeCallReult(dto, false, string.Format("{0} {1}", CallConstants.ExtensionNotFound, dto.From));

            PhoneSystem.Root.MakeCall(dto.From, dto.To);

            return MakeCallReult(dto, true, CallConstants.CallInitSuccess);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

    }

    public BaseResult HangupCall(HangupCallDto dto)
    {
        if (!CheckExtension(dto.Extension)) return new BaseResult(false, string.Format("{0} {1}", CallConstants.ExtensionNotFound, dto.Extension));

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(dto.Extension))
        {
            using (IArrayDisposer<ActiveConnection> disposer = dnByNumber.GetActiveConnections().GetDisposer<ActiveConnection>())
            {
                bool connectionIsDrop = false;
                string message = string.Format("{0} {1}", CallConstants.ActiveCallDoesNotExists, dto.Extension);
                foreach (ActiveConnection activeConnection in (IEnumerable<ActiveConnection>)disposer)
                {
                    if (activeConnection.Status == ConnectionStatus.Connected)
                    {
                        activeConnection.Drop();
                        connectionIsDrop = true;
                        message = CallConstants.CallDropSuccess;
                    }
                }
                return new BaseResult(connectionIsDrop, message);
            };
        };

    }

    public BaseResult TransferCall(TransferCallDto dto)
    {

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(dto.Extension))
        {
            using (IArrayDisposer<ActiveConnection> disposer = dnByNumber.GetActiveConnections().GetDisposer<ActiveConnection>())
            {

                if (dto.DestinationNumber.Length > 5)
                {
                    return TransferToExternal(disposer, dto);
                }
                else
                {
                    return TransferToLocal(disposer, dto);

                }
            };
        };

    }

    private static BaseResult TransferToLocal(IArrayDisposer<ActiveConnection> disposer, TransferCallDto dto)
    {

        bool isCallTransfer = false;
        string message = string.Format("{0} {1}", CallConstants.ActiveCallDoesNotExists, dto.Extension);
        foreach (ActiveConnection activeConnection in (IEnumerable<ActiveConnection>)disposer)
        {
            if (activeConnection.Status == ConnectionStatus.Ringing)
            {
                if (CheckIsExtension(dto.DestinationNumber))
                {
                    if (IsExtensionRegister(dto.DestinationNumber))
                    {
                        TransferCall(dto.DestinationNumber, activeConnection);
                        isCallTransfer = true;
                        message = string.Format("{0}: {1}", CallConstants.TranferCall, dto.DestinationNumber);
                    }
                    else
                    {
                        isCallTransfer = false;
                        message = string.Format("{0}: {1}", CallConstants.ExtensionNotRegister, dto.DestinationNumber);
                    }
                }
                else
                {
                    TransferCall(dto.DestinationNumber, activeConnection);
                    isCallTransfer = true;
                    message = string.Format("{0}: {1}", CallConstants.TranferCall, dto.DestinationNumber);
                }
            }
        }
        return new BaseResult(isCallTransfer, message);
    }

    private static BaseResult TransferToExternal(IArrayDisposer<ActiveConnection> disposer, TransferCallDto dto)
    {
        bool isCallTransfer = false;
        string message = string.Format("{0} {1}", CallConstants.ActiveCallDoesNotExists, dto.Extension);
        foreach (ActiveConnection activeConnection in (IEnumerable<ActiveConnection>)disposer)
        {
            if (activeConnection.Status == ConnectionStatus.Ringing)
            {
                TransferCall(dto.DestinationNumber, activeConnection);
                isCallTransfer = true;
                message = string.Format("{0}: {1}", CallConstants.TranferCall, dto.DestinationNumber);
            }
        }
        return new BaseResult(isCallTransfer, message);
    }

    private static void TransferCall(string destinationNumber, ActiveConnection activeConnection)
    {
        activeConnection.Drop();
        activeConnection.ReplaceWith(destinationNumber);
    }

    private static bool IsExtensionRegister(string ext)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            Console.WriteLine(dnByNumber.IsRegistered);
            return dnByNumber.IsRegistered;
        };
    }
    private static bool CheckIsExtension(string ext)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            return dnByNumber is Extension extension;
        };

    }

    private static MakeCall MakeCallReult(MakeCallDto dto, bool reult, string message)
    {
        return new MakeCall()
        {
            From = dto.From,
            To = dto.To,
            Result = reult,
            Message = message
        };
    }


    private static bool CheckExtension(string num)
    {

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(num))
        {
            if (!(dnByNumber is Extension))
            {
                return false;
            }

            return true; ;
        };
    }
}