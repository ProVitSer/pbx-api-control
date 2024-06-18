namespace PbxApiControl.Models.Queue;

public class QueueDataModel
{
    public string QueueNumber { get; }
    public string[] QueueAgents { get; }
    
    
    public QueueDataModel(string queueNumber, string[] queueAgents)
    {
        QueueNumber = queueNumber;
        QueueAgents = queueAgents;
    }
}