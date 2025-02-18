using UnityEngine;

namespace AutoClass.Contexts
{
    public class IfContext<TParent> : BuilderContext<TParent>,
        IMethodBody
        where TParent : IBuilderContext
    {
        public IfContext<TParent> Else(string condition = null)
        {
            return this.AddLine(
                text: string.Concat(
                    "}",
                    " else ",
                    !string.IsNullOrEmpty(condition) ? $"if ({condition}) " : string.Empty,
                    "{"),
                indent: Indent - 1);
        }
    }

    /// <summary>
    /// A context can inherit from this to allow declaring if statements.
    /// </summary>
    public interface IIfDeclarable : IBuilderContext { }

    public partial class ScopeExtensions
    {
        public static IfContext<T> AddIf<T>(this T context,
            string condition = "true")
            where T : IIfDeclarable
        {
            context.AddLine($"if ({condition}) {{");
            return context.Open(out IfContext<T> _);
        }
    }
}
