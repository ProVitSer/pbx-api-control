using TCX.Configuration;
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
    private readonly HttpClient _httpClient;

    private string _insertedUrl;
    private string _updatedUrl;
    private string _deletedUrl;

    public static PbxEventListenerService GetInstance(ILogger<PbxEventListenerService> logger, HttpClient httpClient)
    {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = new PbxEventListenerService(logger, httpClient);
            }
            return _instance;
        }
    }

    private PbxEventListenerService(ILogger<PbxEventListenerService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public void StartListenInsertedEvent(string url)
    {
        lock (_lock)
        {
            _insertedUrl = url;
            PhoneSystem.Root.Inserted += ActiveConnectionsInsertedHandler;
        }
    }

    public void StartListenUpdatedEvent(string url)
    {
        lock (_lock)
        {
            _updatedUrl = url;
            PhoneSystem.Root.Updated += ActiveConnectionsUpdatedHandler;
        }
    }

    public void StartListenDeletedEvent(string url)
    {
        lock (_lock)
        {
            _deletedUrl = url;
            PhoneSystem.Root.Deleted += ActiveConnectionsDeletedHandler;
        }
    }

    public void StopListenInsertedEvent()
    {
        lock (_lock)
        {
            PhoneSystem.Root.Inserted -= ActiveConnectionsInsertedHandler;
            _insertedUrl = null; 
        }
    }

    public void StopListenUpdatedEvent()
    {
        lock (_lock)
        {
            PhoneSystem.Root.Updated -= ActiveConnectionsUpdatedHandler;
            _updatedUrl = null; 
        }
    }

    public void StopListenDeletedEvent()
    {
        lock (_lock)
        {
            PhoneSystem.Root.Deleted -= ActiveConnectionsDeletedHandler;
            _deletedUrl = null;
        }
    }

    private void ActiveConnectionsInsertedHandler(object sender, NotificationEventArgs ev)
    {
        var activeConnectionsInfo = ActiveConnectionsInfo();
        if (activeConnectionsInfo.Count > 0 && !string.IsNullOrEmpty(_insertedUrl))
        {
            SendPostRequest(activeConnectionsInfo, _insertedUrl).ConfigureAwait(false);
        }
    }

    private void ActiveConnectionsUpdatedHandler(object sender, NotificationEventArgs ev)
    {
        var activeConnectionsInfo = ActiveConnectionsInfo();
        if (activeConnectionsInfo.Count > 0 && !string.IsNullOrEmpty(_updatedUrl))
        {
            SendPostRequest(activeConnectionsInfo, _updatedUrl).ConfigureAwait(false);
        }
    }

    private void ActiveConnectionsDeletedHandler(object sender, NotificationEventArgs ev)
    {
        var activeConnectionsInfo = ActiveConnectionsInfo();
        if (activeConnectionsInfo.Count > 0 && !string.IsNullOrEmpty(_deletedUrl))
        {
            SendPostRequest(activeConnectionsInfo, _deletedUrl).ConfigureAwait(false);
        }
    }

    private List<FullActiveConnectionInfoModel> ActiveConnectionsInfo()
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
            _logger.LogError($"SendPostRequest failed: {e.Message}");
        }
    }
}

}
