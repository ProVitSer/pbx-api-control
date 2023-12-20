
using System.Collections.Generic;
using PbxApiControl.Models;

namespace PbxApiControl.Interface;
public interface IExtensionService
{

    IEnumerable<string> GetAllExtensions();
    IEnumerable<string> GetRegisteredExtensions();

    ExtensionStatus GetExtensionStatus(string ext);
    ExtensionInfo GetExtensionInfo(string ext);

}
