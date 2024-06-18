using TCX.Configuration;


namespace PbxApiControl.Models.Queue;
public class QueueAgentsDataModels
{
    public string Extension { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public bool LoggedIn { get;  }
    
    
    public QueueAgentsDataModels(Extension extension,  QueueAgent agent)
    {
        Extension = extension.Number;
        FirstName = extension.FirstName;
        LastName = extension.LastName;
        LoggedIn = agent.QueueStatus == QueueStatusType.LoggedIn;
    }
}