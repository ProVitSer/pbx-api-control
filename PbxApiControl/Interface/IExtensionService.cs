using PbxApiControl.Models.Extensions;

namespace PbxApiControl.Interface;
public interface IExtensionService
{
    ExtensionStatus? ExtensionStatus(string ext);
    ExtensionInfo? ExtensionInfo(string ext);
    IEnumerable<string> AllExtensions();
    IEnumerable<string> RegisteredExtensions();
    ExtensionDeviceInfo?  ExtensionDeviceInfo(string ext);
    ExtensionInfo? CreateExt(CreateExtensionDataModel data);
    bool DeleteExt(string ext);
    ExtensionInfo? UpdateExt(UpdateExtensionDataModel data);
    bool SetExtForwardStatus(ExtensionForwardStatusDataMode data);
    bool SetExtQueuesStatus(ExtensionQueuesStatusDataModel data);
    bool SetExtQueueStatus(ExtensionQueueStatusDataModel data);

}