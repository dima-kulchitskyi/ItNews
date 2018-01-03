using NHibernate;

namespace ItNews.Nhibernate
{
    public class SessionManager
    {
        protected readonly ISessionFactory sessionFactory;

        protected ISession session;

        public ISession Session
        {
            get
            {
                if (session == null || !session.IsOpen)
                    session = sessionFactory?.OpenSession();

                return session;
            }
        }

        public bool IsSessionOpen => session != null && session.IsOpen;

        public SessionManager(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }
    }
}