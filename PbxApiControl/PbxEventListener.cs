using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TCX.Configuration;
using PbxApiControl.Enums;
using PbxApiControl.Models;
using Newtonsoft.Json;

public static class PbxEventListener
{
    private static object lockH = new object();

    private static List<CallState> Calls { get; } = new List<CallState>();

    private static string url = "https://webhook.site/69ce8e35-f84f-4a46-aae8-3afa3b3a784f";


    public static void Start()
    {
        PhoneSystem.Root.Updated += new NotificationEventHandler(ActiveConnectionsHandler);
        PhoneSystem.Root.Inserted += new NotificationEventHandler(ActiveConnectionsHandler);
        PhoneSystem.Root.Deleted += new NotificationEventHandler(ActiveConnectionsHandlerDeleted);
    }


    static void ActiveConnectionsHandlerDeleted(object sender, NotificationEventArgs ev)
    {
        lock (PbxEventListener.lockH)
        {

            Dictionary<uint, List<ActiveConnection>> connectionsByCallId = PhoneSystem.Root.GetActiveConnectionsByCallID();

            foreach (KeyValuePair<uint, List<ActiveConnection>> keyValuePair in connectionsByCallId)
            {

                CallState callState = CallState(keyValuePair.Key);
                Calls.Add(callState);

                foreach (var activeConnection in keyValuePair.Value)
                {

                    callState.Status = CallStatus.Finish;
                    DetermineCallDirection(callState, activeConnection);
                }

                SendPostRequest(Calls);
                Calls.Clear();

            }
        }
    }

    static void ActiveConnectionsHandler(object sender, NotificationEventArgs ev)
    {
        lock (PbxEventListener.lockH)
        {

            Dictionary<uint, List<ActiveConnection>> connectionsByCallId = PhoneSystem.Root.GetActiveConnectionsByCallID();

            foreach (KeyValuePair<uint, List<ActiveConnection>> keyValuePair in connectionsByCallId)
            {

                CallState callState = CallState(keyValuePair.Key);
                Calls.Add(callState);

                foreach (var activeConnection in keyValuePair.Value)
                {
                    ProcessActiveConnection(callState, activeConnection);
                }

                SendPostRequest(Calls);
                Calls.Clear();

            }
        }
    }

    static async Task SendPostRequest(List<CallState> Calls)
    {
        string jsonData = JsonConvert.SerializeObject(Calls, Formatting.Indented);

        using (HttpClient httpClient = new HttpClient())
        {
            try
            {

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(url, content);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    private static CallState CallState(uint key)
    {
        return new CallState(key);
    }

    private static void ProcessActiveConnection(CallState callState, ActiveConnection activeConnection)
    {
        if (callState.CallDirection == Direction.Undefined)
        {
            DetermineCallDirection(callState, activeConnection);
        }
        else
        {
            UpdateAConnStateStatus(callState, activeConnection);
        }
    }

    private static void DetermineCallDirection(CallState callState, ActiveConnection activeConnection)
    {
        if (activeConnection.ExternalParty.Length > 5)
        {
            callState.RemoteNumber = activeConnection.ExternalParty;

            if (activeConnection.DN is ExternalLine)
            {
                callState.CallDirection = Direction.Inbound;
            }
            else
            {
                callState.CallDirection = Direction.Outbound;
                callState.LocalNumber = activeConnection.DN.Number;
            }
        }
        else
        {
            callState.RemoteNumber = activeConnection.ExternalParty;
            callState.CallDirection = Direction.Local;
            callState.LocalNumber = activeConnection.DN.Number;
        }
    }

    private static void UpdateAConnStateStatus(CallState callState, ActiveConnection activeConnection)
    {
        switch (callState.CallDirection)
        {
            case Direction.Outbound:
            case Direction.Local:
                UpdateStatus(callState, activeConnection.Status);
                break;
            case Direction.Inbound:
                callState.LocalNumber = activeConnection.DN.Number;
                UpdateStatus(callState, activeConnection.Status);
                break;
        }
    }

    private static void UpdateStatus(CallState callState, ConnectionStatus status)
    {
        switch (status)
        {
            case ConnectionStatus.Dialing:
                callState.Status = CallStatus.Dialing;
                break;
            case ConnectionStatus.Ringing:
                callState.Status = CallStatus.Ringing;
                break;
            case ConnectionStatus.Connected:
                callState.Status = CallStatus.Talking;
                break;
            default:
                callState.Status = CallStatus.Other;
                break;
        }
    }



}

