﻿using TCX.Configuration;
using PbxApiControl.Interface;
using System.Text;
using Newtonsoft.Json;
using PbxApiControl.Models.Call;
using PbxApiControl.Models.CallReply;

namespace PbxApiControl.Services.Pbx
{
    public class PbxEventListenerService : IPbxEventListenerService
    {
        private static readonly object _lock = new object();
        private static PbxEventListenerService _instance = null;
        private readonly ILogger<PbxEventListenerService> _logger;
        private readonly IApiHostSettings _apiHostSettings;
        private readonly HttpClient _httpClient;

        private PbxEventListenerService(ILogger<PbxEventListenerService> logger, IApiHostSettings apiHostSettings, HttpClient httpClient)
        {
            _logger = logger;
            _apiHostSettings = new IApiHostSettings
            {
                Insert = apiHostSettings.Insert,
                Delete = apiHostSettings.Delete,
                Update = apiHostSettings.Update
            };
            _httpClient = httpClient;
        }

        public static PbxEventListenerService GetInstance(ILogger<PbxEventListenerService> logger, IApiHostSettings apiHostSettings, HttpClient httpClient)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new PbxEventListenerService(logger, apiHostSettings, httpClient);
                }
                return _instance;
            }
        }
        
        public void OnStartListenEvent()
        { 
            PhoneSystem.Root.Inserted += ActiveConnectionsInsertedHandler;
            PhoneSystem.Root.Updated += ActiveConnectionsUpdatedHandler;
            PhoneSystem.Root.Deleted += ActiveConnectionsDeletedHandler;
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
                SendPostRequest(activeConnectionsInfo, _apiHostSettings.Insert).ConfigureAwait(false);
            }
        }

        private void ActiveConnectionsUpdatedHandler(object sender, NotificationEventArgs ev)
        {
            var activeConnectionsInfo = ActiveConnectionsInfo();
            if (activeConnectionsInfo.Count > 0)
            {
                SendPostRequest(activeConnectionsInfo, _apiHostSettings.Update).ConfigureAwait(false);
            }
        }

        private void ActiveConnectionsDeletedHandler(object sender, NotificationEventArgs ev)
        {
            var activeConnectionsInfo = ActiveConnectionsInfo();
            if (activeConnectionsInfo.Count > 0)
            {               
                SendPostRequest(activeConnectionsInfo, _apiHostSettings.Delete).ConfigureAwait(false);
            }
        }

        private List<FullActiveConnectionInfoModel> ActiveConnectionsInfo()
        {
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

        private async Task SendPostRequest(List<FullActiveConnectionInfoModel> activeConnectionsInfo, string url)
        {
            try
            {
                var formattedInfo = ActiveConnectionsInfoReply.FormatConnectionsInfoInfo(activeConnectionsInfo);
                
                string jsonData = JsonConvert.SerializeObject(formattedInfo, Formatting.Indented);

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _logger.LogError( "SendPostRequest failed");

            }
        }
    }
}
