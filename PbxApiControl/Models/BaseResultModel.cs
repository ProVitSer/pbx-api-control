namespace PbxApiControl.Models;

public class BaseResult
{
    public bool Result { get; }

    public string Message { get; }


    internal BaseResult(bool result, string message)
    {
        this.Result = result;
        this.Message = message;
    }
}

