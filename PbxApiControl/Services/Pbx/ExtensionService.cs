
using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.Models;


namespace PbxApiControl.Services.Pbx;

public class ExtensionService : IExtensionService
{
    private readonly ILogger<ExtensionService> _logger;

    public ExtensionService(ILogger<ExtensionService> logger)
    {
        _logger = logger;
    }


    public ExtensionInfo? GetExtensionInfo(string ext)
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


