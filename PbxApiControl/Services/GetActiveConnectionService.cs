using System.Collections.Generic;
using System.Text.Json.Serialization;
using TCX.Configuration;
using PbxApiControl.Enums;
using PbxApiControl.Models;
using PbxApiControl.Interface;


public class GetActiveConnectionsService : IGetActiveConnectionService
{
    public int CountCalls => Calls.Count;

    [JsonPropertyName("calls")]
    public List<CallState> Calls { get; } = new List<CallState>();

    public GetActiveConnectionsService()
    {
        foreach (var keyValuePair in PhoneSystem.Root.GetActiveConnectionsByCallID())
        {
            CallState callState = CallState(keyValuePair.Key);
            Calls.Add(callState);

            foreach (var activeConnection in keyValuePair.Value)
            {
                ProcessActiveConnection(callState, activeConnection);
            }
        }
    }

    private CallState CallState(uint key)
    {
        return new CallState(key);
    }

    private void ProcessActiveConnection(CallState callState, ActiveConnection activeConnection)
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

    private void DetermineCallDirection(CallState callState, ActiveConnection activeConnection)
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

    private void UpdateAConnStateStatus(CallState callState, ActiveConnection activeConnection)
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

    private void UpdateStatus(CallState callState, ConnectionStatus status)
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