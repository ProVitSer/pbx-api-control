﻿using PbxApiControl.Interface;
using Grpc.Core;

namespace PbxApiControl.Middleware
{
    public class AuthMiddleware : IMiddleware
    {
        private readonly string[] _whitelistedIPs;
        private readonly ITokenValidationService _tokenValidationService;
        private readonly ILogger<AuthMiddleware> _logger;

        public AuthMiddleware(ILogger<AuthMiddleware> logger,ITokenValidationService tokenValidationService, IConfiguration configuration)
        {
            _logger = logger;
            _whitelistedIPs = configuration.GetSection("WhitelistedIPs").Get<string[]>();
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

                if (remoteIpString != null && !_whitelistedIPs.Contains(remoteIpString))
                {

                    throw new RpcException(new Status(StatusCode.PermissionDenied, "IP адрес не в белом списке"));
                }

                if (!context.Request.Headers.ContainsKey("Authorization") ||
                    !context.Request.Headers["Authorization"].ToString().StartsWith("Bearer "))
                {
                    throw new RpcException(new Status(StatusCode.Unauthenticated, "Требуется авторизация"));
                }

                // Extract and validate token
                var token = context.Request.Headers["Authorization"].ToString().Split(' ')[1];
                var isValid = _tokenValidationService.ValidateToken(token);
                if (!isValid)
                {
                    context.Response.StatusCode = (int)StatusCode.Unauthenticated; 
                    await context.Response.WriteAsync("Неверный токен");
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