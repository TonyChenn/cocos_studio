using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Static helper methods for working with nullable types.
	/// </summary>
	// Token: 0x020000F1 RID: 241
	public static class NullableType
	{
		/// <summary>
		/// Gets whether the specified type is a nullable type.
		/// </summary>
		// Token: 0x060008E8 RID: 2280 RVA: 0x00017D00 File Offset: 0x00016D00
		public static bool IsNullable(IType type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			ParameterizedType parameterizedType = type as ParameterizedType;
			return parameterizedType != null && parameterizedType.TypeParameterCount == 1 && parameterizedType.GetDefinition().KnownTypeCode == KnownTypeCode.NullableOfT;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00017D40 File Offset: 0x00016D40
		public static bool IsNonNullableValueType(IType type)
		{
			return type.IsReferenceType == false && !NullableType.IsNullable(type);
		}

		/// <summary>
		/// Returns the element type, if <paramref name="type" /> is a nullable type.
		/// Otherwise, returns the type itself.
		/// </summary>
		// Token: 0x060008EA RID: 2282 RVA: 0x00017D74 File Offset: 0x00016D74
		public static IType GetUnderlyingType(IType type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			ParameterizedType parameterizedType = type as ParameterizedType;
			if (parameterizedType != null && parameterizedType.TypeParameterCount == 1 && parameterizedType.FullName == "System.Nullable")
			{
				return parameterizedType.GetTypeArgument(0);
			}
			return type;
		}

		/// <summary>
		/// Creates a nullable type.
		/// </summary>
		// Token: 0x060008EB RID: 2283 RVA: 0x00017DC0 File Offset: 0x00016DC0
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
			IType type = compilation.FindType(KnownTypeCode.NullableOfT);
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

		/// <summary>
		/// Creates a nullable type reference.
		/// </summary>
		// Token: 0x060008EC RID: 2284 RVA: 0x00017E10 File Offset: 0x00016E10
		public static ParameterizedTypeReference Create(ITypeReference elementType)
		{
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			return new ParameterizedTypeReference(KnownTypeReference.NullableOfT, new ITypeReference[]
			{
				elementType
			});
		}
	}
}
