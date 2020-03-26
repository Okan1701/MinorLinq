namespace MinorLinq.Lib
{
    public interface IDbConnectionHandler
    {
        void OpenConnection();
        void CloseConnection();
        void ToSql();
        void ExecuteSql();
        void GetResultsFromQuery();
    }
}