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

        public async Task Commit()
        {
            try
            {
                await transaction.Commit();
            }
            catch
            {
                await Rollback();
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        public async Task Rollback()
        {
            try
            {
                await transaction.RollBack();
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