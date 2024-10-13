using Grpc.Core;
using PbxApiControl.Interface;
using Google.Protobuf.WellKnownTypes;

namespace PbxApiControl.Services.Grpc
{
    public class IvrService : IvrPbxService.IvrPbxServiceBase
    {
        private readonly ILogger<RingGroupService> _logger;
        private readonly IIvrService _ivrService;

        public IvrService(ILogger<RingGroupService> logger, IIvrService ivrService)
        {
            _logger = logger;
            _ivrService = ivrService;
        }

        public override Task<IvrListReply> GetIvrList(Empty request, ServerCallContext context)
        {
            try
            {

                var ivrList = _ivrService.GetIvrList();

                var response = new IvrListReply();

                response.Ivrs.AddRange(ivrList.Select(q => new IvrListInfo
                {
                    Name = q.Name,
                    Number = q.Number
                }));


                return Task.FromResult(response);
            }
            catch (Exception e)
            {
                _logger.LogError("GetRingGroupList: {@e}", e.ToString());

                throw new RpcException(new Status(StatusCode.Internal, e.ToString()));

            }

        }

    }
}