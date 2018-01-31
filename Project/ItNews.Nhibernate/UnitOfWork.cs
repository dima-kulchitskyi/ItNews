using NHibernate;
using ItNews.Business;
using System;
using System.Threading.Tasks;

namespace ItNews.Nhibernate
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        protected ITransaction transaction;

        protected SessionContainer sessionContainer;

        public bool IsActive => transaction?.IsActive ?? false;

        public bool IsCommited => transaction?.WasCommitted ?? false;

        public bool IsRolledBack => transaction?.WasRolledBack ?? false;

        public UnitOfWork()
        {
            sessionContainer = SessionContainer.Open();
        }

        public void BeginTransaction()
        {
            if (transaction == null || !transaction.IsActive)
                transaction = sessionContainer.Session.BeginTransaction();
        }

        public void Commit()
        {
            if (transaction == null || !transaction.IsActive)
                return;

            try
            {
                transaction.Commit();
            }
            catch
            {
                Rollback();
            }
            finally
            {
                sessionContainer?.Dispose();
            }
        }

        public void Rollback()
        {
            if (transaction == null || !transaction.IsActive)
                return;

            try
            {
                transaction.Rollback();
            }
            finally
            {
                sessionContainer?.Dispose();
            }
        }

        public void Dispose()
        {
            if (transaction != null && transaction.IsActive)
            {
                Rollback();

                throw new InvalidOperationException("Transaction is still active, maybe you forgot to commit it?");
            }

            sessionContainer.Dispose();
        }
    }
}