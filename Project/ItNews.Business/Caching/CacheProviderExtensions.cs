using ItNews.Business.Caching;
using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business.Caching
{
    public static class CacheProviderExtensions
    {
        public static async Task<TResult> GetById<TResult>(this CacheProvider<TResult> provider, string id, Func<Task<TResult>> func)
            where TResult : class, IEntity
        {
            var result = provider.GetById(id);

            if (result == null)
            {
                result = await func();
                provider.Save(result);
            }

            return result;
        }

        public static async Task<IEnumerable<TResult>> GetMany<TResult>(this CacheProvider<TResult> provider, IList<string> ids, Func<IEnumerable<string>, Task<IList<TResult>>> func)
            where TResult : class, IEntity
        {
            var notLoaded = new List<string>(ids);

            var results = new List<TResult>(ids.Count);

            for (var i = 0; i < notLoaded.Count;)
            {
                var item = provider.GetById(notLoaded[i]);
                if (item != null)
                {
                    results.Add(item);
                    notLoaded.RemoveAt(i);
                }
                else i++;
            }

            if (notLoaded.Count > 0)
            {
                var newItems = await func(notLoaded);

                foreach (var it in newItems)
                    if (it != null)
                        provider.Save(it);

                results.AddRange(newItems);
            }

            return results.OrderBy(it => ids.IndexOf(it.Id));
        }
    }
}
