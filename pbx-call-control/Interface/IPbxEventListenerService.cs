namespace PbxApiControl.Interface
{
    public interface IPbxEventListenerService
    {
        void StartListenInsertedEvent(string url);
        void StartListenUpdatedEvent(string url);
        void StartListenDeletedEvent(string url);
        void StopListenInsertedEvent();
        void StopListenUpdatedEvent();
        void StopListenDeletedEvent();

    } 
}