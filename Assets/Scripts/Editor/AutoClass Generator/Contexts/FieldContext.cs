using System;

namespace AutoClass.Contexts
{
    /// <summary>
    /// A context can inherit from this to allow declaring fields.
    /// </summary>
    public interface IFieldDeclarable : IBuilderContext { }

    public partial class ScopeExtensions
    {
        public static T AddField<T>(this T context,
            Access access = Access.Private,
            Modifiers modifiers = Modifiers.None,
            string type = "void",
            string name = "myVariable",
            string initialValue = "default")
            where T : IFieldDeclarable
        {
            return context.AddLine(
                string.Concat(
                    access.AsKeyword(),
                    modifiers.AsKeyword(),
                    type.AsReturnType(),
                    name,
                    " = ",
                    initialValue,
                    ";")
                );
        }
    }
}
