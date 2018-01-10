using NHibernate;
using System;

namespace ItNews.Nhibernate
{
    public class SessionContainerFactory
    {
        public SessionContainerFactory()
        {

        }

        private SessionContainer currentContainer;

        public SessionContainer Current
        {
            get
            {
                if (currentContainer?.Disposed == true)
                {
                    currentContainer = currentContainer.Parent;
                    return Current;
                }

                return currentContainer;
            }
            private set
            {
                currentContainer = value;
            }
        }

        public SessionContainer CreateSessionContainer()
        {
            return Current = new SessionContainer(Current);
        }

    }
}