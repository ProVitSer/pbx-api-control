using PbxApiControl.Interface;
using Grpc.Core;
using System.Net;

namespace PbxApiControl.Middleware
{
    public class AuthMiddleware : IMiddleware
    {
        private readonly string[] _whitelistedIPsOrDomains;
        private readonly ITokenValidationService _tokenValidationService;
        private readonly ILogger<AuthMiddleware> _logger;

        public AuthMiddleware(ILogger<AuthMiddleware> logger,ITokenValidationService tokenValidationService, IConfiguration configuration)
        {
            _logger = logger;
            _whitelistedIPsOrDomains = configuration.GetSection("WhitelistedIPs").Get<string[]>();
            _tokenValidationService = tokenValidationService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                var remoteIp = context.Connection.RemoteIpAddress;
                if (remoteIp != null && remoteIp.IsIPv4MappedToIPv6)
                {
                    remoteIp = remoteIp.MapToIPv4();
                }

                var remoteIpString = remoteIp?.ToString();

                bool isWhitelisted = false;
                
                foreach (var entry in _whitelistedIPsOrDomains)
                {
                    if (IPAddress.TryParse(entry, out var whitelistedIp))  
                    {
                        if (whitelistedIp.ToString() == remoteIpString)
                        {
                            isWhitelisted = true;
                            break;
                        }
                    }
                    else  
                    {
                        try
                        {
                            var hostEntry = await Dns.GetHostEntryAsync(entry);  
                            if (hostEntry.AddressList.Any(ip => ip.MapToIPv4().ToString() == remoteIpString))
                            {
                                isWhitelisted = true;
                                break;
                            }
                        }
                        catch (Exception dnsEx)
                        {
                            _logger.LogWarning($"Failed to resolve domain {entry}: {dnsEx.Message}");
                        }
                    }
                }


                if (!isWhitelisted)
                {

                    throw new RpcException(new Status(StatusCode.PermissionDenied, "IP address is not in the white list"));
                }
                
                //_tokenValidationService.GenerateToken(); // Generate token for testing purposes in console

                if (!context.Request.Headers.ContainsKey("Authorization") ||
                    !context.Request.Headers["Authorization"].ToString().StartsWith("Bearer "))
                {
                    throw new RpcException(new Status(StatusCode.Unauthenticated, "Authorization required"));
                }

                // Extract and validate token
                var token = context.Request.Headers["Authorization"].ToString().Split(' ')[1];
                
                var isValid = _tokenValidationService.ValidateToken(token);
                
                if (!isValid)
                {
                    context.Response.StatusCode = (int)StatusCode.Unauthenticated; 
                    await context.Response.WriteAsync("Invalid token");
                    return; 
                }

                await next(context);

            }    
            catch (RpcException rpcEx)
            {
                _logger.LogError($"RpcException: {rpcEx.Status.Detail}");
                context.Response.StatusCode = (int)rpcEx.StatusCode;
                await context.Response.WriteAsync(rpcEx.Status.Detail);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Internal server error");
            }
        }
    }
}