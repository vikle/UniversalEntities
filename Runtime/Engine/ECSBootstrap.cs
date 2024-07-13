using UnityEngine;

namespace UniversalEntities
{
    public abstract class ECSBootstrap : MonoBehaviour
    {
        public abstract void OnBootstrap(IContextBinding context);
    };
}
