namespace UniversalEntities
{
    partial class Pipeline
    {
        public Pipeline BindEvent<T>() where T : class, IEvent
        {
            return BindSystem<EventCollector<T>>();
        }
        
        public Pipeline BindPromise<T>() where T : class, IPromise
        {
            return BindSystem<PromiseCollector<T>>();
        }
        
        public Pipeline BindSystem<T>() where T : class, ISystem, new()
        {
            m_allSystems.Add(new T());
            return this;
        }

        public Pipeline Inject<T>(T injection) where T : class
        {
            if (m_injectionsCache.Contains(injection) == false)
            {
                m_injectionsCache.Add(injection);
            }
            
            return this;
        }
    };
}
