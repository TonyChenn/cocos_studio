using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Helper class for dealing with System.Threading.Tasks.Task.
	/// </summary>
	// Token: 0x02000083 RID: 131
	public static class TaskType
	{
		/// <summary>
		/// Gets the T in Task&lt;T&gt;.
		/// Returns void for non-generic Task.
		/// Any other type is returned unmodified.
		/// </summary>
		// Token: 0x06000412 RID: 1042 RVA: 0x0000A231 File Offset: 0x00009231
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
		// Token: 0x06000413 RID: 1043 RVA: 0x0000A25C File Offset: 0x0000925C
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
		// Token: 0x06000414 RID: 1044 RVA: 0x0000A294 File Offset: 0x00009294
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
