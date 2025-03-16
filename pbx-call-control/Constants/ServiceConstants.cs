namespace PbxApiControl.Constants
{
    public static class ServiceConstants
    {
        public const string ExtensionNotFound = "Extension number not found";
        public const string ExtensionExists = "Extension already exists on PBX";
        public const string DnIsNotExten = "Number is not in the list of internal numbers";
        public const string DnIsNotRingGroup = "Number is not in the list of ring group numbers";
        public const string RingGroupNotFound = "Ring group number not found";
        public const string TryParseError =
            "The input string has an incorrect format or represents a number that is too large or too small for Int32.";
        public const string ContactIdNotFound = "Contact with the given Id not found";
        public const string DnIsNotQueue = "Number is not in the list of queue numbers";
        public const string QueueNotFound = "Queue number not found";
        public const string CallInitSuccess = "Call successfully initialized";
        public const string CallInitError = "Call initialization issues";
        public const string CallDropSuccess = "Call successfully completed";
        public const string CallDropError = "Call termination issues";
        public const string DestinationNumberNotFound = "The internal number to which the call needs to be transferred does not exist";
        public const string DestinationNumberUnregister = "The internal number to which the call needs to be transferred is not registered";
        public const string CallTransferSuccess = "Call successfully transferred";
        public const string CallTransferError = "Call transfer issues";
        public const string NoActiveConnection = "No active calls for the number";
        public const string DataError = "Invalid value provided";
    }
}