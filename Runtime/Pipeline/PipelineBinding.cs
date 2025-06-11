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

#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
            var all_constructors = system_type.GetConstructors();

            if (all_constructors.Length == 0)
            {
                throw new Exception($"The '{system_type.Name}' must bee have constructor with one parameter 'Pipeline'.");
            }
            
            var main_constructor = all_constructors[0];

#if UNITY_2020_3_OR_NEWER
            if (!Attribute.IsDefined(main_constructor, typeof(UnityEngine.Scripting.PreserveAttribute)))
            {
                throw new Exception($"Constructor in '{system_type.Name}' must bee have 'UnityEngine.Scripting.PreserveAttribute'.");
            }
#endif
            var parameters = main_constructor.GetParameters();
            
            if (parameters.Length != 1)
            {
                throw new Exception($"Constructor in '{system_type.Name}' must bee have only one parameter.");
            } 
            
            if (parameters[0].ParameterType != typeof(Pipeline))
            {
                throw new Exception($"Constructor in '{system_type.Name}' must bee have parameter type of 'Pipeline'.");
            }
#endif
            object system = Activator.CreateInstance(system_type, m_systemParams);
            m_allSystems.Add((ISystem)system);
            return this;
        }
    };
}
