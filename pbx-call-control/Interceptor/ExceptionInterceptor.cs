

namespace PbxApiControl.Interceptor
{
    using Grpc.Core;
    using Grpc.Core.Interceptors;
    using System.Threading.Tasks;
    
    public class ExceptionInterceptor  : Interceptor
    {
        private readonly ILogger<ExceptionInterceptor> _logger;

        public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
        {
            _logger = logger;
        }
    
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation("Monitoring grpc pipeline for exceptions");
            try
            {
                return await continuation(request, context);
            }

            catch (RpcException ex)
            {
                _logger.LogError($"RpcException: {ex.Status.Detail}");
                throw new RpcException(new Status(ex.StatusCode, ex.Message));

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unknown exception detected, throwing");
                throw;
            }
        }
    }
}


