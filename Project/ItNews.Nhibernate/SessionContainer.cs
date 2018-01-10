using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ItNews.Nhibernate
{
    public class SessionContainer : IDisposable
    {
        private ISessionFactory nhibernateSessionFactory;

        private ISession session;

        private bool isBaseContainer;

        public SessionContainer(SessionContainer parentContainer)
        {
            Parent = parentContainer;
            isBaseContainer = parentContainer == null;

            nhibernateSessionFactory = DependencyResolver.Current.GetService<ISessionFactory>();
        }


        public SessionContainer Parent { get; private set; }

        public ISession Session
        {
            get
            {
                if (isBaseContainer)
                {
                    if (session == null || !session.IsOpen)
                        session = nhibernateSessionFactory.OpenSession();

                    return session;
                }

                return Parent.Session;
            }
        }

        public bool Disposed { get; private set; }

        public void Dispose()
        {
            if (Parent?.Disposed == true)
                throw new InvalidOperationException("The parent container was disposed");

            if (isBaseContainer)
                session?.Dispose();

            Disposed = true;
        }
    }
}
