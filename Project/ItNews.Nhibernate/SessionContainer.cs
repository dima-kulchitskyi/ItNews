using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ItNews.Nhibernate
{
    public class SessionContainer : IDisposable
    {
        private const string CurrentSessionThearKey = "asdasdad";

        public SessionContainer()
        {
            Parent = (SessionContainer)Thread.GetData(Thread.GetNamedDataSlot(CurrentSessionThearKey));
            Session = IsBaseContainer ? DependencyResolver.Current.GetService<ISessionFactory>().OpenSession() : Parent.Session;

            Thread.SetData(Thread.GetNamedDataSlot(CurrentSessionThearKey), this);
        }

        public SessionContainer Parent { get; }

        public ISession Session { get; }

        private bool IsBaseContainer => Parent == null;

        public static SessionContainer Open()
        {
           return new SessionContainer();
        }

        public void Dispose()
        {
            if (IsBaseContainer)
                Session.Dispose();

            Thread.SetData(Thread.GetNamedDataSlot(CurrentSessionThearKey), Parent);
        }
    }
}
