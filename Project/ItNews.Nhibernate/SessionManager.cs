using NHibernate;

namespace ItNews.Nhibernate
{
    public class SessionManager
    {
        protected readonly ISessionFactory sessionFactory;

        protected ISession session;

        public ISession Session => session ?? (session = sessionFactory?.OpenSession());

        public SessionManager(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }
    }
}