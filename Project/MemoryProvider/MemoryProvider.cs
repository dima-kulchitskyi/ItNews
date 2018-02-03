using ItNews.Business;
using ItNews.Business.Entities;
using ItNews.Business.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace ItNews.FileProvider
{
    public class MemoryProvider<T> : IProvider<T>
        where T : class, IEntity
    {
        protected readonly string fileName;
        protected static readonly MyAsyncAutoResetEvent autoResetEvent = new MyAsyncAutoResetEvent();

        public MemoryProvider()
        {
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, WebConfigurationManager.AppSettings["MemoryProviderFolder"], typeof(T).Name);
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return new MemoryProviderUnitOfWork();
        }

        public async Task Delete(T instance)
        {
            await autoResetEvent.Wait();

            try
            {
                IList<T> collection;
                using (var reader = new StreamReader(fileName, Encoding.UTF8))
                    collection = JsonConvert.DeserializeObject<List<T>>(await reader.ReadToEndAsync());

                if (collection != null)
                {
                    var item = collection.Where(it => it.Id == instance.Id).FirstOrDefault();

                    if (item != null)
                    {
                        collection.Remove(item);
                        using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
                            await writer.WriteAsync(JsonConvert.SerializeObject(collection));
                    }
                }
            }
            finally
            {
                autoResetEvent.Set();
            }
        }


        public virtual async Task<T> Get(string id)
        {
            await autoResetEvent.Wait();
            try
            {
                using (var reader = new StreamReader(fileName, Encoding.UTF8))
                {
                    var collection = JsonConvert.DeserializeObject<IList<T>>(await reader.ReadToEndAsync());
                    return collection.Where(it => it.Id == id).FirstOrDefault();
                }
            }
            finally
            {
                autoResetEvent.Set();
            }
        }

        public async Task<int> GetCount()
        {
            await autoResetEvent.Wait();

            try
            {
                using (var reader = new StreamReader(fileName, Encoding.UTF8))
                    return JsonConvert.DeserializeObject<IList<T>>(await reader.ReadToEndAsync())?.Count ?? 0;
            }
            finally
            {
                autoResetEvent.Set();
            }
        }

        public virtual async Task<IList<T>> GetList()
        {
            await autoResetEvent.Wait();

            try
            {
                using (var reader = new StreamReader(fileName, Encoding.UTF8))
                    return JsonConvert.DeserializeObject<IList<T>>(await reader.ReadToEndAsync()) ?? new List<T>();
            }
            finally
            {
                autoResetEvent.Set();
            }
        }

        public virtual async Task<T> SaveOrUpdate(T instance)
        {
            await autoResetEvent.Wait();

            try
            {
                IList<T> collection;

                using (var reader = new StreamReader(fileName, Encoding.UTF8))
                {
                    collection = JsonConvert.DeserializeObject<IList<T>>(await reader.ReadToEndAsync()) ?? new List<T>();

                    if (!string.IsNullOrEmpty(instance.Id))
                    {
                        var item = collection.Where(it => it.Id == instance.Id).FirstOrDefault();

                        if (item != null)
                            collection[collection.IndexOf(item)] = instance;
                        else
                            collection.Add(instance);
                    }
                    else
                    {
                        instance.Id = Guid.NewGuid().ToString();
                        collection.Add(instance);
                    }
                }

                using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
                    await writer.WriteAsync(JsonConvert.SerializeObject(collection));

                return instance;
            }
            finally
            {
                autoResetEvent.Set();
            }
        }
    }
}
