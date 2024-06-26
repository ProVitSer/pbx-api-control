
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace PbxApiControl.Logging
{
    public class LoggingInterceptor: Interceptor
    {
        private readonly ILogger<LoggingInterceptor> _logger;

        public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request, 
            ServerCallContext context, 
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogDebug("Calling Method {method} with argument {argument}", context.Method, request);
            var response = await continuation(request, context);
            _logger.LogDebug("Called Method {method} with result {response}", context.Method, response);
            return response;
        }
    }
}