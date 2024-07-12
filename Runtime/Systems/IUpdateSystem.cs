namespace UniversalEntities
{
    public interface IUpdateSystem : ISystem
    {
        void OnUpdate(IContext context);
    };
}


