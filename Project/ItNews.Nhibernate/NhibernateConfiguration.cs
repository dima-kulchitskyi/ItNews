using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Ninject;
using System.Configuration;

namespace ItNews.Nhibernate
{
    public static class NhibernateConfiguration
    {
        public static void RegisterDependencies(IKernel kernel)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ToString();
            kernel.Bind<ISessionFactory>().ToConstant(Fluently.Configure().Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString).ShowSql())
                                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SessionManager>())
                                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                                .BuildSessionFactory());
        }
    }
}
