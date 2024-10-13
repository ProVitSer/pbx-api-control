using TCX.Configuration;
using PbxApiControl.Enums;
using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.Models.Call
{
    public class ActiveConnectionInfoModel
    {
        public int Id  { get; set;}
    
        public int CallConnectionId  { get; set;}
    
        public string ExternalParty { get; set;}
    
        public string RecordingState { get; set;}
    
        public int PartyConnectionId  { get; set;}
    
        public int ReferredBy  { get; set;}
    
        public bool IsOutbound { get; set;}
    
        public bool IsInbound { get; set;}
    
        public string DialedNumber  { get; set;}
    
        public string InternalParty  { get; set;}
    
        public string InternalPartyNumber  { get; set;}

        [EnumDataType(typeof(ConnectionCallStatus))]
        public ConnectionCallStatus ConnectionCallStatus  { get; set;}

        public string DestinationNumber  { get; set;}

        public ActiveConnectionInfoModel(ActiveConnection activeConnection)
        {
            this.Id = activeConnection.ID;
            this.CallConnectionId = activeConnection.CallConnectionID;
            this.ExternalParty = activeConnection.ExternalParty ?? string.Empty;
            this.RecordingState = activeConnection.RecordingState.ToString() ?? string.Empty;
            this.PartyConnectionId = activeConnection.PartyConnectionID;
            this.ReferredBy = activeConnection.ReferredBy;
            this.IsOutbound = activeConnection.IsOutbound;
            this.IsInbound = activeConnection.IsInbound;
            this.DialedNumber = activeConnection.DialedNumber ?? string.Empty;
            this.InternalParty = activeConnection.InternalParty.ToString() ?? string.Empty;
            this.InternalPartyNumber = activeConnection.InternalParty.Number;
            this.ConnectionCallStatus = ConvertActiveConnectionStatus(activeConnection.Status);
            this.DestinationNumber = activeConnection.DN.Number;

        }
    
        public ConnectionCallStatus ConvertActiveConnectionStatus(ConnectionStatus status)
        {

            return status switch
            {
                ConnectionStatus.Undefined => ConnectionCallStatus.CallUndefined,
                ConnectionStatus.Dialing => ConnectionCallStatus.CallDialing,
                ConnectionStatus.Ringing => ConnectionCallStatus.CallRinging,
                ConnectionStatus.Connected => ConnectionCallStatus.CallConnected,
                ConnectionStatus.Hold => ConnectionCallStatus.CallHold,
                ConnectionStatus.Held => ConnectionCallStatus.CallHeld,

                _ => throw new ArgumentOutOfRangeException(nameof(status), $"Неизвестный статус вызова: {status}")
            };
        }
    }
    
    
    
  

}

