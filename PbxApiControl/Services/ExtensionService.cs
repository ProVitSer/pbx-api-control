using System.Collections.Generic;
using System.Linq;
using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.Models;



namespace PbxApiControl.Services;

#nullable enable
public class ExtensionService : IExtensionService
{
    private readonly IPbxService _pbxService;

    public ExtensionService(IPbxService pbxService)
    {
        _pbxService = pbxService;
    }

    public IEnumerable<string> GetAllExtensions()
    {
        return _pbxService.GetPbxNumbers<Extension>();
    }

    public IEnumerable<string> GetRegisteredExtensions()
    {
        return GetRegisteredExtensions<Extension>();
    }


    public ExtensionStatus? GetExtensionStatus(string ext)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            if (dnByNumber is Extension extension)
            {
                return new ExtensionStatus(extension);

            }
            return null;
        };

    }

    public ExtensionInfo? GetExtensionInfo(string ext)
    {
        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            if (dnByNumber is Extension extension)
            {
                return new ExtensionInfo(extension);

            }
            return null;
        };
    }

    private IEnumerable<string> GetRegisteredExtensions<T>() where T : class, DN
    {

        using (IArrayDisposer<T> disposer = PhoneSystem.Root.GetAll<T>().GetDisposer())
        {
            var RegNumber = disposer
            .Where(x => x.IsRegistered)
            .Select(x => x.Number)
            .ToArray();

            return RegNumber;
        };
    }
}