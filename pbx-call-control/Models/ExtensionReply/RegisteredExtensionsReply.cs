namespace PbxApiControl.Models.ExtensionReply
{
    public class RegisteredExtensionsReply
    {
        public static GetRegisteredExtensionsReply FormatRegisteredExtensions(IEnumerable<string> regExtensions)
        {
            return new GetRegisteredExtensionsReply
            {
                Extensions = { regExtensions }
            };
        }
    }
}

