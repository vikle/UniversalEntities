namespace UniversalEntities
{
    public sealed class EventCollector<T> : IUpdateSystem where T : class, IEvent
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (entity.Has<T>())
                {
                    entity.Remove<T>();
                }
            }
        }
    };
}
