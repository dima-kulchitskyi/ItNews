using ItNews.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryProvider
{
    public class MemoryProviderUnitOfWork : IUnitOfWork
    {
        public bool IsActive { get; private set; }

        public bool IsCommited { get; private set; }

        public bool IsRolledBack => false;

        public void BeginTransaction()
        {
            IsActive = true;
        }

        public void Commit()
        {
            IsActive = false;
            IsCommited = true;
        }

        public void Dispose()
        {
            IsActive = false;
        }

        public void Rollback()
        {
            IsActive = false;
        }
    }
}
