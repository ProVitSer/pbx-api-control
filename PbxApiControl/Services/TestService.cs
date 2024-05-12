using PbxApiControl.Interface;
using Grpc.Core;
using Google.Protobuf.Collections;

namespace PbxApiControl.Services
{
    public class TestService : ExtensionTest.ExtensionTestBase
    {
        private readonly IExtensionService _extensionService;

        public TestService(IExtensionService extensionService)
        {
            _extensionService = extensionService;
        }

        public override Task<ExtensionStatusReply> TestGetExtensionStatus(ExtensionStatusRequest request, ServerCallContext context)
        {
            Console.WriteLine(request);

            var extensionStatus = _extensionService.GetExtensionStatus(request.Ext);
            
            RepeatedField<string> groups = new RepeatedField<string>();
            groups.AddRange(extensionStatus.Groups);

            RepeatedField<string> queues = new RepeatedField<string>();
            groups.AddRange(extensionStatus.Queues);
            
            RepeatedField<string> ringGroups = new RepeatedField<string>();
            groups.AddRange(extensionStatus.RingGroups);
            
            var reply = new ExtensionStatusReply
            {
                Extension = extensionStatus.Extension,
                Registered = extensionStatus.Registered,
                ForwardingRulesStatus = extensionStatus.ForwardingRulesStatus,
                QueuesStatus = extensionStatus.QueuesStatus,
                Groups = { groups },
                Queues = { queues },
                RingGroups = { ringGroups }
            };
            return Task.FromResult(reply);
        }
    }
}

