namespace MinorLinq.Lib
{
    public abstract class DataContext : IDataContext
    {
        protected IDbConnectionHandler dbConnectionHandler;

        public DataContext()
        {
            OnInit();
        }

        private void OnInit()
        {
            OnEntityRegister();
        }

        protected virtual void OnEntityRegister()
        {
            foreach (var property in this.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(IDbTable))
                {
                    IDbTable entity = (IDbTable)property.GetValue(this);
                    entity.SetAssignedContext(this);
                }
            }
        }


    }
}