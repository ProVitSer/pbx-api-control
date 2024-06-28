using Grpc.Core;
using PbxApiControl.Interface;
using Google.Protobuf.WellKnownTypes;
using PbxApiControl.Models.QueueReply;

namespace PbxApiControl.Services.Grpc {
    public class QueueService: QueuePbxService.QueuePbxServiceBase {
        private readonly ILogger<QueueService> _logger;
        private readonly IQueueService _queueService;

        public QueueService(ILogger<QueueService> logger, IQueueService queueService) {
            _logger = logger;
            _queueService = queueService;
        }

        public override Task<QueueListDataReply>GetQueueList(Empty request, ServerCallContext context) {
            try {

                var queueList = _queueService.QueueList();

                return Task.FromResult(new QueueListDataReply {
                    QueueNumbers = {
                        queueList
                    }
                });
            } catch (Exception e) {
                _logger.LogError("GetQueueList: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }
        }

        public override Task<QueueInfoReply>GetQueueAgents(QueueInfoRequest request, ServerCallContext context) {
            try {

                var isQueueNumberExists = _queueService.IsQueueExists(request.QueueNumber);

                if (!isQueueNumberExists) {
                    throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.QueueNotFound));

                }

                var queueAgents = _queueService.QueueAgents(request.QueueNumber);

                return Task.FromResult(new QueueInfoReply {
                    QueueInfo = {
                        queueAgents.Select(x => QueueAgentsInfoReply.FormatQueueInfo(x)).ToArray()
                    }
                });
            } catch (Exception e) {
                _logger.LogError("GetQueueAgents: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }
        }

        public override Task<QueueInfoReply>GetFreeQueueAgents(QueueInfoRequest request, ServerCallContext context) {
            try {

                var isQueueNumberExists = _queueService.IsQueueExists(request.QueueNumber);

                if (!isQueueNumberExists) {
                    throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.QueueNotFound));

                }

                var freeQueueAgents = _queueService.FreeQueueAgents(request.QueueNumber);

                return Task.FromResult(new QueueInfoReply {
                    QueueInfo = {
                        freeQueueAgents.Select(x => QueueAgentsInfoReply.FormatQueueInfo(x)).ToArray()
                    }
                });
            } catch (Exception e) {
                _logger.LogError("GetFreeQueueAgents: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }
        }

        public override Task<QueueInfoReply>GetBusyQueueAgents(QueueInfoRequest request, ServerCallContext context) {
            try {

                var isQueueNumberExists = _queueService.IsQueueExists(request.QueueNumber);

                if (!isQueueNumberExists) {
                    throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.QueueNotFound));

                }

                var busyQueueAgents = _queueService.BusyQueueAgents(request.QueueNumber);

                return Task.FromResult(new QueueInfoReply {
                    QueueInfo = {
                        busyQueueAgents.Select(x => QueueAgentsInfoReply.FormatQueueInfo(x)).ToArray()
                    }
                });
            } catch (Exception e) {
                _logger.LogError("GetBusyQueueAgents: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }
        }

        public override Task<QueueModifyReply>AddAgentsToQueue(QueueModifyRequest request, ServerCallContext context) {
            try {

                var isQueueNumberExists = _queueService.IsQueueExists(request.QueueNumber);

                if (!isQueueNumberExists) {
                    throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.QueueNotFound));

                }

                var queueData = _queueService.AddQueueAgents(request.QueueNumber, request.Agents.ToArray());

                return Task.FromResult(new QueueModifyReply {
                    QueueNumber = queueData.QueueNumber, Agents = {
                        queueData.QueueAgents
                    }
                });
            } catch (Exception e) {
                _logger.LogError("AddAgentsToQueue: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }
        }

        public override Task<QueueModifyReply>DeleteAgentsFromQueue(QueueModifyRequest request, ServerCallContext context) {
            try {

                var isQueueNumberExists = _queueService.IsQueueExists(request.QueueNumber);

                if (!isQueueNumberExists) {
                    throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.QueueNotFound));

                }

                var queueData = _queueService.RemoveQueueAgents(request.QueueNumber, request.Agents.ToArray());

                return Task.FromResult(new QueueModifyReply {
                    QueueNumber = queueData.QueueNumber, Agents = {
                        queueData.QueueAgents
                    }
                });
            } catch (Exception e) {
                _logger.LogError("DeleteAgentsFromQueue: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }
        }
    }
}