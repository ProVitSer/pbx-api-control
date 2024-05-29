using PbxApiControl.Models;

namespace PbxApiControl.Interface;
public interface IExtensionService
{
    ExtensionStatus? ExtensionStatus(string ext);
    ExtensionInfo? ExtensionInfo(string ext);
}