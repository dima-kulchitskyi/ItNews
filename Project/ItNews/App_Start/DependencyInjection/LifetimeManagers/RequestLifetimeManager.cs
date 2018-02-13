using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity.AspNet.Mvc;
using Unity.Lifetime;

namespace ItNews.DependencyInjection.LifetimeManagers
{
    public class RequestLifetimeManager : LifetimeManager
    {
        object key = new object();

        public LifetimeManager Resolve()
        {
            if (HttpContext.Current == null)
                return new TransientLifetimeManager();

            return new PerRequestLifetimeManager();
        } 

        public override object GetValue(ILifetimeContainer container = null)
        {
            return Resolve().GetValue(container);
        }

        public override void RemoveValue(ILifetimeContainer container = null)
        {
             Resolve().RemoveValue(container);
        }

        public override void SetValue(object newValue, ILifetimeContainer container = null)
        {
            Resolve().SetValue(newValue, container); 
        }

        protected override LifetimeManager OnCreateLifetimeManager()
        {
            return Resolve();
        }
    }
}