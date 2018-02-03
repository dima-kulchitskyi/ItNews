using NHibernate;
using ItNews.Business;
using System;
using System.Threading.Tasks;
using ItNews.Mvc;
using System.Web.Mvc;

namespace ItNews.Nhibernate
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private TransactionContainer transaction;

        public UnitOfWork()
        {
            transaction = new TransactionContainer();
        }

        public bool IsActive => transaction.IsActive;

        public bool IsCommited => transaction.IsCommited;

        public bool IsRolledBack => transaction.IsRolledBack;

        public void BeginTransaction()
        {
            transaction.Begin();
        }

        public void Commit()
        {
            try
            {
                transaction.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                transaction.RollBack();
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            transaction.Dispose();
        }
    }
}