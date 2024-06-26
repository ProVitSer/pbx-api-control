using PbxApiControl.Enums;
using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.Models.Call
{
    public class CallStateModel
    {
        public uint CallID { get; }
    
        [EnumDataType(typeof(Direction))]
        public Direction CallDirection { get; set;}
    
        [EnumDataType(typeof(CallStatus))]
        public CallStatus Status { get; set; }

        public string? Direction  { get; }
    
        public string? CallStatus  { get; }
    
        public string? LocalNumber { get; set; }
    
        public string? RemoteNumber { get; set;}
    
        internal CallStateModel(uint callid)
        {
            this.CallID = callid;
        }
    
    }

}

