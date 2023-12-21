
using System.Collections.Generic;
using PbxApiControl.Models;
using PbxApiControl.DTOs.Extension;

namespace PbxApiControl.Interface;

#nullable enable

public interface IExtensionService
{
    IEnumerable<string> GetAllExtensions();
    IEnumerable<string> GetRegisteredExtensions();
    ExtensionStatus? GetExtensionStatus(string ext);
    ExtensionInfo? GetExtensionInfo(string ext);
    ExtensionInfo? CreateExtension(BaseExtensionDto dto);
    bool? DeleteExtension(DeleteExtensionDto dto);
    ExtensionInfo? UpdateExtension(BaseExtensionDto dto);
    bool? SetExtensionForwardStatus(SetForwardStatusDto dto);

    bool? SetExtensionQueuesStatus(SetQueuestatusDto dto);
    bool? SetExtensionQueueStatus(SetQueuetatusDto dto);
}
