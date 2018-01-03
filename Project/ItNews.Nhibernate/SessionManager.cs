using NHibernate;
using System;

namespace ItNews.Nhibernate
{
    public class SessionManager
    {
        protected readonly ISessionFactory sessionFactory;

        protected ISession session;

        public bool IsSessionOpen => session != null && session.IsOpen;

        public SessionManager(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public ISession GetCurrentSession()
        {
            return session ?? throw new InvalidOperationException("Session does not exists.");
        }

        public ISession GetExistingOrOpenSession()
        {
            if (session == null || !session.IsOpen)
                session = sessionFactory.OpenSession();

            return session;
        }

    }
}