using Grpc.Core;
using PbxApiControl.Interface;
using Google.Protobuf.WellKnownTypes;  
using PbxApiControl.Models.Extensions;
using PbxApiControl.Models.Reply;

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
            var extensionExists = _extensionService.IsExtensionExists(request.Extension);
            
            if (!extensionExists)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

            }
            
            var extensionStatus = _extensionService.ExtensionStatus(request.Extension);
            
            return Task.FromResult(ExtensionStatusReply.FormatExtensionStatus(extensionStatus));
        }
        catch (Exception e)
        {
            _logger.LogError("GetExtensionInfo: {@e}", e.ToString());
            
            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
        
    }
    
    
    public override Task<GetExtensionInfoReply> GetExtensionInfo(GetExtensionInfoRequest request, ServerCallContext context)
    {
        try
        {
            
            var extensionExists = _extensionService.IsExtensionExists(request.Extension);
            
            if (!extensionExists)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

            }
            var extensionInfo = _extensionService.ExtensionInfo(request.Extension);
            
            return Task.FromResult(ExtensionInfoReply.FormatExtensionInfo(extensionInfo));
        }
        catch (Exception e)
        {
            _logger.LogError("GetExtensionInfo: {@e}", e.ToString());

            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
        
    }
    
     public override Task<GetExtensionsReply> GetExtensions(Empty request, ServerCallContext context)
    {
        try
        {
            var extensions = _extensionService.AllExtensions();
            
            return Task.FromResult(ExtensionsReply.FormatExtensions(extensions));

        }
        catch (Exception e)
        {
            _logger.LogError("GetExtensions: {@e}", e.ToString());

            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
    }
     
     public override Task<GetRegisteredExtensionsReply> GetRegisteredExtensions(Empty request, ServerCallContext context)
     {
         try
         {
             var regExtensions = _extensionService.RegisteredExtensions();
        
             return Task.FromResult(RegisteredExtensionsReply.FormatRegisteredExtensions(regExtensions));

         }
         catch (Exception e)
         {
             _logger.LogError("GetRegisteredExtensions: {@e}", e.ToString());

             throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

         }
        
     }
     
     public override Task<GetExtensionDeviceInfoReply> GetExtensionDeviceInfo(GetExtensionDeviceInfoRequest request, ServerCallContext context)
     {
         try
         {
             
             var extensionExists = _extensionService.IsExtensionExists(request.Extension);
            
             if (!extensionExists)
             {
                 throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

             }

             var extensionDeviceInfo = _extensionService.ExtensionDeviceInfo(request.Extension);
             
             return Task.FromResult(ExtensionDeviceInfoReply.FormatExtensionDeviceInfo(extensionDeviceInfo));

         }
         catch (Exception e)
         {
             _logger.LogError("GetExtensionDeviceInfo: {@e}", e.ToString());

             throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

         }
        
     }
     
      public override Task<CreateExtensionReply> CreateExtension(CreateExtensionRequest request, ServerCallContext context)
    {
        try
        {

            var extensionExists = _extensionService.IsExtensionExists(request.Extension);

            if (extensionExists)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, ServiceConstants.ExtensionExists));

            }
            
            var createExtension = _extensionService.CreateExt(new CreateExtensionDataModel(request));
        
            return Task.FromResult(CreateExtensionInfoReply.FormatCreateExtensionInfo(createExtension));

        }
        catch (Exception e)
        {
            _logger.LogError("CreateExtension: {@e}", e.ToString());

            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
        
    }
    
       public override Task<DeleteExtensionReply> DeleteExtension(DeleteExtensionRequest request, ServerCallContext context)
    {
        try
        {

            var extensionExists = _extensionService.IsExtensionExists(request.Extension);
            
            if (!extensionExists)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

            }
            
            var result = _extensionService.DeleteExt(request.Extension);
            
            return Task.FromResult(DeleteExtensionResultReply.FormatDeleteExtensionResult(result));

        }
        catch (Exception e)
        {
            _logger.LogError("DeleteExtension: {@e}", e.ToString());

            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
    }
       
    public override Task<UpdateExtensionInfoReply> UpdateExtensionInfo(UpdateExtensionInfoRequest request, ServerCallContext context)
    {
           try
           {
               
               var extensionExists = _extensionService.IsExtensionExists(request.Extension);
            
               if (!extensionExists)
               {
                   throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

               }
               
               var extensionInfo = _extensionService.UpdateExt(new UpdateExtensionDataModel(request));
        
               return Task.FromResult(UpdateExtensionResultInfoReply.FormatUpdateExtensionResultInfo(extensionInfo));

           }
           catch (Exception e)
           {
               _logger.LogError("UpdateExtensionInfo: {@e}", e.ToString());

               throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

           }
    }
    
    public override Task<SetExtensionForwardStatusReply> SetExtensionForwardStatus(SetExtensionForwardStatusRequest request, ServerCallContext context)
    {
           try
           {
               var extensionExists = _extensionService.IsExtensionExists(request.Extension);
            
               if (!extensionExists)
               {
                   throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

               }
               
               var result = _extensionService.SetExtForwardStatus(new ExtensionForwardStatusDataMode(request));
        
               return Task.FromResult(ExtensionForwardStatusReply.FormatExtensionForwardStatus(result));

           }
           catch (Exception e)
           {
               _logger.LogError("SetExtensionForwardStatus: {@e}", e.ToString());

               throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

           }
    }
    
    public override Task<SetExtensionGlobalQueuesStatusReply> SetExtensionGlobalQueuesStatus(SetExtensionGlobalQueuesStatusRequest request, ServerCallContext context)
    {
        try
        {
            var extensionExists = _extensionService.IsExtensionExists(request.Extension);
            
            if (!extensionExists)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

            }

            var result = _extensionService.SetExtQueuesStatus(new ExtensionQueuesStatusDataModel(request));
        
            return Task.FromResult(ExtensionGlobalQueuesStatusReply.FormatExtensionGlobalQueuesStatus(result));

        }
        catch (Exception e)
        {
            _logger.LogError("SetExtensionGlobalQueuesStatus: {@e}", e.ToString());

            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
    }
    
    public override Task<SetExtensionStatusInQueueReply> SetExtensionStatusInQueue(SetExtensionStatusInQueueRequest request, ServerCallContext context)
    {
        try
        {

            var extensionExists = _extensionService.IsExtensionExists(request.Extension);
            
            if (!extensionExists)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

            }
            
            var result = _extensionService.SetExtQueueStatus(new ExtensionQueueStatusDataModel(request));
            
            return Task.FromResult(ExtensionStatusInQueueReply.FormatExtensionStatusInQueue(result));

        }
        catch (Exception e)
        {
            _logger.LogError("SetExtensionStatusInQueue: {@e}", e.ToString());

            throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

        }
    }
}