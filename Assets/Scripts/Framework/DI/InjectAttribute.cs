using System;
using JetBrains.Annotations;

namespace Framework.DI
{
    [AttributeUsage(AttributeTargets.Field)]
    [MeansImplicitUse]
    public class InjectAttribute : Attribute
    {
        
    }
}