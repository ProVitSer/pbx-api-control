namespace PbxApiControl.Models.ExtensionReply
{
    public class DeleteExtensionResultReply
    {
        public static DeleteExtensionReply FormatDeleteExtensionResult(bool result)
        {
           return new DeleteExtensionReply
           {
    
               Result = result,
  
           };
        }
    }
}

