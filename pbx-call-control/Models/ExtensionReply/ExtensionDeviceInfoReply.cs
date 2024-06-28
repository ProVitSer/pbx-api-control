using PbxApiControl.Models.Extensions;

namespace PbxApiControl.Models.ExtensionReply
{
    public class ExtensionDeviceInfoReply
    {
        public static GetExtensionDeviceInfoReply FormatExtensionDeviceInfo(ExtensionDeviceInfo extensionDeviceInfo)
        {
            var reply = new GetExtensionDeviceInfoReply
            {
            
                Extension = extensionDeviceInfo.Extension
            
            };
            
            reply.Devices.AddRange(extensionDeviceInfo.Devices!.Select(devInfo => new Device
            {
                UserAgent = devInfo.UserAgent,
                Contact = devInfo.Contact
            }));

            return reply;
        }
    }
}

