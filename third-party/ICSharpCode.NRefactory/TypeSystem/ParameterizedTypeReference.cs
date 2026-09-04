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
	// Token: 0x020000F3 RID: 243
	[Serializable]
	public sealed class ParameterizedTypeReference : ITypeReference, ISupportsInterning
	{
		// Token: 0x06000911 RID: 2321 RVA: 0x00018410 File Offset: 0x00017410
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

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x0001848A File Offset: 0x0001748A
		public ITypeReference GenericType
		{
			get
			{
				return this.genericType;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x00018492 File Offset: 0x00017492
		public ReadOnlyCollection<ITypeReference> TypeArguments
		{
			get
			{
				return Array.AsReadOnly<ITypeReference>(this.typeArguments);
			}
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x000184A0 File Offset: 0x000174A0
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

		// Token: 0x06000915 RID: 2325 RVA: 0x0001851C File Offset: 0x0001751C
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

		// Token: 0x06000916 RID: 2326 RVA: 0x00018598 File Offset: 0x00017598
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

		// Token: 0x06000917 RID: 2327 RVA: 0x000185DC File Offset: 0x000175DC
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

		// Token: 0x040002E5 RID: 741
		private readonly ITypeReference genericType;

		// Token: 0x040002E6 RID: 742
		private readonly ITypeReference[] typeArguments;
	}
}
