using Grpc.Core;
using PbxApiControl.Interface;
using Google.Protobuf.WellKnownTypes;  
using PbxApiControl.Models.Extensions;

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
            var extensionInfo = _extensionService.ExtensionStatus(request.Extension);
            
            if (extensionInfo == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

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
            var extensionInfo = _extensionService.ExtensionInfo(request.Extension);
        
            _logger.LogInformation("GetExtensionInfo: {@extensionInfo}", extensionInfo);

            if (extensionInfo == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

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
    
     public override Task<GetExtensionsReply> GetExtensions(Empty request, ServerCallContext context)
    {
        try
        {
            var extensions = _extensionService.AllExtensions();
        
            _logger.LogInformation("GetExtensions: {@extensions}", extensions);

            var reply = new GetExtensionsReply
            {
    
                Extensions = { extensions }

            };
            return Task.FromResult(reply);
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
        
    }
     
     public override Task<GetRegisteredExtensionsReply> GetRegisteredExtensions(Empty request, ServerCallContext context)
     {
         try
         {
             var regExtensions = _extensionService.RegisteredExtensions();
        
             _logger.LogInformation("GetRegisteredExtensions: {@regExtensions}", regExtensions);

             var reply = new GetRegisteredExtensionsReply
             {
    
                 Extensions = { regExtensions }

             };
             return Task.FromResult(reply);
         }
         catch (Exception e)
         {
             throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

         }
        
     }
     
     public override Task<GetExtensionDeviceInfoReply> GetExtensionDeviceInfo(GetExtensionDeviceInfoRequest request, ServerCallContext context)
     {
         try
         {
             var extensionDeviceInfo = _extensionService.ExtensionDeviceInfo(request.Extension);
        
             _logger.LogInformation("GetExtensionDeviceInfo: {@extensionDeviceInfo}", extensionDeviceInfo);
             
             if (extensionDeviceInfo == null)
             {
                 throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

             }

             var reply = new GetExtensionDeviceInfoReply
             {
    
                 Extension = extensionDeviceInfo.Extension

             };
             
             reply.Devices.AddRange(extensionDeviceInfo.Devices.Select(devInfo => new Device
             {
                 UserAgent = devInfo.UserAgent,
                 Contact = devInfo.Contact
             }));
             
             return Task.FromResult(reply);
         }
         catch (Exception e)
         {
             throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

         }
        
     }
     
      public override Task<CreateExtensionReply> CreateExtension(CreateExtensionRequest request, ServerCallContext context)
    {
        try
        {

            var extensionInfo = _extensionService.CreateExt(new CreateExtensionDataModel(request));
        
            _logger.LogInformation("CreateExtension: {@extensionInfo}", extensionInfo);

            if (extensionInfo == null)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, ServiceConstants.ExtensionExists));

            }
        
            var reply = new CreateExtensionReply
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
    
       public override Task<DeleteExtensionReply> DeleteExtension(DeleteExtensionRequest request, ServerCallContext context)
    {
        try
        {

            var result = _extensionService.DeleteExt(request.Extension);
        
            _logger.LogInformation("DeleteExtension: {@result}", result);

            if (result == false)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

            }
        
            var reply = new DeleteExtensionReply
            {
    
                Result = result,
  
            };
            return Task.FromResult(reply);
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
    }
       
    public override Task<UpdateExtensionInfoReply> UpdateExtensionInfo(UpdateExtensionInfoRequest request, ServerCallContext context)
    {
           try
           {
               
               _logger.LogInformation("GetExtensionInfo: {@request}", request);

               var extensionInfo = _extensionService.UpdateExt(new UpdateExtensionDataModel(request));
        
               _logger.LogInformation("UpdateExtensionInfo: {@extensionInfo}", extensionInfo);

               if (extensionInfo == null)
               {
                   throw new RpcException(new Status(StatusCode.AlreadyExists, ServiceConstants.ExtensionNotFound));

               }

               var reply = new UpdateExtensionInfoReply
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
    
    public override Task<SetExtensionForwardStatusReply> SetExtensionForwardStatus(SetExtensionForwardStatusRequest request, ServerCallContext context)
    {
           try
           {

               var result = _extensionService.SetExtForwardStatus(new ExtensionForwardStatusDataMode(request));
        
               _logger.LogInformation("SetExtensionForwardStatus: {@result}", result);

               if (result == false)
               {
                   throw new RpcException(new Status(StatusCode.AlreadyExists, ServiceConstants.ExtensionNotFound));

               }

               var reply = new SetExtensionForwardStatusReply
               {
    
                   Result = result,

  
               };
               return Task.FromResult(reply);
           }
           catch (Exception e)
           {
               throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

           }
    }
    
    public override Task<SetExtensionGlobalQueuesStatusReply> SetExtensionGlobalQueuesStatus(SetExtensionGlobalQueuesStatusRequest request, ServerCallContext context)
    {
        try
        {

            var result = _extensionService.SetExtQueuesStatus(new ExtensionQueuesStatusDataModel(request));
        
            _logger.LogInformation("SetExtensionGlobalQueuesStatus: {@result}", result);

            if (result == false)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, ServiceConstants.ExtensionNotFound));

            }

            var reply = new SetExtensionGlobalQueuesStatusReply
            {
    
                Result = result,

  
            };
            return Task.FromResult(reply);
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
    }
    
    public override Task<SetExtensionStatusInQueueReply> SetExtensionStatusInQueue(SetExtensionStatusInQueueRequest request, ServerCallContext context)
    {
        try
        {

            var result = _extensionService.SetExtQueueStatus(new ExtensionQueueStatusDataModel(request));
        
            _logger.LogInformation("SetExtensionStatusInQueue: {@result}", result);

            if (result == false)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, ServiceConstants.ExtensionNotFound));

            }

            var reply = new SetExtensionStatusInQueueReply
            {
    
                Result = result,

  
            };
            return Task.FromResult(reply);
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
    }
}