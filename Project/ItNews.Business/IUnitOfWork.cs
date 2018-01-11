using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Rollback();
        void Commit();
        bool IsActive { get; }
        bool IsCommited { get; }
        bool IsRolledBack { get;  }
    }
}
