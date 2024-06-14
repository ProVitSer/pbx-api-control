namespace PbxApiControl.Models.Reply
{
    public class ExtensionGlobalQueuesStatusReply
    {
        public static SetExtensionGlobalQueuesStatusReply FormatExtensionGlobalQueuesStatus(bool result)
        {
            return new SetExtensionGlobalQueuesStatusReply
            {
    
                Result = result,

            };
        }
    }
}

