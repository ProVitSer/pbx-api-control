
namespace PbxApiControl.Models;
public struct QueueAgents
{
    public string Extension { get; internal set; }
    public string FirstName { get; internal set; }
    public string LastName { get; internal set; }
    public bool LoggedIn { get; internal set; }
}