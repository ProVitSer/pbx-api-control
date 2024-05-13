
using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.Models;


namespace PbxApiControl.Services;

public class ExtensionService : IExtensionService
{
    private readonly ILogger<ExtensionService> _logger;

    public ExtensionService(ILogger<ExtensionService> logger)
    {
        _logger = logger;
    }


    public ExtensionInfo GetExtensionInfo(string ext)
    {

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            if (dnByNumber is Extension extension)
            {
                return new ExtensionInfo(extension);

            }
        };

        return null;
    }
}


//     public override Task<ExtensionStatusReply> TestGetExtensionStatus(ExtensionStatusRequest request, ServerCallContext context)
//     {
//         _logger.LogInformation("ExtensionStatusReply", request);

//         var extensionStatus = _extensionService.GetExtensionStatus(request.Ext);

//         RepeatedField<string> groups = new RepeatedField<string>();
//         groups.AddRange(extensionStatus.Groups);

//         RepeatedField<string> queues = new RepeatedField<string>();
//         queues.AddRange(extensionStatus.Queues);

//         RepeatedField<string> ringGroups = new RepeatedField<string>();
//         ringGroups.AddRange(extensionStatus.RingGroups);

//         var reply = new ExtensionStatusReply
//         {
//             Extension = extensionStatus.Extension,
//             Registered = extensionStatus.Registered,
//             ForwardingRulesStatus = extensionStatus.ForwardingRulesStatus,
//             QueuesStatus = extensionStatus.QueuesStatus,
//             Groups = { groups },
//             Queues = { queues },
//             RingGroups = { ringGroups }
//         };
//         return Task.FromResult(reply);
//     }
// }