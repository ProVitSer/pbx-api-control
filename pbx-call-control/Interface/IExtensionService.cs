using PbxApiControl.Models.Extensions;

namespace PbxApiControl.Interface
{
    public interface IExtensionService
    {
    
        bool IsExtensionExists(string ext);
        ExtensionStatus ExtensionStatus(string ext);
        ExtensionInfo ExtensionInfo(string ext);
        IEnumerable<string> AllExtensions();
        IEnumerable<string> RegisteredExtensions();
        ExtensionDeviceInfo  ExtensionDeviceInfo(string ext);
        ExtensionInfo CreateExt(CreateExtensionDataModel data);
        bool DeleteExt(string ext);
        ExtensionInfo UpdateExt(UpdateExtensionDataModel data);
        ExtensionStatus SetExtForwardStatus(ExtensionForwardStatusDataModel data);
        ExtensionStatus SetExtQueuesStatus(ExtensionQueuesStatusDataModel data);
        ExtensionStatus SetExtQueueStatus(ExtensionQueueStatusDataModel data);

    }
}
