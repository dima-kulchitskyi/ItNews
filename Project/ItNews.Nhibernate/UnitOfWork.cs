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
        public SessionManager SessionManager { get; protected set; }

        protected ITransaction transaction;

        public UnitOfWork(SessionManager sessionManager)
        {
            SessionManager = sessionManager;
        }

        public void BeginTransaction()
        {
            transaction = SessionManager?.Session?.Transaction;
            if (transaction == null || transaction.WasCommitted)
                transaction = SessionManager?.Session?.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            transaction = SessionManager?.Session?.Transaction;
            if (transaction?.IsActive == true && transaction?.WasCommitted == false && transaction?.WasRolledBack == false)
                transaction?.Rollback();
        }

        public void CommitTransaction()
        {
            if (transaction?.IsActive == true && transaction?.WasCommitted == false && transaction?.WasRolledBack == false)
                transaction?.Commit();
        }

        public void Dispose()
        {
            CommitTransaction();

            SessionManager?.Session?.Dispose();
        }


     
    }
}
