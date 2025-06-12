#if ENABLE_IL2CPP

using System;

namespace Unity.IL2CPP.CompilerServices
{
    public enum Option
    {
        NullChecks = 1,
        ArrayBoundsChecks = 2,
        DivideByZeroChecks = 3,
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Delegate, Inherited = false, AllowMultiple = true)]
    public class Il2CppSetOptionAttribute : Attribute
    {
        public Option Option { get; private set; }
        public object Value { get; private set; }

        public Il2CppSetOptionAttribute(Option option, object value)
        {
            Option = option;
            Value = value;
        }
    }
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class Il2CppEagerStaticClassConstructionAttribute : Attribute
    {
    }
}

#endif