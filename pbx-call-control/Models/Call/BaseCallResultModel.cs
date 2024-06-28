namespace PbxApiControl.Models.Call
{
    public class BaseCallResultModel
    {
        public bool Result { get; }

        public string Message { get; }


        internal BaseCallResultModel(bool result, string message)
        {
            this.Result = result;
            this.Message = message;
        }
    }
}

