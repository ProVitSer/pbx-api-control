using Grpc.Core;
using PbxApiControl.Interface;


namespace PbxApiControl.Services.Grpc;
public class ExtensionService : ExtensionsPbxService.ExtensionsPbxServiceBase
{
    
    private readonly ILogger<ExtensionService> _logger;
    private readonly IExtensionService _extensionService;

    public ExtensionService(ILogger<ExtensionService> logger, IExtensionService extensionService)
    {
        _logger = logger;
        _extensionService = extensionService;
    }
    
    public override Task<GetExtensionStatusReply> GetExtensionStatus(GetExtensionStatusRequest request, ServerCallContext context)
    {
        try
        {
            var extensionInfo = _extensionService.ExtensionStatus(request.Ext);
        
            _logger.LogInformation("GetExtensionStatus: {@extensionInfo}", extensionInfo);

            if (extensionInfo == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Добавочный номер не найден"));

            }
        
            var reply = new GetExtensionStatusReply
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
    
    
    public override Task<GetExtensionInfoReply> GetExtensionInfo(GetExtensionInfoRequest request, ServerCallContext context)
    {
        try
        {
            var extensionInfo = _extensionService.ExtensionInfo(request.Ext);
        
            _logger.LogInformation("GetExtensionInfo: {@extensionInfo}", extensionInfo);

            if (extensionInfo == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Добавочный номер не найден"));

            }
        
            var reply = new GetExtensionInfoReply
            {
    
                AuthId = extensionInfo.AuthID,
                AuthPassword = extensionInfo.AuthPassword,
                SipId = extensionInfo.SipID,
                Extension = extensionInfo.Extension,
                FirstName = extensionInfo.FirstName,
                LastName = extensionInfo.LastName,
                Email = extensionInfo.Email,
                MobileNumber = extensionInfo.MobileNumber,
                OutboundCallerId = extensionInfo.OutboundCallerID,
                RecordingType = extensionInfo.RecordingType,
                IsExtenionEnabled = extensionInfo.IsExtenionEnabled,
                AllowedExternalCalls = extensionInfo.AllowedExternalCalls,
                DeliverAudio = extensionInfo.DeliverAudio,
                SupportReinvite = extensionInfo.SupportReinvite,
                SupportReplaces = extensionInfo.SupportReplaces,
                EmailOptions = extensionInfo.EmailOptions,
                VoiceMailEnable = extensionInfo.VoiceMailEnable,
                VoiceMailPin = extensionInfo.VoiceMailPin,
                VoiceMailPlayCallerId = extensionInfo.VoiceMailPlayCallerID,
                Internal = extensionInfo.Internal,
                NoAnswerTimeout = extensionInfo.NoAnswerTimeout
            };
            return Task.FromResult(reply);
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
        
    }

}