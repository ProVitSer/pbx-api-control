
namespace PbxApiControl.Models.ExtensionReply
{
    public class ExtensionsReply
    {
        public static GetExtensionsReply FormatExtensions(IEnumerable<string> extensions)
        {
            return new GetExtensionsReply
            {
                Extensions = { extensions }
            };
        }
    }
}


