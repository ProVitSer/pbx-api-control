using PbxApiControl.Models.Queue;

namespace PbxApiControl.Models.QueueReply;

public class QueueAgentsInfoReply
{
    public static QueueInfo FormatQueueInfo(QueueAgentsDataModels data)
    {
        return new QueueInfo
        {
            Extension = data.Extension,
            FirstName = data.FirstName,
            LastName = data.LastName,
            LoggedIn = data.LoggedIn
        };
    }
}