namespace PbxApiControl.Models;

public class MakeCall
{
    public string To { get; set; }
    public string From { get; set; }
    public bool Result { get; set; }
    public string Message { get; set; }
    
    public MakeCall(string to, string from, bool result, string message)
    {
        To = to;
        From = from;
        Result = result;
        Message = message;
    }
}