using Grpc.Core;
using PbxApiControl.Interface;
using Google.Protobuf.WellKnownTypes;

namespace PbxApiControl.Services.Grpc
{
    public class PbxEventListenerService : PbxSubscribeEventService.PbxSubscribeEventServiceBase {
        
        private readonly ILogger<PbxEventListenerService> _logger;
        private readonly IPbxEventListenerService _pbxEventListenerService;

        public PbxEventListenerService(ILogger<PbxEventListenerService> logger, IPbxEventListenerService pbxEventListenerService) {
            _logger = logger;
            _pbxEventListenerService = pbxEventListenerService;
        }
        
        
        public override Task<Empty>SubscribeInsertedEvent(Empty request, ServerCallContext context) {
            try {

                _pbxEventListenerService.StartListenInsertedEvent();

                return Task.FromResult(request);
                
            } catch (Exception e) {
                _logger.LogError("SubscribeInsertedEvent: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }
        }
        
        public override Task<Empty>SubscribeDeletedEvent(Empty request, ServerCallContext context) {
            try {

                _pbxEventListenerService.StartListenDeletedEvent();

                return Task.FromResult(request);
                
            } catch (Exception e) {
                _logger.LogError("SubscribeDeletedEvent: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }
        }
        
        public override Task<Empty>SubscribeUpdateEvent(Empty request, ServerCallContext context) {
            try {

                _pbxEventListenerService.StartListenUpdatedEvent();

                return Task.FromResult(request);
                
            } catch (Exception e) {
                _logger.LogError("SubscribeUpdateEvent: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }
        }
        
        public override Task<Empty>UnsubscribeInsertedEvent(Empty request, ServerCallContext context) {
            try {

                _pbxEventListenerService.StopListenInsertedEvent();

                return Task.FromResult(request);
                
            } catch (Exception e) {
                _logger.LogError("UnsubscribeInsertedEvent: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }
        }
        
        public override Task<Empty>UnsubscribeDeletedEvent(Empty request, ServerCallContext context) {
            try {

                _pbxEventListenerService.StopListenDeletedEvent();

                return Task.FromResult(request);
                
            } catch (Exception e) {
                _logger.LogError("UnsubscribeDeletedEvent: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }
        }
        
        public override Task<Empty>UnsubscribeUpdateEvent(Empty request, ServerCallContext context) {
            try {

                _pbxEventListenerService.StopListenUpdatedEvent();

                return Task.FromResult(request);
                
            } catch (Exception e) {
                _logger.LogError("UnsubscribeUpdateEvent: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }
        }

    }
}

