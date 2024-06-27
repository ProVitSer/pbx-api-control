using TCX.Configuration;
using PbxApiControl.Interface;
using System.Text;
using Newtonsoft.Json;
using PbxApiControl.Models.Call;

namespace PbxApiControl.Services.Pbx
{
    public class PbxEventListenerService : IPbxEventListenerService
    {
        private static readonly object _lock = new object();
        private static PbxEventListenerService _instance = null;
        private readonly ILogger<PbxEventListenerService> _logger;
        private readonly string _url = "https://webhook.site/9427be0a-3aab-4f82-ac30-becb6b84b43c";

        private PbxEventListenerService(ILogger<PbxEventListenerService> logger)
        {
            _logger = logger;
        }

        public static PbxEventListenerService GetInstance(ILogger<PbxEventListenerService> logger)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new PbxEventListenerService(logger);
                }
                return _instance;
            }
        }

        public void StartListenInsertedEvent()
        {
            lock (_lock)
            {
                PhoneSystem.Root.Inserted += ActiveConnectionsInsertedHandler;
            }
        }

        public void StartListenUpdatedEvent()
        {
            lock (_lock)
            {
                PhoneSystem.Root.Updated += ActiveConnectionsUpdatedHandler;
            }
        }

        public void StartListenDeletedEvent()
        {
            lock (_lock)
            {
                PhoneSystem.Root.Deleted += ActiveConnectionsDeletedHandler;
            }
        }

        public void StopListenInsertedEvent()
        {
            lock (_lock)
            {
                PhoneSystem.Root.Inserted -= ActiveConnectionsInsertedHandler;
            }
        }

        public void StopListenUpdatedEvent()
        {
            lock (_lock)
            {
                PhoneSystem.Root.Updated -= ActiveConnectionsUpdatedHandler;
            }
        }

        public void StopListenDeletedEvent()
        {
            lock (_lock)
            {
                PhoneSystem.Root.Deleted -= ActiveConnectionsDeletedHandler;
            }
        }

        private void ActiveConnectionsInsertedHandler(object sender, NotificationEventArgs ev)
        {
            var activeConnectionsInfo = ActiveConnectionsInfo();
            if (activeConnectionsInfo.Count > 0)
            {
                SendPostRequest(activeConnectionsInfo, _url);
                activeConnectionsInfo.Clear();
            }
        }

        private void ActiveConnectionsUpdatedHandler(object sender, NotificationEventArgs ev)
        {
            var activeConnectionsInfo = ActiveConnectionsInfo();
            if (activeConnectionsInfo.Count > 0)
            {
                SendPostRequest(activeConnectionsInfo, _url);
                activeConnectionsInfo.Clear();
            }
        }

        private void ActiveConnectionsDeletedHandler(object sender, NotificationEventArgs ev)
        {
            var activeConnectionsInfo = ActiveConnectionsInfo();
            if (activeConnectionsInfo.Count > 0)
            {
                SendPostRequest(activeConnectionsInfo, _url);
                activeConnectionsInfo.Clear();
            }
        }

        private List<FullActiveConnectionInfoModel> ActiveConnectionsInfo()
        {
            lock (_lock)
            {
                var activeConnectionsInfo = new List<FullActiveConnectionInfoModel>();
                foreach (var keyValuePair in PhoneSystem.Root.GetActiveConnectionsByCallID())
                {
                    var activeConnectionInfo = new FullActiveConnectionInfoModel(keyValuePair.Key);
                    activeConnectionsInfo.Add(activeConnectionInfo);
                    foreach (var activeConnection in keyValuePair.Value)
                    {
                        activeConnectionInfo.AddActiveConnectionInfo(activeConnection);
                    }
                }
                return activeConnectionsInfo;
            }
        }

        private async void SendPostRequest(List<FullActiveConnectionInfoModel> activeConnectionInfo, string url)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(activeConnectionInfo, Formatting.Indented);
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                }
            }
            catch (Exception e)
            {
                _logger.LogDebug("SendPostRequest: {@e}", e.ToString());
            }
        }
    }
}