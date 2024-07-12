namespace UniversalEntities
{
    public interface IFixedUpdateSystem : ISystem
    {
        void OnFixedUpdate(IContext context);
    };
}
