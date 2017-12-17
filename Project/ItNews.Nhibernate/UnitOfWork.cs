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
    public class UnitOfWork : IDisposable
    {
        public SessionManager SessionManager { get; protected set; }

        public ITransaction Transaction { get; protected set; }

        public UnitOfWork(bool useTransaction)
        {
            SessionManager = DependencyResolver.Current.GetService<SessionManager>();
            if (useTransaction)
                BeginTransaction();
        }

        public UnitOfWork BeginTransaction()
        {
            Transaction = SessionManager?.Session?.Transaction;
            if (Transaction == null || Transaction.WasCommitted)
                Transaction = SessionManager?.Session?.BeginTransaction();

            return this;
        }

        public void Dispose()
        {
            if(Transaction?.IsActive == true && Transaction?.WasCommitted == false && Transaction?.WasRolledBack == false)
                Transaction?.Commit();

            SessionManager?.Session?.Dispose();
        }
    }
}
