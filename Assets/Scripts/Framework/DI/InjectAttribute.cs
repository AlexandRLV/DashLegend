using System;
using JetBrains.Annotations;

namespace Framework.DI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Constructor | AttributeTargets.Property)]
    [MeansImplicitUse]
    public class InjectAttribute : Attribute
    {
        
    }
}