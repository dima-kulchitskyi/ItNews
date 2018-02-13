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
    public class FileProvider<T> : IProvider<T>
        where T : class, IEntity
    {
        protected readonly string fileName;

        protected static readonly MyAsyncAutoResetEvent autoResetEvent = new MyAsyncAutoResetEvent();

        public FileProvider()
        {
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, WebConfigurationManager.AppSettings["MemoryProviderFolder"], typeof(T).Name);
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return new FileProviderUnitOfWork();
        }

        protected async Task<IList<T>> ReadFile()
        {
            using (var reader = new StreamReader(fileName, Encoding.UTF8))
                return JsonConvert.DeserializeObject<List<T>>(await reader.ReadToEndAsync()) ?? new List<T>();
        }

        protected async Task WriteFile(IList<T> collection)
        {
            using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
                await writer.WriteAsync(JsonConvert.SerializeObject(collection ?? new List<T>()));
        }

        public async Task Delete(T instance)
        {
            await autoResetEvent.Wait();

            try
            {
                var collection = await ReadFile();
                var item = collection.Where(it => it.Id == instance.Id).FirstOrDefault();

                if (item != null)
                {
                    collection.Remove(item);
                    await WriteFile(collection);
                }
            }
            finally
            {
                autoResetEvent.Set();
            }
        }

        public async Task<T> Get(string id)
        {
            return (await GetList()).Where(it => it.Id == id).FirstOrDefault();
        }

        public async Task<IList<T>> Get(IEnumerable<string> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            if (ids.Count() == 0)
                throw new ArgumentException($"{nameof(ids)} is empty");

            return (await GetList()).Where(it => ids.Contains(it.Id)).ToList();
        }

        public async Task<int> GetCount()
        {
            return (await GetList()).Count;
        }

        public virtual async Task<IList<T>> GetList()
        {
            await autoResetEvent.Wait();

            try
            {
                return await ReadFile();
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
                var collection = await ReadFile();

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

                await WriteFile(collection);

                return instance;
            }
            finally
            {
                autoResetEvent.Set();
            }
        }
    }
}
