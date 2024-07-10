using PbxApiControl.Interface;

namespace PbxApiControl.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _whitelistedIPs;
        private readonly ITokenValidationService _tokenValidationService;

        public AuthMiddleware(RequestDelegate next, ITokenValidationService tokenValidationService)
        {
            _next = next;
            _whitelistedIPs = new[]{ "::ffff:127.0.0.1", "127.0.0.1" };;
            _tokenValidationService = tokenValidationService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Проверка IP адреса
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();

            if (remoteIp != null && !_whitelistedIPs.Contains(remoteIp))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("IP адрес не в белом списке");
                return;
            }

            // Проверка Bearer токена
            if (!context.Request.Headers.ContainsKey("Authorization") ||
                !context.Request.Headers["Authorization"].ToString().StartsWith("Bearer "))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Требуется авторизация Bearer токена");
                return;
            }

            var token = context.Request.Headers["Authorization"].ToString().Split(' ')[1];

            var isValid = _tokenValidationService.ValidateToken(token);

            if (!isValid)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Неверный Bearer токен");
                return;
            }

            await _next(context);
        }
    }
}

