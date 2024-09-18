using Grpc.Core;
using PbxApiControl.Interface;
using System.Text.Json;
using PbxApiControl.Models.Sql;

namespace PbxApiControl.Services.Grpc
{
    public class SqlService : SqlServicePbxService.SqlServicePbxServiceBase
    {
        private readonly ILogger<RingGroupService> _logger;
        private readonly ISqlService _sqlService;

        public SqlService(ILogger<RingGroupService> logger, ISqlService SqlService)
        {
            _logger = logger;
            _sqlService = SqlService;
        }

        public override async Task<SqlResponse> ExecuteSql(SqlRequest request, ServerCallContext context) 
        {
            
            var response = new SqlResponse();
            var sqlQueryResult = new SqlQueryResultModel();

 
            try
            {
                _logger.LogDebug(request.Query);

                sqlQueryResult = await _sqlService.ExecuteSqlQueryAsync(request.Query);

                if (!string.IsNullOrEmpty(sqlQueryResult.Error))
                {
                    response.Result = JsonSerializer.Serialize(sqlQueryResult.Result);
                    return response; 
                }

                response.Result = JsonSerializer.Serialize(sqlQueryResult.Result);
            }
            catch (Exception e)
            {
                _logger.LogError("ExecuteSql: {@e}", e.ToString());
                response.Error = e.ToString();
                response.Result = JsonSerializer.Serialize(sqlQueryResult.Result);
            }

            return response;
        }
    }
    


}

