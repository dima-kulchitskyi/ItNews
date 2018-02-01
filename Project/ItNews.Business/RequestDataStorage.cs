using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business
{
    public class RequestDataStorage
    {
        private Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public T GetValue<T>(string key)
            where T : class
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key] as T;

            return null;
        }

        public void SetValue<T>(string key, T value)
            where T : class
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);
        }
    }
}
