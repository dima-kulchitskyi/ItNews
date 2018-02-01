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
        private const string CurrentUnitOfWorkKey = "CurrentUnitOfWork";

        private RequestDataStorage requestDataStorage;

        private readonly UnitOfWork parent;

        protected ITransaction transaction;

        protected SessionContainer sessionContainer;

        public UnitOfWork()
        {
            sessionContainer = SessionContainer.Open();

            requestDataStorage = DependencyResolver.Current.GetService<RequestDataStorage>();

            parent = requestDataStorage.GetValue<UnitOfWork>(CurrentUnitOfWorkKey);
            requestDataStorage.SetValue(CurrentUnitOfWorkKey, this);
        }

        private bool IsBaseUnit => parent == null;

        public bool IsActive => transaction?.IsActive ?? false;

        public bool IsCommited => transaction?.WasCommitted ?? false;

        public bool IsRolledBack => transaction?.WasRolledBack ?? false;

        public void BeginTransaction()
        {
            if (!IsActive)
                transaction = IsBaseUnit ? sessionContainer.Session.BeginTransaction() : parent.transaction;
        }

        public void Commit()
        {
            if (!IsActive) return;

            try
            {
                if (IsBaseUnit)
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
            if (!IsActive) return;

            try
            {
                if (IsBaseUnit)
                    transaction.Rollback();
                else
                    parent.Rollback();
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (IsBaseUnit && IsActive)
            {
                transaction.Rollback();
                throw new InvalidOperationException("Transaction is still active, maybe you forgot to commit it?");
            }

            requestDataStorage.SetValue(CurrentUnitOfWorkKey, parent);

            sessionContainer.Dispose();
        }
    }
}