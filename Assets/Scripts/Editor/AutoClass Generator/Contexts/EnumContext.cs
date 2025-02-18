using UnityEngine;

namespace AutoClass.Contexts
{
	public class EnumContext<TParent> : BuilderContext<TParent>
		where TParent : IBuilderContext
	{
		public EnumContext<TParent> AddValue(string name, int? value = null)
		{
			string suffix = value is null ? string.Empty : $" = {value}";
			return this.AddLine($"{name}{suffix},");
		}

		public EnumContext<TParent> AddValues(params (string name, int? value)[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				this.AddValue(values[i].name, values[i].value);
			}
			return this;
		}
	}

	public interface IEnumDeclarable : IBuilderContext { }

	public static partial class ScopeExtensions
	{
		public static EnumContext<T> AddEnum<T>(this T context,
			Access access = Access.Public,
			string name = "MyEnum")
			where T : IEnumDeclarable
		{
			return context.AddLine(
				string.Concat(
					access.AsKeyword(),
					name,
					" {"
				)).Open(out EnumContext<T> _);
		}
	}
}