using ItNews.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.FileProvider
{
    public class FileProviderUnitOfWork : IUnitOfWork
    {
        public bool IsActive { get; private set; }

        public bool IsCommited { get; private set; }

        public bool IsRolledBack => false;

        public void BeginTransaction()
        {
            IsActive = true;
        }

        public Task Commit()
        {
            IsActive = false;
            IsCommited = true;

            return Task.CompletedTask;
        }

        public Task Rollback()
        {
            IsActive = false;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            IsActive = false;
        }
    }
}
