namespace PbxApiControl.Models.Sql
{
    public class SqlQueryResultModel
    {
        public List<List<string>> Result { get; set; } = new List<List<string>>();
        public string Error { get; set; }
    }
}

