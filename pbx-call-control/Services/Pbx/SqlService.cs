using Npgsql;
using System.Data;
using System.Text;
using PbxApiControl.Interface;
using PbxApiControl.Config;
using PbxApiControl.Models.Sql;

namespace PbxApiControl.Services.Pbx
{
    public class SqlService: ISqlService
    {
        private readonly ILogger<SqlService> _logger;
        


        public SqlService(ILogger<SqlService> logger)
        {
            _logger = logger;
        }

        public async Task<SqlQueryResultModel> ExecuteSqlQueryAsync(string sqlQuery)
        {
            string connectionString = GetDbConnectionString();

            using var connection = new NpgsqlConnection(connectionString);
            
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(sqlQuery, connection);
            
            using var reader = await command.ExecuteReaderAsync();

            var result = new SqlQueryResultModel();
    
            var dataTable = new DataTable();
            
            dataTable.Load(reader);

            foreach (DataRow row in dataTable.Rows)
            {
                var rowData = new List<string>();
                
                foreach (var item in row.ItemArray)
                {
                    rowData.Add(item.ToString());
                }
                
                result.Result.Add(rowData);
            }

            return result;
        }
        
        private string GetDbConnectionString()
        {
            return $"Host={PbxApiConfig.DbHost};Port={PbxApiConfig.DbPort};Database={PbxApiConfig.DbName};Username={PbxApiConfig.DbUser};Password={PbxApiConfig.DbPassword}";
        }
        private string ConvertDataTableToString(DataTable dataTable)
        {
            var result = new StringBuilder();
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    result.Append(item.ToString() + "\t");
                }
                result.AppendLine();
            }
            return result.ToString();
        }
    
    }
}

