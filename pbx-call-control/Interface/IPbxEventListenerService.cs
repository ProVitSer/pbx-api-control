namespace PbxApiControl.Interface
{
    public interface IPbxEventListenerService
    {
        void OnStartListenEvent();
        void StartListenInsertedEvent();
        void StartListenUpdatedEvent();
        void StartListenDeletedEvent();
        void StopListenInsertedEvent();
        void StopListenUpdatedEvent();
        void StopListenDeletedEvent();

    } 
}