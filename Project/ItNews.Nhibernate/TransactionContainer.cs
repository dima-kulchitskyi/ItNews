using ItNews.Business;
using ItNews.Mvc;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ItNews.Nhibernate
{
    public class TransactionContainer : IDisposable
    {
        private const string CurrentTransactionContainerKey = "CurrentTransactionContainer";

        private RequestDataStorage requestDataStorage;

        private readonly TransactionContainer parent;

        private SessionContainer sessionContainer;

        private ITransaction transaction;

        public TransactionContainer()
        {
            requestDataStorage = DependencyResolver.Current.GetService<RequestDataStorage>();

            parent = requestDataStorage.GetValue<TransactionContainer>(CurrentTransactionContainerKey);
            requestDataStorage.SetValue(CurrentTransactionContainerKey, this);

            sessionContainer = SessionContainer.Open();
        }

        public bool IsActive => transaction?.IsActive ?? false;

        public bool IsCommited => transaction?.WasCommitted ?? false;

        public bool IsRolledBack => transaction?.WasRolledBack ?? false;

        private bool IsBaseContainer => parent == null || !parent.IsActive;

        public void Begin()
        {
            if (transaction == null || !transaction.IsActive)
                transaction = IsBaseContainer ? sessionContainer.Session.BeginTransaction() : parent.transaction;
        }

        public void RollBack()
        {
            transaction.Rollback();
        }

        public void Commit()
        {
            if (IsBaseContainer)
                transaction.Commit();
        }

        public void Dispose()
        {
            if (IsBaseContainer)
                transaction?.Dispose();

            sessionContainer.Dispose();

            requestDataStorage.SetValue(CurrentTransactionContainerKey, parent);
        }
    }
}
