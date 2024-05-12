
using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.Models;


namespace PbxApiControl.Services;

public class ExtensionService : IExtensionService
{
    
    public  NewExtensionStatus GetExtensionStatus(string ext)
    {

        using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber(ext))
        {
            if (dnByNumber is Extension extension)
            {
                return new NewExtensionStatus(extension);

            }
        };

        return null;
    }
}