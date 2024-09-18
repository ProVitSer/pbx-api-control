using PbxApiControl.Models.Sql;

namespace PbxApiControl.Interface
{
    public interface ISqlService
    {
        Task<SqlQueryResultModel> ExecuteSqlQueryAsync(string sqlQuery);
    }
}

