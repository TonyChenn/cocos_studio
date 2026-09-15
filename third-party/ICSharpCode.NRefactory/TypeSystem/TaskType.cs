using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Helper class for dealing with System.Threading.Tasks.Task.
	/// </summary>
	public static class TaskType
	{
		/// <summary>
		/// Gets the T in Task&lt;T&gt;.
		/// Returns void for non-generic Task.
		/// Any other type is returned unmodified.
		/// </summary>
		public static IType UnpackTask(ICompilation compilation, IType type)
		{
			if (!TaskType.IsTask(type))
			{
				return type;
			}
			if (type.TypeParameterCount == 0)
			{
				return compilation.FindType(KnownTypeCode.Void);
			}
			return type.TypeArguments[0];
		}

		/// <summary>
		/// Gets whether the specified type is Task or Task&lt;T&gt;.
		/// </summary>
		public static bool IsTask(IType type)
		{
			ITypeDefinition definition = type.GetDefinition();
			if (definition != null)
			{
				if (definition.KnownTypeCode == KnownTypeCode.Task)
				{
					return true;
				}
				if (definition.KnownTypeCode == KnownTypeCode.TaskOfT)
				{
					return type is ParameterizedType;
				}
			}
			return false;
		}

		/// <summary>
		/// Creates a task type.
		/// </summary>
		public static IType Create(ICompilation compilation, IType elementType)
		{
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			if (elementType.Kind == TypeKind.Void)
			{
				return compilation.FindType(KnownTypeCode.Task);
			}
			IType type = compilation.FindType(KnownTypeCode.TaskOfT);
			ITypeDefinition definition = type.GetDefinition();
			if (definition != null)
			{
				return new ParameterizedType(definition, new IType[]
				{
					elementType
				});
			}
			return type;
		}
	}
}
