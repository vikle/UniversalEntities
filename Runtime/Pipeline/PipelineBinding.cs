using System;

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
        
        public Pipeline BindSystem<T>() where T : class, ISystem
        {
            var system_type = typeof(T);

#if (UNITY_2020_3_OR_NEWER && DEBUG)
            var all_constructors = system_type.GetConstructors();
            var main_constructor = all_constructors[0];

            if (!Attribute.IsDefined(main_constructor, typeof(UnityEngine.Scripting.PreserveAttribute)))
            {
                throw new Exception($"Not found 'UnityEngine.Scripting.PreserveAttribute' on Constructor in '{system_type.Name}'.");
            }
#endif
            object system = Activator.CreateInstance(system_type, m_systemParams);
            m_allSystems.Add((ISystem)system);
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
