using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PbxApiControl.Interface;
using PbxApiControl.Models.CallReply;

namespace PbxApiControl.Services.Grpc {
    public class CallService: CallPbxService.CallPbxServiceBase {
        private readonly ILogger<ContactService> _logger;
        private readonly ICallService _callService;
        private readonly IExtensionService _extensionService;
        private readonly IPbxEventListenerService _pbxEventListenerService;

        public CallService(ILogger<ContactService> logger, ICallService callService, IExtensionService extensionService, IPbxEventListenerService pbxEventListenerService) {
            _logger = logger;
            _callService = callService;
            _extensionService = extensionService;
            _pbxEventListenerService = pbxEventListenerService;
        }

        public override Task<BaseCallReply>MakeCall(MakeCallRequest request, ServerCallContext context) {
            try {

                var isExtensionExists = _extensionService.IsExtensionExists(request.From);

                if (!isExtensionExists) {
                    throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

                }

                var makeCallResult = _callService.MakeCall(request.To, request.From);

                return Task.FromResult(new BaseCallReply {
                    Result = makeCallResult.Result, Message = makeCallResult.Message
                });
            } catch (Exception e) {
                _logger.LogError("MakeCall: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }

        public override Task<BaseCallReply>HangupCall(HangupCallRequest request, ServerCallContext context) {
            try {

                var isExtensionExists = _extensionService.IsExtensionExists(request.Extension);

                if (!isExtensionExists) {
                    throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.ExtensionNotFound));

                }

                var hangupCallResult = _callService.HangupCall(request.Extension);

                return Task.FromResult(new BaseCallReply {
                    Result = hangupCallResult.Result, Message = hangupCallResult.Message
                });
            } catch (Exception e) {
                _logger.LogError("HangupCall: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }

        public override Task<BaseCallReply>TransferCall(TrasferCallRequest request, ServerCallContext context) {
            try {

                var transferCallResult = _callService.TransferCallByCallId((uint) request.CallId, request.PartyConnectionId, request.DestinationNumber);

                return Task.FromResult(new BaseCallReply {
                    Result = transferCallResult.Result, Message = transferCallResult.Message
                });
            } catch (Exception e) {
                _logger.LogError("TransferCall: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }

        public override Task<GetCountCallsReply>GetCountCalls(Empty request, ServerCallContext context) {
            try {

                var currentCountCalls = _callService.CountCalls();

                return Task.FromResult(new GetCountCallsReply {
                    CurrentCountCalls = currentCountCalls
                });
            } catch (Exception e) {
                _logger.LogError("GetCountCalls: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }

        public override Task<GetActiveCallsInfoReply>GetActiveCallsInfo(Empty request, ServerCallContext context) {
            try {

                var activeCallsInfo = _callService.ActiveCallsInfo();

                return Task.FromResult(ActiveCallsInfoReply.FormatActiveCallsInfo(activeCallsInfo));
            } catch (Exception e) {
                _logger.LogError("GetActiveCallsInfo: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }

        public override Task<GetActiveConnectionsInfoReply>GetActiveConnectionsInfo(Empty request, ServerCallContext context) {
            try {
                var activeConnectionsInfo = _callService.FullActiveConnectionsInfo();

                var formattedInfo = ActiveConnectionsInfoReply.FormatConnectionsInfoInfo(activeConnectionsInfo);

                return Task.FromResult(formattedInfo);
            } catch (Exception e) {
                _logger.LogError("GetActiveConnectionsInfo: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }

    }
}