using System.Collections.Generic;

namespace UniversalEntities
{
    public sealed class PromiseCollector<T> : IUpdateSystem where T : class, IPromise
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.TryGet(out T promise)) continue;

                List<IEvent> resolve;
                var state = promise.State;

                switch (state)
                {
                    case EPromiseState.Fulfilled:
                    case EPromiseState.Rejected: 
                        resolve = promise.Resolve;
                        break;
                    
                    default: continue;
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

                entity.Remove(promise);
                promise.State = EPromiseState.Pending;
                promise.Target = null;
                resolve.Clear();
            }
        }
    };
}
