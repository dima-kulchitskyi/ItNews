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

        private bool useTransaction;

        public UnitOfWork(SessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
        }

        public IUnitOfWork BeginTransaction()
        {
            transaction = sessionManager.Session.Transaction;
            if (transaction == null || !transaction.IsActive)
                transaction = sessionManager.Session.BeginTransaction();

            useTransaction = true;

            return this;
        }

        public void RollbackTransaction()
        {
            if (!useTransaction)
                throw new InvalidOperationException("Transaction wan not opened");

            transaction = sessionManager.Session?.Transaction;
            try
            {
                if (transaction?.IsActive == true && transaction?.WasCommitted == false && transaction?.WasRolledBack == false)
                    transaction?.Rollback();
            }
            finally
            {
                sessionManager.Session?.Dispose();
            }
        }

        public void CommitTransaction()
        {
            if (!useTransaction)
                throw new InvalidOperationException("Transaction wan not opened");
            try
            {
                if (transaction?.IsActive == true && transaction?.WasCommitted == false && transaction?.WasRolledBack == false)
                    transaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
            }
            finally
            {
                sessionManager.Session?.Dispose();
            }
        }

        public void Dispose()
        {
            if (useTransaction)
                CommitTransaction();

            sessionManager.Session?.Dispose();
        }
    }
}