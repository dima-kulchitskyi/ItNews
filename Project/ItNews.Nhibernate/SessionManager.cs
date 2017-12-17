using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using ItNews.Business.Entities;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Web.Configuration;

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
