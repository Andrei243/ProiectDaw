using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace DataAccess.Base
{
    public class BaseUnitOfWork : IUnitOfWork, IDisposable
    {
        protected readonly SocializRContext DbContext;
        public BaseUnitOfWork(SocializRContext context)
        {
            DbContext = context;
        }
        public void Dispose()
        {
            DbContext.Dispose();
        }

        public int SaveChanges()
        { 
            return DbContext.SaveChanges();
        }
    }
}
