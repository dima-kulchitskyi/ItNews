using ItNews.Business.Entities;
using ItNews.Nhibernate.Providers;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Nhibernate
{
    public static class ProviderHelper
    {
        public static async Task<TResult> GetSession<TResult>(Func<ISession, Task<TResult>> func)
        {
            using (var sessionContainer = SessionContainer.Open())
                return await func(sessionContainer.Session);
        }

        public static async Task GetSession(Func<ISession, Task> func)
        {
            await GetSession(async s =>
            {
                await func(s);
                return true;
            });
        }
    }
}
