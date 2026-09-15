using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// ParameterizedTypeReference is a reference to generic class that specifies the type parameters.
	/// Example: List&lt;string&gt;
	/// </summary>
	[Serializable]
	public sealed class ParameterizedTypeReference : ITypeReference, ISupportsInterning
	{
		public ParameterizedTypeReference(ITypeReference genericType, IEnumerable<ITypeReference> typeArguments)
		{
			if (genericType == null)
			{
				throw new ArgumentNullException("genericType");
			}
			if (typeArguments == null)
			{
				throw new ArgumentNullException("typeArguments");
			}
			this.genericType = genericType;
			this.typeArguments = typeArguments.ToArray<ITypeReference>();
			for (int i = 0; i < this.typeArguments.Length; i++)
			{
				if (this.typeArguments[i] == null)
				{
					throw new ArgumentNullException("typeArguments[" + i + "]");
				}
			}
		}

		public ITypeReference GenericType
		{
			get
			{
				return this.genericType;
			}
		}

		public ReadOnlyCollection<ITypeReference> TypeArguments
		{
			get
			{
				return Array.AsReadOnly<ITypeReference>(this.typeArguments);
			}
		}

		public IType Resolve(ITypeResolveContext context)
		{
			IType type = this.genericType.Resolve(context);
			ITypeDefinition definition = type.GetDefinition();
			if (definition == null)
			{
				return type;
			}
			int typeParameterCount = definition.TypeParameterCount;
			if (typeParameterCount == 0)
			{
				return definition;
			}
			IType[] array = new IType[typeParameterCount];
			for (int i = 0; i < array.Length; i++)
			{
				if (i < this.typeArguments.Length)
				{
					array[i] = this.typeArguments[i].Resolve(context);
				}
				else
				{
					array[i] = SpecialType.UnknownType;
				}
			}
			return new ParameterizedType(definition, array);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(this.genericType.ToString());
			stringBuilder.Append('[');
			for (int i = 0; i < this.typeArguments.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append('[');
				stringBuilder.Append(this.typeArguments[i].ToString());
				stringBuilder.Append(']');
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		int ISupportsInterning.GetHashCodeForInterning()
		{
			int num = this.genericType.GetHashCode();
			foreach (ITypeReference typeReference in this.typeArguments)
			{
				num *= 27;
				num += typeReference.GetHashCode();
			}
			return num;
		}

		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			ParameterizedTypeReference parameterizedTypeReference = other as ParameterizedTypeReference;
			if (parameterizedTypeReference != null && this.genericType == parameterizedTypeReference.genericType && this.typeArguments.Length == parameterizedTypeReference.typeArguments.Length)
			{
				for (int i = 0; i < this.typeArguments.Length; i++)
				{
					if (this.typeArguments[i] != parameterizedTypeReference.typeArguments[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		private readonly ITypeReference genericType;

		private readonly ITypeReference[] typeArguments;
	}
}
