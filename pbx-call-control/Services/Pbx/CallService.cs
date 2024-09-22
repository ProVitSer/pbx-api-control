using PbxApiControl.Interface;
using PbxApiControl.Models.Call;
using PbxApiControl.Enums;
using TCX.Configuration;
using PbxApiControl.Constants;

namespace PbxApiControl.Services.Pbx
{
    public class CallService : ICallService
    {
        private readonly ILogger<CallService> _logger;
        private readonly IExtensionService _extensionService;
        private readonly ILogUtilService _logUtilService;

        private const  int MaxLocalExtLength = 5;
        public CallService(ILogger<CallService> logger, IExtensionService extensionService, ILogUtilService logUtilService)
        {
            _logger = logger;
            _extensionService = extensionService;
            _logUtilService = logUtilService;
        }

        public int CountCalls()
        {
            return PhoneSystem.Root.GetActiveConnectionsByCallID().Count;
        }

        public BaseCallResultModel MakeCall(string to, string from)
        {
            try
            {
                
                PhoneSystem.Root.MakeCall(from, to);
                
                return new BaseCallResultModel(true, ServiceConstants.CallInitSuccess);
            }
            catch (Exception e)
            {
                _logger.LogDebug($"Error Make call: {e}");

                return new BaseCallResultModel(false, ServiceConstants.CallInitError);
            }
        }

        public BaseCallResultModel HangupCall(string extension)
        {
            try
            {
                using var dnByNumber = PhoneSystem.Root.GetDNByNumber(extension);
                if (dnByNumber is not Extension)
                {
                    throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
                }

                using var disposer = dnByNumber.GetActiveConnections().GetDisposer<ActiveConnection>();
                foreach (var activeConnection in disposer)
                {
                    if (activeConnection.Status == ConnectionStatus.Connected)
                    {
                        activeConnection.Drop();
                    }
                }

                return new BaseCallResultModel(true, ServiceConstants.CallDropSuccess);
            }
            catch (InvalidOperationException e)
            {
                
                _logger.LogDebug($"Invalid operation Hangup call: {e}");
                
                return new BaseCallResultModel(false, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogDebug($"Error Hangup call: {e}");
                
                return new BaseCallResultModel(false, ServiceConstants.CallDropError);
            }
        }

        public BaseCallResultModel TransferCallByCallId(uint callId, string dn, string numberTo)
        {
            try
            {
                var activeConnections = PhoneSystem.Root.GetActiveConnectionsByCallID();

                if (!activeConnections.TryGetValue(callId, out var connections))
                {
                    throw new InvalidOperationException(ServiceConstants.NoActiveConnection);
                }

                PhoneSystem.Root.TransferCall((int)callId, dn, numberTo);
                
                return new BaseCallResultModel(true, ServiceConstants.CallTransferSuccess);
            }
            catch (InvalidOperationException e)
            {
                
                _logger.LogDebug($"Invalid operation transferring call: {e}");
                
                return new BaseCallResultModel(false, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogDebug($"Error transferring call: {e}");
                
                return new BaseCallResultModel(false, e.ToString());
            }
        }

        private BaseCallResultModel TransferCall(string extension, string destinationNumber)
        {
            try
            {
                using var dnByNumber = PhoneSystem.Root.GetDNByNumber(extension);
                if (dnByNumber is not Extension)
                {
                    throw new InvalidOperationException(ServiceConstants.DnIsNotExten);
                }

                if (destinationNumber.Length < 5)
                {
                    if (!_extensionService.IsExtensionExists(destinationNumber))
                    {
                        throw new InvalidOperationException(ServiceConstants.ExtensionNotFound);
                    }

                    if (!IsExtensionRegister(destinationNumber))
                    {
                        throw new InvalidOperationException(ServiceConstants.DestinationNumberUnregister);
                    }
                }

                TransferCallToDestNumber(extension, destinationNumber);
                
                return new BaseCallResultModel(true, ServiceConstants.CallTransferSuccess);
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.ToString());
                return new BaseCallResultModel(false, e.ToString());
            }
        }

        public List<CallStateModel> ActiveCallsInfo()
        {
            try
            {
                var callsState = new List<CallStateModel>();

                foreach (var keyValuePair in PhoneSystem.Root.GetActiveConnectionsByCallID())
                {
                    var callState = new CallStateModel(keyValuePair.Key);
                    
                    callsState.Add(callState);

                    foreach (var activeConnection in keyValuePair.Value)
                    {
                        ProcessActiveConnection(callState, activeConnection);
                    }
                }

                _logUtilService.LogCallsState(callsState);
                
                return callsState;
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.ToString());
                throw;
            }
        }

        public List<FullActiveConnectionInfoModel> FullActiveConnectionsInfo()
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

        private void ProcessActiveConnection(CallStateModel callState, ActiveConnection activeConnection)
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

        private void DetermineCallDirection(CallStateModel callState, ActiveConnection activeConnection)
        {
            if (activeConnection.ExternalParty.Length > MaxLocalExtLength)
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

        private void UpdateAConnStateStatus(CallStateModel callState, ActiveConnection activeConnection)
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

        private void UpdateStatus(CallStateModel callState, ConnectionStatus status)
        {
            callState.Status = status switch
            {
                ConnectionStatus.Dialing => CallStatus.Dialing,
                ConnectionStatus.Ringing => CallStatus.Ringing,
                ConnectionStatus.Connected => CallStatus.Talking,
                _ => CallStatus.Other,
            };
        }

        private void TransferCallToDestNumber(string extension, string destinationNumber)
        {
            var activeConnectionA = GetActiveConnectionByNumber(extension);
            var externalParty = activeConnectionA.ExternalParty;
            var activeConnectionB = GetActiveConnectionByNumber(externalParty);

            if (externalParty != extension)
            {
                PhoneSystem.Root.TransferCall(activeConnectionB, destinationNumber);
            }
            else
            {
                PhoneSystem.Root.TransferCall(activeConnectionA, destinationNumber);
            }
        }

        private ActiveConnection GetActiveConnectionByNumber(string extension)
        {
            using var dnByNumber = PhoneSystem.Root.GetDNByNumber(extension);
            using var disposer = dnByNumber.GetActiveConnections().GetDisposer<ActiveConnection>();

            var allTakenConnections = disposer.ToDictionary(x => x, y => y.OtherCallParties);
            var owner = allTakenConnections.Keys.FirstOrDefault();

            if (owner == null)
            {
                throw new InvalidOperationException(ServiceConstants.NoActiveConnection);
            }

            return owner;
        }

        private static bool IsExtensionRegister(string extension)
        {
            using var dnByNumber = PhoneSystem.Root.GetDNByNumber(extension);
            return dnByNumber.IsRegistered;
        }

  
    }
}




