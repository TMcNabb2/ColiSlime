using System;
using System.Linq;
using UnityEngine;

namespace AutoClass.Contexts
{
    public class MethodContext<TParent> : BuilderContext<TParent>,
            IMethodBody
        where TParent : IBuilderContext
    {
        public TParent Return(string value = "") => this.AddLine($"return {value};").Escape();
    }

    public interface IMethodBody : IMethodDeclarable, IFieldDeclarable, IIfDeclarable { }

    /// <summary>
    /// A context can inherit from this to allow declaring methods.
    /// </summary>
    public interface IMethodDeclarable : IBuilderContext { }

    public partial class ScopeExtensions
    {
        public static MethodContext<T> AddMethod<T>(this T context,
            Access access = Access.Private,
            Modifiers modifiers = Modifiers.None,
            string returnType = "void",
            string name = "MyMethod",
            string parameters = "")
            where T : IMethodDeclarable
        {
            return context.AddLine(
                string.Concat(
                    access.AsKeyword(),
                    modifiers.AsKeyword(),
                    returnType.AsReturnType(),
                    name,
                    $"({parameters})",
                    " {")
                ).Open(out MethodContext<T> _);
        }
    }
}
