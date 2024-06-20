using PbxApiControl.Interface;
using PbxApiControl.Models.Call;
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