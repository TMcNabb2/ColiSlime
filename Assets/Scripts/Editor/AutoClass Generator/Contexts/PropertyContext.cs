using System;
using UnityEngine;

namespace AutoClass.Contexts
{
    public class PropertyContext<TParent> : BuilderContext<TParent>

        where TParent : IBuilderContext
    {
        public PropertyContext<TParent> AutoGet(Access access = Access.Public)
        {
            return this.Add($"{(access is Access.Public ? "" : access.AsKeyword())}get; ", indent: 0);
        }

        public PropertyContext<TParent> AutoSet(Access access = Access.Public)
        {
            return this.Add($"{(access is Access.Public ? "" : access.AsKeyword())}set; ", indent: 0);
        }

        public MethodContext<PropertyContext<TParent>> Get() => Accessor("get");
        public MethodContext<PropertyContext<TParent>> Set() => Accessor("set");

        private MethodContext<PropertyContext<TParent>> Accessor(string name)
        {
            return this.AddLine("").AddLine($"{name} {{").Open(out MethodContext<PropertyContext<TParent>> _);
        }
    }

    /// <summary>
    /// A context can inherit from this to allow declaring properties.
    /// </summary>
    public interface IPropertyDeclarable : IBuilderContext { }

    public partial class ScopeExtensions
    {
        public static PropertyContext<T> AddProperty<T>(this T context,
            Access access = Access.Private,
            Modifiers modifiers = Modifiers.None,
            string returnType = null,
            string name = "MyProperty")
            where T : IPropertyDeclarable
        {
            return context.Add(
                text: string.Concat(
                    access.AsKeyword(),
                    modifiers.AsKeyword(),
                    returnType.AsReturnType(),
                    name,
                    " { ")
                ).Open(out PropertyContext<T> _);
        }
    }
}
