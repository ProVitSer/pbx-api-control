using PbxApiControl.Models;

namespace PbxApiControl.Interface;
public interface IExtensionService
{
    ExtensionInfo? GetExtensionInfo(string ext);

}