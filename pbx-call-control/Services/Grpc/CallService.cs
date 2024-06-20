using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PbxApiControl.Interface;

namespace PbxApiControl.Services.Grpc;

public class CallService : CallPbxService.CallPbxServiceBase
{
    private readonly ILogger<ContactService> _logger;
    private readonly ICallService _callService;
    private readonly IExtensionService _extensionService;

    public CallService(ILogger<ContactService> logger, ICallService callService, IExtensionService extensionService)
    {
        _logger = logger;
        _callService = callService;
        _extensionService = extensionService;

    }
    
    public override Task<BaseCallReply> MakeCall(MakeCallRequest request, ServerCallContext context)
    {
        try
        {

            var isExtensionExists = _extensionService.IsExtensionExists(request.From);
            
            if (!isExtensionExists)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

            }
            
            var makeCallResult = _callService.MakeCall(request.To, request.From);
            
            return Task.FromResult(new BaseCallReply{ Result  = makeCallResult.Result, Message = makeCallResult.Message});
        }
        catch (Exception e)
        {
            _logger.LogError("MakeCall: {@e}", e.ToString());
            
            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
        
    }
    
    public override Task<BaseCallReply> HangupCall(HangupCallRequest request, ServerCallContext context)
    {
        try
        {

            var isExtensionExists = _extensionService.IsExtensionExists(request.Extension);
            
            if (!isExtensionExists)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

            }
            
            var hangupCallResult = _callService.HangupCall(request.Extension);
            
            return Task.FromResult(new BaseCallReply{ Result  = hangupCallResult.Result, Message = hangupCallResult.Message});
        }
        catch (Exception e)
        {
            _logger.LogError("HangupCall: {@e}", e.ToString());
            
            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
        
    }
    
    public override Task<BaseCallReply> TransferCall(TrasferCallRequest request, ServerCallContext context)
    {
        try
        {

            var isExtensionExists = _extensionService.IsExtensionExists(request.Extension);
            
            if (!isExtensionExists)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

            }
            
            var transferCallResult = _callService.TransferCall(request.Extension, request.DestinationNumber);
            
            return Task.FromResult(new BaseCallReply{ Result  = transferCallResult.Result, Message = transferCallResult.Message});
        }
        catch (Exception e)
        {
            _logger.LogError("TransferCall: {@e}", e.ToString());
            
            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
        
    }
    
    public override Task<GetCountCallsReply> GetCountCalls(Empty request, ServerCallContext context)
    {
        try
        {

            var currentCountCalls = _callService.CountCalls();
            
            return Task.FromResult(new GetCountCallsReply{ CurrentCountCalls = currentCountCalls });
        }
        catch (Exception e)
        {
            _logger.LogError("TransferCall: {@e}", e.ToString());
            
            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
        
    }
}