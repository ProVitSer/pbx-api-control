namespace PbxApiControl.Models.Reply
{
    public class ExtensionForwardStatusReply
    {
        public static SetExtensionForwardStatusReply FormatExtensionForwardStatus(bool result)
        {
            return new SetExtensionForwardStatusReply
            {
    
                Result = result,

            };
        }
    }
}

