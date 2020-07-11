using System;

namespace MinorLinq.Lib
{
    public abstract class DataContext : IDataContext, IDisposable
    {
        protected IDbDriver dbDriver;
        
        public bool Disposed { get; set; } = false;

        public DataContext()
        {
            OnInit();
        }

        public void Dispose() => Dispose(true);

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

        protected virtual void Dispose(bool disposing) 
        {
            if (Disposed) return;
            if (disposing) 
            {
                dbDriver.CloseConnection();
                Disposed = true;
            }
        }
    }
}