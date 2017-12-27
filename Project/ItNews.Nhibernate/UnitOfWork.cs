using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using ItNews.Business;
using ItNews.Business.Entities;
using System;
using ItNews.Nhibernate.Providers;
using System.Web.Mvc;
using Ninject.Web.Mvc;

namespace ItNews.Nhibernate
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        protected SessionManager sessionManager;

        protected ITransaction transaction;

        protected ISession session;

        public UnitOfWork(SessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
        }

        public IUnitOfWork BeginTransaction()
        {
            session = sessionManager.GetExistingOrOpenSession();
            transaction = session.Transaction;

            if (transaction != null && transaction.IsActive)
                throw new InvalidOperationException("Transaction is already open");

            transaction = session.BeginTransaction();

            return this;
        }

        public void CommitTransaction()
        {
            if (transaction == null || !transaction.IsActive)
                throw new InvalidOperationException("Transaction is not active");

            try
            {
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
            }
            finally
            {
                session?.Dispose();
            }
        }

        public void RollbackTransaction()
        {
            if (transaction == null || !transaction.IsActive)
                throw new InvalidOperationException("Transaction is not active");

            try
            {
                transaction.Rollback();
            }
            finally
            {
                session?.Dispose();
            }
        }

        public void Dispose()
        {
            if (transaction != null && transaction.IsActive)
                RollbackTransaction();

            session?.Dispose();

            transaction = null;
            sessionManager = null;
        }
    }
}