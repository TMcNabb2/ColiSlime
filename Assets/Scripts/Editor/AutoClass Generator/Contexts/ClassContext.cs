using System;
using static AutoClass.Utils;

namespace AutoClass.Contexts
{
    public class ClassContext<TParent> : BuilderContext<TParent>,
        IClassBody
        where TParent : IBuilderContext
    {
        public string Name { get; set; }

        public MethodContext<ClassContext<TParent>> AddConstructor(
            Access access = Access.Public,
            params string[] parameters)
        {
            return this.AddLine($"{access.AsKeyword()}{Name}({Combine(", ", parameters)}) {{")
                .Open(out MethodContext<ClassContext<TParent>> _);
        }
    }

    public interface IClassBody :
        IClassDeclarable,
        IEnumDeclarable,
        IMethodDeclarable,
        IFieldDeclarable,
        IPropertyDeclarable { }

    /// <summary>
    /// A context can inherit from this to allow declaring classes.
    /// </summary>
    public interface IClassDeclarable : IBuilderContext { }

    public static partial class ScopeExtensions
    {
        public static ClassContext<T> AddClass<T>(this T context,
            Access access = Access.Public,
            Modifiers modifiers = Modifiers.None,
            ClassType classType = ClassType.Class,
            string name = "MyClass",
            string baseType = "")
            where T : IClassDeclarable
        {
            context.AddLine(
                string.Concat(
                    access.AsKeyword(),
                    modifiers.AsKeyword(),
                    classType.AsKeyword(),
                    name,
                    GetBaseTypes(baseType),
                    " {")
                ).Open(out ClassContext<T> child);
            child.Name = name;
            return child;
        }
    }
}
