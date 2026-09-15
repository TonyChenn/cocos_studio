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
	[Serializable]
	public sealed class ParameterizedType : IType, INamedElement, IEquatable<IType>, ICompilationProvider
	{
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
		internal ParameterizedType(ITypeDefinition genericType, IType[] typeArguments)
		{
			this.genericType = genericType;
			this.typeArguments = typeArguments;
		}

		public TypeKind Kind
		{
			get
			{
				return this.genericType.Kind;
			}
		}

		public ICompilation Compilation
		{
			get
			{
				return this.genericType.Compilation;
			}
		}

		public bool? IsReferenceType
		{
			get
			{
				return this.genericType.IsReferenceType;
			}
		}

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

		public int TypeParameterCount
		{
			get
			{
				return this.typeArguments.Length;
			}
		}

		public string FullName
		{
			get
			{
				return this.genericType.FullName;
			}
		}

		public string Name
		{
			get
			{
				return this.genericType.Name;
			}
		}

		public string Namespace
		{
			get
			{
				return this.genericType.Namespace;
			}
		}

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

		public override string ToString()
		{
			return this.ReflectionName;
		}

		public IList<IType> TypeArguments
		{
			get
			{
				return this.typeArguments;
			}
		}

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
		public IType GetTypeArgument(int index)
		{
			return this.typeArguments[index];
		}

		/// <summary>
		/// Gets the definition of the generic type.
		/// For <c>ParameterizedType</c>, this method never returns null.
		/// </summary>
		public ITypeDefinition GetDefinition()
		{
			return this.genericType;
		}

		public ITypeReference ToTypeReference()
		{
			return new ParameterizedTypeReference(this.genericType.ToTypeReference(), from t in this.typeArguments
			select t.ToTypeReference());
		}

		/// <summary>
		/// Gets a type visitor that performs the substitution of class type parameters with the type arguments
		/// of this parameterized type.
		/// </summary>
		public TypeParameterSubstitution GetSubstitution()
		{
			return new TypeParameterSubstitution(this.typeArguments, null);
		}

		/// <summary>
		/// Gets a type visitor that performs the substitution of class type parameters with the type arguments
		/// of this parameterized type,
		/// and also substitutes method type parameters with the specified method type arguments.
		/// </summary>
		public TypeParameterSubstitution GetSubstitution(IList<IType> methodTypeArguments)
		{
			return new TypeParameterSubstitution(this.typeArguments, methodTypeArguments);
		}

		public IEnumerable<IType> DirectBaseTypes
		{
			get
			{
				TypeParameterSubstitution substitution = this.GetSubstitution();
				return from t in this.genericType.DirectBaseTypes
				select t.AcceptVisitor(substitution);
			}
		}

		public IEnumerable<IType> GetNestedTypes(Predicate<ITypeDefinition> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetNestedTypes(filter, options);
			}
			return GetMembersHelper.GetNestedTypes(this, filter, options);
		}

		public IEnumerable<IType> GetNestedTypes(IList<IType> typeArguments, Predicate<ITypeDefinition> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetNestedTypes(typeArguments, filter, options);
			}
			return GetMembersHelper.GetNestedTypes(this, typeArguments, filter, options);
		}

		public IEnumerable<IMethod> GetConstructors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.IgnoreInheritedMembers)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetConstructors(filter, options);
			}
			return GetMembersHelper.GetConstructors(this, filter, options);
		}

		public IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetMethods(filter, options);
			}
			return GetMembersHelper.GetMethods(this, filter, options);
		}

		public IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetMethods(typeArguments, filter, options);
			}
			return GetMembersHelper.GetMethods(this, typeArguments, filter, options);
		}

		public IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetProperties(filter, options);
			}
			return GetMembersHelper.GetProperties(this, filter, options);
		}

		public IEnumerable<IField> GetFields(Predicate<IUnresolvedField> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetFields(filter, options);
			}
			return GetMembersHelper.GetFields(this, filter, options);
		}

		public IEnumerable<IEvent> GetEvents(Predicate<IUnresolvedEvent> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetEvents(filter, options);
			}
			return GetMembersHelper.GetEvents(this, filter, options);
		}

		public IEnumerable<IMember> GetMembers(Predicate<IUnresolvedMember> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetMembers(filter, options);
			}
			return GetMembersHelper.GetMembers(this, filter, options);
		}

		public IEnumerable<IMethod> GetAccessors(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.ReturnMemberDefinitions) == GetMemberOptions.ReturnMemberDefinitions)
			{
				return this.genericType.GetAccessors(filter, options);
			}
			return GetMembersHelper.GetAccessors(this, filter, options);
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as IType);
		}

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

		public IType AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitParameterizedType(this);
		}

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

		private readonly ITypeDefinition genericType;

		private readonly IType[] typeArguments;
	}
}
