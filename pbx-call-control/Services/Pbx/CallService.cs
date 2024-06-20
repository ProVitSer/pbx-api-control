using PbxApiControl.Interface;
using PbxApiControl.Models.Call;
using PbxApiControl.Enums;
using TCX.Configuration;

namespace PbxApiControl.Services.Pbx;

public class CallService : ICallService
{
    
    private readonly ILogger<ContactService> _logger;
    private readonly IExtensionService _extensionService;

    public CallService(ILogger<ContactService> logger, IExtensionService extensionService)
    {
        _logger = logger;
        _extensionService = extensionService;

    }

    public int CountCalls()
    {
        return PhoneSystem.Root.GetActiveConnectionsByCallID().Count;
    }

    public BaseCallResultModel MakeCall(string to, string from)
    {
        try
        {
            using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(from))
            {

                if (!(dnByNumber is Extension))
                {
                    throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
                }
            
                PhoneSystem.Root.MakeCall(from, to);
            
                return new BaseCallResultModel( true, ServiceConstants.CallInitSuccess);
            }

        }
        catch (Exception e)
        {
            _logger.LogDebug(e.ToString());
            
            return new BaseCallResultModel( false, ServiceConstants.CallInitError);

        }
    }

    public BaseCallResultModel HangupCall(string extension)
    {
        try
        {
            using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(extension))
            {
                if (!(dnByNumber is Extension))
                {
                    throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
                }
                
                using (IArrayDisposer<ActiveConnection> disposer = dnByNumber.GetActiveConnections().GetDisposer<ActiveConnection>())
                {

                    foreach (ActiveConnection activeConnection in (IEnumerable<ActiveConnection>)disposer)
                    {
                        if (activeConnection.Status == ConnectionStatus.Connected)
                        {
                            activeConnection.Drop();

                        }
                    }
                    return new BaseCallResultModel(true, ServiceConstants.CallDropSuccess);
                };
            };

        }
        catch (Exception e)
        {
            _logger.LogDebug(e.ToString());
            
            return new BaseCallResultModel( false, ServiceConstants.CallDropError);

        }
    }

    public BaseCallResultModel TransferCall(string extension, string destinationNumber)
    {
        try
        {
            using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(extension))
            {
                if (!(dnByNumber is Extension))
                {
                    throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
                }
                
                
                if (destinationNumber.Length < 5)
                {
                    
                    var isExtensionExists = _extensionService.IsExtensionExists(destinationNumber);
            
                    if (!isExtensionExists)
                    {
                        throw new InvalidOperationException( ServiceConstants.ExtensionNotFound);

                    }
                    
                    if (!IsExtensionRegister(destinationNumber))
                    {
                        throw new InvalidOperationException(ServiceConstants.DestinationNumberUnregister);
                    }

                    TransferCallToDestNumber(extension, destinationNumber);
                    
                    return new BaseCallResultModel(true, ServiceConstants.CallTransferSuccess);
                }
  
                TransferCallToDestNumber(extension, destinationNumber);

                return new BaseCallResultModel(true, ServiceConstants.CallTransferSuccess);
                
            }

        }
        catch (Exception e)
        {
            _logger.LogDebug(e.ToString());
            
            return new BaseCallResultModel( false, ServiceConstants.CallTransferError);

        }
    }

    public List<CallStateModel> ActiveCallsInfo()
    {
        try
        {
            List<CallStateModel> callsState = new List<CallStateModel>();

            foreach (var keyValuePair in PhoneSystem.Root.GetActiveConnectionsByCallID())
            {
                var callState = new CallStateModel(keyValuePair.Key);
                
                callsState.Add(callState);
            
                foreach (var activeConnection in keyValuePair.Value)
                {
                    ProcessActiveConnection(callState, activeConnection);
                }
            }

            foreach (var call in callsState)
            {
                _logger.LogDebug("Call ID: {CallID}, Call Direction: {CallDirection}, Status: {Status}, Direction: {Direction}, Call Status: {CallStatus}, Local Number: {LocalNumber}, Remote Number: {RemoteNumber}",
                    call.CallID,
                    call.CallDirection,
                    call.Status,
                    call.Direction,
                    call.CallStatus,
                    call.LocalNumber,
                    call.RemoteNumber);
            }
            
            return callsState;
            
        }catch (Exception e)
        {
            _logger.LogDebug(e.ToString());
            
            throw e;

        }
    }
    
    private void ProcessActiveConnection(CallStateModel callState, ActiveConnection activeConnection)
    {
        if (callState.CallDirection == Direction.Undefined)
        {
            DetermineCallDirection(callState, activeConnection);
        }
        else
        {
            UpdateAConnStateStatus(callState, activeConnection);
        }
    }
    
    private void DetermineCallDirection(CallStateModel callState, ActiveConnection activeConnection)
    {
        _logger.LogDebug($"ID={activeConnection.ID}:CCID={activeConnection.CallConnectionID}:S={activeConnection.Status}:DN={activeConnection.DN.Number}:EP={activeConnection.ExternalParty}:REC={activeConnection.RecordingState}:STATUS={activeConnection.Status}");
        _logger.LogDebug((activeConnection.DN is ExternalLine).ToString());

        if (activeConnection.ExternalParty.Length > 5)
        {
            callState.RemoteNumber = activeConnection.ExternalParty;

            if (activeConnection.DN is ExternalLine)
            {
                callState.CallDirection = Direction.Inbound;
            }
            else
            {
                callState.CallDirection = Direction.Outbound;
                callState.LocalNumber = activeConnection.DN.Number;
            }
        }
        else
        {
            callState.RemoteNumber = activeConnection.ExternalParty;
            callState.CallDirection = Direction.Local;
            callState.LocalNumber = activeConnection.DN.Number;
        }
    }
    
    private void UpdateAConnStateStatus(CallStateModel callState, ActiveConnection activeConnection)
    {
        switch (callState.CallDirection)
        {
            case Direction.Outbound:
            case Direction.Local:
                UpdateStatus(callState, activeConnection.Status);
                break;
            case Direction.Inbound:
                callState.LocalNumber = activeConnection.DN.Number;
                UpdateStatus(callState, activeConnection.Status);
                break;
        }
    }
    
    private void UpdateStatus(CallStateModel callState, ConnectionStatus status)
    {
        switch (status)
        {
            case ConnectionStatus.Dialing:
                callState.Status = CallStatus.Dialing;
                break;
            case ConnectionStatus.Ringing:
                callState.Status = CallStatus.Ringing;
                break;
            case ConnectionStatus.Connected:
                callState.Status = CallStatus.Talking;
                break;
            default:
                callState.Status = CallStatus.Other;
                break;
        }
    }

    private void TransferCallToDestNumber(string extension, string destinationNumber)
    {

        var activeConnectionA = GetActiveConnectionByNumber(extension);
        
        var externalParty = (string)activeConnectionA.ExternalParty;
        
        var activeConnectionB = GetActiveConnectionByNumber(externalParty);


        if (externalParty != extension)
        {

            PhoneSystem.Root.TransferCall(activeConnectionB, destinationNumber);
            
        }
        else if(externalParty == extension)
        {

            PhoneSystem.Root.TransferCall(activeConnectionA, destinationNumber);

        }

    }
    
    private ActiveConnection GetActiveConnectionByNumber(string extension)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(extension))
        {
            using (IArrayDisposer<ActiveConnection> disposer = dnByNumber.GetActiveConnections().GetDisposer<ActiveConnection>())
            {
                var allTakenConnections = disposer.ToDictionary(x => x, y => y.OtherCallParties);

                ActiveConnection owner = null;

                foreach (var kv in allTakenConnections)
                {
                    owner = kv.Key;

                    //_logger.LogDebug($"ID={owner.ID}:CCID={owner.CallConnectionID}:S={owner.Status}:DN={owner.DN.Number}:EP={owner.ExternalParty}:REC={owner.RecordingState}");
                }

                if (owner == null)
                {
                    throw new InvalidOperationException(ServiceConstants.NoActiveConnection);
                }

                return owner;
            }
        }
        
    }

    private static bool IsExtensionRegister(string extension)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(extension))
        {
            return dnByNumber.IsRegistered;
        };
    }

}