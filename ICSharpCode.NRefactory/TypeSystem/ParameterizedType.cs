using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// ParameterizedType represents an instance of a generic type.
	/// Example: List&lt;string&gt;
	/// </summary>
	/// <remarks>
	/// When getting the members, this type modifies the lists so that
	/// type parameters in the signatures of the members are replaced with
	/// the type arguments.
	/// </remarks>
	// Token: 0x020000F2 RID: 242
	[Serializable]
	public sealed class ParameterizedType : IType, INamedElement, IEquatable<IType>, ICompilationProvider
	{
		// Token: 0x060008ED RID: 2285 RVA: 0x00017E44 File Offset: 0x00016E44
		public ParameterizedType(ITypeDefinition genericType, IEnumerable<IType> typeArguments)
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
			this.typeArguments = typeArguments.ToArray<IType>();
			if (this.typeArguments.Length == 0)
			{
				throw new ArgumentException("Cannot use ParameterizedType with 0 type arguments.");
			}
			if (genericType.TypeParameterCount != this.typeArguments.Length)
			{
				throw new ArgumentException("Number of type arguments must match the type definition's number of type parameters");
			}
			for (int i = 0; i < this.typeArguments.Length; i++)
			{
				if (this.typeArguments[i] == null)
				{
					throw new ArgumentNullException("typeArguments[" + i + "]");
				}
				ICompilationProvider compilationProvider = this.typeArguments[i] as ICompilationProvider;
				if (compilationProvider != null && compilationProvider.Compilation != genericType.Compilation)
				{
					throw new InvalidOperationException("Cannot parameterize a type with type arguments from a different compilation.");
				}
			}
		}

		/// <summary>
		/// Fast internal version of the constructor. (no safety checks)
		/// Keeps the array that was passed and assumes it won't be modified.
		/// </summary>
		// Token: 0x060008EE RID: 2286 RVA: 0x00017F18 File Offset: 0x00016F18
		internal ParameterizedType(ITypeDefinition genericType, IType[] typeArguments)
		{
			this.genericType = genericType;
			this.typeArguments = typeArguments;
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x00017F2E File Offset: 0x00016F2E
		public TypeKind Kind
		{
			get
			{
				return this.genericType.Kind;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00017F3B File Offset: 0x00016F3B
		public ICompilation Compilation
		{
			get
			{
				return this.genericType.Compilation;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x00017F48 File Offset: 0x00016F48
		public bool? IsReferenceType
		{
			get
			{
				return this.genericType.IsReferenceType;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00017F58 File Offset: 0x00016F58
		public IType DeclaringType
		{
			get
			{
				ITypeDefinition declaringTypeDefinition = this.genericType.DeclaringTypeDefinition;
				if (declaringTypeDefinition != null && declaringTypeDefinition.TypeParameterCount > 0 && declaringTypeDefinition.TypeParameterCount <= this.genericType.TypeParameterCount)
				{
					IType[] array = new IType[declaringTypeDefinition.TypeParameterCount];
					Array.Copy(this.typeArguments, 0, array, 0, array.Length);
					return new ParameterizedType(declaringTypeDefinition, array);
				}
				return declaringTypeDefinition;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x00017FB6 File Offset: 0x00016FB6
		public int TypeParameterCount
		{
			get
			{
				return this.typeArguments.Length;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x00017FC0 File Offset: 0x00016FC0
		public string FullName
		{
			get
			{
				return this.genericType.FullName;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x00017FCD File Offset: 0x00016FCD
		public string Name
		{
			get
			{
				return this.genericType.Name;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00017FDA File Offset: 0x00016FDA
		public string Namespace
		{
			get
			{
				return this.genericType.Namespace;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00017FE8 File Offset: 0x00016FE8
		public string ReflectionName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(this.genericType.ReflectionName);
				stringBuilder.Append('[');
				for (int i = 0; i < this.typeArguments.Length; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(',');
					}
					stringBuilder.Append('[');
					stringBuilder.Append(this.typeArguments[i].ReflectionName);
					stringBuilder.Append(']');
				}
				stringBuilder.Append(']');
				return stringBuilder.ToString();
			}
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00018064 File Offset: 0x00017064
		public override string ToString()
		{
			return this.ReflectionName;
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x0001806C File Offset: 0x0001706C
		public IList<IType> TypeArguments
		{
			get
			{
				return this.typeArguments;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x00018074 File Offset: 0x00017074
		public bool IsParameterized
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Same as 'parameterizedType.TypeArguments[index]', but is a bit more efficient (doesn't require the read-only wrapper).
		/// </summary>
		// Token: 0x060008FB RID: 2299 RVA: 0x00018077 File Offset: 0x00017077
		public IType GetTypeArgument(int index)
		{
			return this.typeArguments[index];
		}

		/// <summary>
		/// Gets the definition of the generic type.
		/// For <c>ParameterizedType</c>, this method never returns null.
		/// </summary>
		// Token: 0x060008FC RID: 2300 RVA: 0x00018081 File Offset: 0x00017081
		public ITypeDefinition GetDefinition()
		{
			return this.genericType;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00018091 File Offset: 0x00017091
		public ITypeReference ToTypeReference()
		{
			return new ParameterizedTypeReference(this.genericType.ToTypeReference(), from t in this.typeArguments
			select t.ToTypeReference());
		}

		/// <summary>
		/// Gets a type visitor that performs the substitution of class type parameters with the type arguments
		/// of this parameterized type.
		/// </summary>
		// Token: 0x060008FE RID: 2302 RVA: 0x000180CB File Offset: 0x000170CB
		public TypeParameterSubstitution GetSubstitution()
		{
			return new TypeParameterSubstitution(this.typeArguments, null);
		}

		/// <summary>
		/// Gets a type visitor that performs the substitution of class type parameters with the type arguments
		/// of this parameterized type,
		/// and also substitutes method type parameters with the specified method type arguments.
		/// </summary>
		// Token: 0x060008FF RID: 2303 RVA: 0x000180D9 File Offset: 0x000170D9
		public TypeParameterSubstitution GetSubstitution(IList<IType> methodTypeArguments)
		{
			return new TypeParameterSubstitution(this.typeArguments, methodTypeArguments);
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x00018100 File Offset: 0x00017100
		public IEnumerable<IType> DirectBaseTypes
		{
			get
			{
				TypeParameterSubstitution substitution = this.GetSubstitution();
				return from t in this.genericType.DirectBaseTypes
				select t.AcceptVisitor(substitution);
			}
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0001813B File Offset: 0x0001713B
		public IEnumerable<IType> GetNestedTypes(Predicate<ITypeDefinition> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetNestedTypes(filter, options);
			}
			return GetMembersHelper.GetNestedTypes(this, filter, options);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00018159 File Offset: 0x00017159
		public IEnumerable<IType> GetNestedTypes(IList<IType> typeArguments, Predicate<ITypeDefinition> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetNestedTypes(typeArguments, filter, options);
			}
			return GetMembersHelper.GetNestedTypes(this, typeArguments, filter, options);
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00018179 File Offset: 0x00017179
		public IEnumerable<IMethod> GetConstructors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.IgnoreInheritedMembers)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetConstructors(filter, options);
			}
			return GetMembersHelper.GetConstructors(this, filter, options);
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00018197 File Offset: 0x00017197
		public IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetMethods(filter, options);
			}
			return GetMembersHelper.GetMethods(this, filter, options);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x000181B5 File Offset: 0x000171B5
		public IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetMethods(typeArguments, filter, options);
			}
			return GetMembersHelper.GetMethods(this, typeArguments, filter, options);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x000181D5 File Offset: 0x000171D5
		public IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetProperties(filter, options);
			}
			return GetMembersHelper.GetProperties(this, filter, options);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x000181F3 File Offset: 0x000171F3
		public IEnumerable<IField> GetFields(Predicate<IUnresolvedField> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetFields(filter, options);
			}
			return GetMembersHelper.GetFields(this, filter, options);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00018211 File Offset: 0x00017211
		public IEnumerable<IEvent> GetEvents(Predicate<IUnresolvedEvent> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetEvents(filter, options);
			}
			return GetMembersHelper.GetEvents(this, filter, options);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0001822F File Offset: 0x0001722F
		public IEnumerable<IMember> GetMembers(Predicate<IUnresolvedMember> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetMembers(filter, options);
			}
			return GetMembersHelper.GetMembers(this, filter, options);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0001824D File Offset: 0x0001724D
		public IEnumerable<IMethod> GetAccessors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetAccessors(filter, options);
			}
			return GetMembersHelper.GetAccessors(this, filter, options);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0001826B File Offset: 0x0001726B
		public override bool Equals(object obj)
		{
			return this.Equals(obj as IType);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0001827C File Offset: 0x0001727C
		public bool Equals(IType other)
		{
			ParameterizedType parameterizedType = other as ParameterizedType;
			if (parameterizedType == null || !this.genericType.Equals(parameterizedType.genericType) || this.typeArguments.Length != parameterizedType.typeArguments.Length)
			{
				return false;
			}
			for (int i = 0; i < this.typeArguments.Length; i++)
			{
				if (!this.typeArguments[i].Equals(parameterizedType.typeArguments[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x000182E8 File Offset: 0x000172E8
		public override int GetHashCode()
		{
			int num = this.genericType.GetHashCode();
			foreach (IType type in this.typeArguments)
			{
				num *= 1000000007;
				num += 1000000009 * type.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00018332 File Offset: 0x00017332
		public IType AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitParameterizedType(this);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0001833C File Offset: 0x0001733C
		public IType VisitChildren(TypeVisitor visitor)
		{
			IType type = this.genericType.AcceptVisitor(visitor);
			ITypeDefinition typeDefinition = type as ITypeDefinition;
			if (typeDefinition == null)
			{
				return type;
			}
			IType[] array = (type != this.genericType) ? new IType[this.typeArguments.Length] : null;
			for (int i = 0; i < this.typeArguments.Length; i++)
			{
				IType type2 = this.typeArguments[i].AcceptVisitor(visitor);
				if (type2 == null)
				{
					throw new NullReferenceException("TypeVisitor.Visit-method returned null");
				}
				if (array == null && type2 != this.typeArguments[i])
				{
					array = new IType[this.typeArguments.Length];
					for (int j = 0; j < i; j++)
					{
						array[j] = this.typeArguments[j];
					}
				}
				if (array != null)
				{
					array[i] = type2;
				}
			}
			if (typeDefinition == this.genericType && array == null)
			{
				return this;
			}
			return new ParameterizedType(typeDefinition, array ?? this.typeArguments);
		}

		// Token: 0x040002E2 RID: 738
		private readonly ITypeDefinition genericType;

		// Token: 0x040002E3 RID: 739
		private readonly IType[] typeArguments;
	}
}
