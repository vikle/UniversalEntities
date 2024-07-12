namespace UniversalEntities
{
    public interface ILateUpdateSystem : ISystem
    {
        void OnLateUpdate(IContext context);
    };
}
