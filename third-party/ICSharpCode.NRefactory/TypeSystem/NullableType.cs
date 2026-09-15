using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Static helper methods for working with nullable types.
	/// </summary>
	public static class NullableType
	{
		/// <summary>
		/// Gets whether the specified type is a nullable type.
		/// </summary>
		public static bool IsNullable(IType type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			ParameterizedType parameterizedType = type as ParameterizedType;
			return parameterizedType != null && parameterizedType.TypeParameterCount == 1 && parameterizedType.GetDefinition().KnownTypeCode == KnownTypeCode.NullableOfT;
		}

		public static bool IsNonNullableValueType(IType type)
		{
			return type.IsReferenceType == false && !NullableType.IsNullable(type);
		}

		/// <summary>
		/// Returns the element type, if <paramref name="type" /> is a nullable type.
		/// Otherwise, returns the type itself.
		/// </summary>
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
