namespace PbxApiControl.Models.Reply
{
    public class ExtensionStatusInQueueReply
    {
        public static SetExtensionStatusInQueueReply FormatExtensionStatusInQueue(bool result)
        {
            return new SetExtensionStatusInQueueReply
            {
                Result = result
            };
        }
    }
}

