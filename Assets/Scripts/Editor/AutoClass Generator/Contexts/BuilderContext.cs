using System.Text;
using UnityEngine;
using static AutoClass.Utils;

namespace AutoClass
{
    public interface IBuilderContext
    {
        public StringBuilder Builder { get; set; }
        public int Indent { get; set; }
    }

    public interface IBuilderContext<TParent> : IBuilderContext where TParent : IBuilderContext
    {
        public TParent Parent { get; set; }
    }

    /// <summary>
    /// Represents a specific context in the class builder.
    /// </summary>
    /// <typeparam name="TParent">The context type to return after escaping the context.</typeparam>
    public abstract class BuilderContext<TParent> : IBuilderContext<TParent>
        where TParent : IBuilderContext
    {
        /// <summary>
        /// Is <see langword="true"/> if <see cref="Escape"/> has been called at least once.
        /// </summary>
        protected bool Escaped { get; set; } = false;

        public StringBuilder Builder { get; set; }
        public int Indent { get; set; }

        public TParent Parent { get; set; }

        protected BuilderContext()
        {
            Builder = new();
            Indent = 0;
        }

        /// <summary>
        /// Escapes the current context and returns the parent.
        /// </summary>
        /// <returns><see cref="Parent"/></returns>
        public TParent Escape()
        {
            if (!Escaped) // Prevents multi-escaping as the parent may be the same as the child
            {
                Escaped = true;
                OnReturn();
                Parent.Builder = Builder;
            }
            return Parent;
        }
        /// <summary>
        /// Invoked when <see cref="Escape"/> is called.
        /// </summary>
        protected virtual void OnReturn()
        {
            this.AddLine("}", Mathf.Max(0, Indent - 1));
        }

        public override string ToString()
        {
            return Builder.ToString();
        }
    }

    public static class ContextExtensions
    {
        /// <summary>
        /// Appends the given <paramref name="text"/> to the <paramref name="context"/>. Bypasses all validation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="text">The text to append.</param>
        /// <param name="indent">Overrides the default context indent level.</param>
        /// <returns><paramref name="context"/>.</returns>
        public static T Add<T>(this T context, string text, int indent = -1) where T : IBuilderContext
        {
            if (indent < 0)
                indent = context.Indent;
            context.Builder.Append($"{Repeat("\t", indent)}{text}");
            return context;
        }
        /// <summary>
        /// Appends the given line of <paramref name="text"/> to the <paramref name="context"/>. Bypasses all validation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="text">The text to append.</param>
        /// <param name="indent">Overrides the default context indent level.</param>
        /// <returns><paramref name="context"/>.</returns>
        public static T AddLine<T>(this T context, string text, int indent = -1) where T : IBuilderContext
        {
            if (indent < 0)
                indent = context.Indent;
            context.Builder.AppendLine($"{Repeat("\t", indent)}{text}");
            return context;
        }

        public static T AddAttributes<T>(this T context, params string[] attributes) where T : IBuilderContext
        {
            return context.AddLine($"[{Combine(", ", attributes)}]");
        }

        /// <summary>
        /// Opens a subcontext of type <typeparamref name="TChild"/>.
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns>The child context.</returns>
        public static TChild Open<TParent, TChild>(this TParent parent, out TChild child)
            where TParent : IBuilderContext
            where TChild : class, IBuilderContext<TParent>, new()
        {
            child = new TChild() { Parent = parent, Builder = parent.Builder, Indent = parent.Indent + 1 };
            return child;
        }
    }
}
