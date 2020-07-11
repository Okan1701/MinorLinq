namespace MinorLinq.Lib
{
    public interface IDbDriver
    {
        void OpenConnection(string connection);
        void CloseConnection();
    }
}