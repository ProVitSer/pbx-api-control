using Grpc.Core;
using Google.Protobuf.Collections;
using PbxApiControl.Interface;


namespace PbxApiControl.Services.Grpc;
public class ExtensionService : ExtensionsInfoService.ExtensionsInfoServiceBase
{
    
    private readonly ILogger<ExtensionService> _logger;
    private readonly IExtensionService _extensionService;

    public ExtensionService(ILogger<ExtensionService> logger, IExtensionService extensionService)
    {
        _logger = logger;
        _extensionService = extensionService;
    }
    
    public override Task<ExtensionInfoReply> GetExtensionInfo(ExtensionInfoRequest request, ServerCallContext context)
    {
        try
        {
            var extensionInfo = _extensionService.GetExtensionInfo(request.Ext);
        
            _logger.LogInformation("ExtensionInfo: {@extensionInfo}", extensionInfo);

            if (extensionInfo == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Добавочный номер не найден"));

            }
        
            var reply = new ExtensionInfoReply
            {
                Extension = extensionInfo.Extension,
                Registered = extensionInfo.Registered,
                ForwardingRulesStatus = extensionInfo.ForwardingRulesStatus,
                QueuesStatus = extensionInfo.QueuesStatus,
                Groups = { extensionInfo.Groups },
                Queues = { extensionInfo.Queues },
                RingGroups = { extensionInfo.RingGroups }
            };
            return Task.FromResult(reply);
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
        
    }

}