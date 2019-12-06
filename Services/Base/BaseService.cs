using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;

namespace Services.Base
{
    public class BaseService : IDisposable
    {
        protected readonly SocializRUnitOfWork unitOfWork;

        public BaseService(SocializRUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

    }
}
