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
    public class SessionContainer : IDisposable
    {
        private const string CurrentSessionKey = "CurrentSessionContainerKey";

        private RequestDataStorage requestDataStorage;

        private readonly SessionContainer parent;

        public SessionContainer()
        {
            requestDataStorage = DependencyResolver.Current.GetService<RequestDataStorage>();

            parent = requestDataStorage.GetValue<SessionContainer>(CurrentSessionKey);
            requestDataStorage.SetValue(CurrentSessionKey, this);

            Session = IsBaseContainer ? DependencyResolver.Current.GetService<ISessionFactory>().OpenSession() : parent.Session;
        }
        
        public ISession Session { get;  }

        private bool IsBaseContainer => parent == null;

        public static SessionContainer Open()
        {
          
            return new SessionContainer();
        }

        public void Dispose()
        {
            if (IsBaseContainer)
                Session.Dispose();

            requestDataStorage.SetValue(CurrentSessionKey, parent);
        }
    }
}
