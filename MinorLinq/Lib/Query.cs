namespace MinorLinq.Lib
{
    public class Query
    {
        private string tableName;
        private string[] selects;

        public Query(string tableName, string[] selects)
        {
            this.tableName = tableName;
            this.selects = selects;
        }
    }
}