using System.Collections.Generic;

namespace UniversalEntities
{
    public sealed class PromiseCollector<T> 
        : Processor<T>
        , IUpdateSystem where T : class, IPromise
    {
        protected override void OnExecute(IContext context, IEntity entity)
        {
            var promise = m_data1;
            
            List<IEvent> resolve;
            var state = promise.State;

            switch (state)
            {
                case EPromiseState.Fulfilled:
                case EPromiseState.Rejected: 
                    resolve = promise.Resolve;
                    break;
                    
                default: return;
            }
                
            switch (state)
            {
                case EPromiseState.Fulfilled:
                    var target = promise.Target;
                    for (int i = 0, i_max = resolve.Count; i < i_max; i++)
                    {
                        target.Add(resolve[i]);
                    }
                    break;
                case EPromiseState.Rejected: 
                    for (int i = 0, i_max = resolve.Count; i < i_max; i++)
                    {
                        FragmentPool.Release(resolve[i]);
                    }
                    break; 
            }

            resolve.Clear();
            
            promise.Target = null;
            promise.State = EPromiseState.Pending;

            entity.Remove(promise);
        }
    };
}
