using Grpc.Core;
using PbxApiControl.Interface;
using Google.Protobuf.WellKnownTypes;
using PbxApiControl.Models.RingGroup;
using PbxApiControl.Constants;

namespace PbxApiControl.Services.Grpc {
    public class RingGroupService: RingGroupPbxService.RingGroupPbxServiceBase {
        private readonly ILogger<RingGroupService> _logger;
        private readonly IRingGroupService _ringGroupService;

        public RingGroupService(ILogger<RingGroupService> logger, IRingGroupService ringGroupService) {
            _logger = logger;
            _ringGroupService = ringGroupService;
        }

        public override Task<RingGroupListReply>GetRingGroupList(Empty request, ServerCallContext context) {
            try {

                var ringGroups = _ringGroupService.GetRingGroupList();

                return Task.FromResult(new RingGroupListReply {
                    RingGroupNumbers = {
                        ringGroups
                    }
                });
            } catch (Exception e) {
                _logger.LogError("GetRingGroupList: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }

        public override Task<RingGroupMembersReply>GetRingGroupMembers(GetRingGroupMembersRequest request, ServerCallContext context) {
            try {

                var ringGroupExists = _ringGroupService.IsRingGroupExists(request.RingGroupNumber);

                if (!ringGroupExists) {
                    throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.RingGroupNotFound));

                }

                var ringGroupMembers = _ringGroupService.GetRingGroupMembers(request.RingGroupNumber);

                return Task.FromResult(new RingGroupMembersReply {
                    Numbers = {
                        ringGroupMembers
                    }
                });
            } catch (Exception e) {
                _logger.LogError("GetRingGroupMembers: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }

        public override Task<RingGroupMembersReply>AddMemberInRingGroup(AddMemberInRingGroupRequest request, ServerCallContext context) {
            try {

                var ringGroupExists = _ringGroupService.IsRingGroupExists(request.RingGroupNumber);

                if (!ringGroupExists) {
                    throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.RingGroupNotFound));

                }

                var ringGroupMembers = _ringGroupService.AddRingGroupMembers(new AddRingGroupMembersDataModel(request));

                return Task.FromResult(new RingGroupMembersReply {
                    Numbers = {
                        ringGroupMembers
                    }
                });
            } catch (Exception e) {
                _logger.LogError("AddMemberInRingGroup: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }

        public override Task<RingGroupMembersReply>DeleteMemberInRingGroup(DeleteMemberInRingGroupRequest request, ServerCallContext context) {
            try {

                var ringGroupExists = _ringGroupService.IsRingGroupExists(request.RingGroupNumber);

                if (!ringGroupExists) {
                    throw new RpcException(new Status(StatusCode.NotFound, ServiceConstants.RingGroupNotFound));

                }

                var ringGroupMembers = _ringGroupService.DeleteRingGroupMembers(new DeleteRingGroupMembersDataModel(request));

                return Task.FromResult(new RingGroupMembersReply {
                    Numbers = {
                        ringGroupMembers
                    }
                });
            } catch (Exception e) {
                _logger.LogError("DeleteMemberInRingGroup: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }

    }
}