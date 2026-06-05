using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IMethod" /> that resolves an unresolved method.
	/// </summary>
	// Token: 0x02000065 RID: 101
	public class DefaultResolvedMethod : AbstractResolvedMember, IMethod, IParameterizedMember, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		// Token: 0x0600032A RID: 810 RVA: 0x00007D20 File Offset: 0x00006D20
		public DefaultResolvedMethod(DefaultUnresolvedMethod unresolved, ITypeResolveContext parentContext) : this(unresolved, parentContext, unresolved.IsExtensionMethod)
		{
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00007D30 File Offset: 0x00006D30
		public DefaultResolvedMethod(IUnresolvedMethod unresolved, ITypeResolveContext parentContext, bool isExtensionMethod) : base(unresolved, parentContext)
		{
			this.Parameters = unresolved.Parameters.CreateResolvedParameters(this.context);
			this.ReturnTypeAttributes = unresolved.ReturnTypeAttributes.CreateResolvedAttributes(parentContext);
			this.TypeParameters = unresolved.TypeParameters.CreateResolvedTypeParameters(this.context);
			this.IsExtensionMethod = isExtensionMethod;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00007D8C File Offset: 0x00006D8C
		public static DefaultResolvedMethod CreateFromMultipleParts(IUnresolvedMethod[] parts, ITypeResolveContext[] contexts, bool isExtensionMethod)
		{
			DefaultResolvedMethod defaultResolvedMethod = new DefaultResolvedMethod(parts[0], contexts[0], isExtensionMethod);
			defaultResolvedMethod.parts = parts;
			if (parts.Length > 1)
			{
				DefaultResolvedMethod.ListOfLists<IAttribute> listOfLists = new DefaultResolvedMethod.ListOfLists<IAttribute>();
				listOfLists.AddList(defaultResolvedMethod.Attributes);
				for (int i = 1; i < parts.Length; i++)
				{
					listOfLists.AddList(parts[i].Attributes.CreateResolvedAttributes(contexts[i]));
				}
				defaultResolvedMethod.Attributes = listOfLists;
			}
			return defaultResolvedMethod;
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600032D RID: 813 RVA: 0x00007DF1 File Offset: 0x00006DF1
		// (set) Token: 0x0600032E RID: 814 RVA: 0x00007DF9 File Offset: 0x00006DF9
		public IList<IParameter> Parameters { get; private set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600032F RID: 815 RVA: 0x00007E02 File Offset: 0x00006E02
		// (set) Token: 0x06000330 RID: 816 RVA: 0x00007E0A File Offset: 0x00006E0A
		public IList<IAttribute> ReturnTypeAttributes { get; private set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000331 RID: 817 RVA: 0x00007E13 File Offset: 0x00006E13
		// (set) Token: 0x06000332 RID: 818 RVA: 0x00007E1B File Offset: 0x00006E1B
		public IList<ITypeParameter> TypeParameters { get; private set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000333 RID: 819 RVA: 0x00007E24 File Offset: 0x00006E24
		public IList<IType> TypeArguments
		{
			get
			{
				return this.TypeParameters.ToList<IType>();
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00007E31 File Offset: 0x00006E31
		bool IMethod.IsParameterized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00007E34 File Offset: 0x00006E34
		// (set) Token: 0x06000336 RID: 822 RVA: 0x00007E3C File Offset: 0x00006E3C
		public bool IsExtensionMethod { get; private set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000337 RID: 823 RVA: 0x00007E48 File Offset: 0x00006E48
		public IList<IUnresolvedMethod> Parts
		{
			get
			{
				IUnresolvedMethod[] result;
				if ((result = this.parts) == null)
				{
					result = new IUnresolvedMethod[]
					{
						(IUnresolvedMethod)this.unresolved
					};
				}
				return result;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00007E75 File Offset: 0x00006E75
		public bool IsConstructor
		{
			get
			{
				return ((IUnresolvedMethod)this.unresolved).IsConstructor;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00007E87 File Offset: 0x00006E87
		public bool IsDestructor
		{
			get
			{
				return ((IUnresolvedMethod)this.unresolved).IsDestructor;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600033A RID: 826 RVA: 0x00007E99 File Offset: 0x00006E99
		public bool IsOperator
		{
			get
			{
				return ((IUnresolvedMethod)this.unresolved).IsOperator;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00007EAB File Offset: 0x00006EAB
		public bool IsPartial
		{
			get
			{
				return ((IUnresolvedMethod)this.unresolved).IsPartial;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600033C RID: 828 RVA: 0x00007EBD File Offset: 0x00006EBD
		public bool IsAsync
		{
			get
			{
				return ((IUnresolvedMethod)this.unresolved).IsAsync;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00007ECF File Offset: 0x00006ECF
		public bool HasBody
		{
			get
			{
				return ((IUnresolvedMethod)this.unresolved).HasBody;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00007EE1 File Offset: 0x00006EE1
		public bool IsAccessor
		{
			get
			{
				return ((IUnresolvedMethod)this.unresolved).AccessorOwner != null;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600033F RID: 831 RVA: 0x00007EF9 File Offset: 0x00006EF9
		IMethod IMethod.ReducedFrom
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00007EFC File Offset: 0x00006EFC
		public virtual IMember AccessorOwner
		{
			get
			{
				IUnresolvedMember accessorOwner = ((IUnresolvedMethod)this.unresolved).AccessorOwner;
				if (accessorOwner != null)
				{
					return accessorOwner.Resolve(this.context);
				}
				return null;
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00007F38 File Offset: 0x00006F38
		public override ISymbolReference ToReference()
		{
			ITypeReference typeReference = this.DeclaringType.ToTypeReference();
			if (base.IsExplicitInterfaceImplementation && base.ImplementedInterfaceMembers.Count == 1)
			{
				return new ExplicitInterfaceImplementationMemberReference(typeReference, base.ImplementedInterfaceMembers[0].ToReference());
			}
			return new DefaultMemberReference(base.SymbolKind, typeReference, base.Name, this.TypeParameters.Count, (from p in this.Parameters
			select p.Type.ToTypeReference()).ToList<ITypeReference>());
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00007FC9 File Offset: 0x00006FC9
		public override IMemberReference ToMemberReference()
		{
			return (IMemberReference)this.ToReference();
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00007FD6 File Offset: 0x00006FD6
		public override IMember Specialize(TypeParameterSubstitution substitution)
		{
			if (TypeParameterSubstitution.Identity.Equals(substitution))
			{
				return this;
			}
			return new SpecializedMethod(this, substitution);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00007FEE File Offset: 0x00006FEE
		IMethod IMethod.Specialize(TypeParameterSubstitution substitution)
		{
			if (TypeParameterSubstitution.Identity.Equals(substitution))
			{
				return this;
			}
			return new SpecializedMethod(this, substitution);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00008008 File Offset: 0x00007008
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("[");
			stringBuilder.Append(base.SymbolKind);
			stringBuilder.Append(' ');
			if (this.DeclaringType.Kind != TypeKind.Unknown)
			{
				stringBuilder.Append(this.DeclaringType.ReflectionName);
				stringBuilder.Append('.');
			}
			stringBuilder.Append(base.Name);
			if (this.TypeParameters.Count > 0)
			{
				stringBuilder.Append("``");
				stringBuilder.Append(this.TypeParameters.Count);
			}
			stringBuilder.Append('(');
			for (int i = 0; i < this.Parameters.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(this.Parameters[i].ToString());
			}
			stringBuilder.Append("):");
			stringBuilder.Append(base.ReturnType.ReflectionName);
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		/// <summary>
		/// Gets a dummy constructor for the specified compilation.
		/// </summary>
		/// <returns>
		/// A public instance constructor with IsSynthetic=true and no declaring type.
		/// </returns>
		/// <seealso cref="P:ICSharpCode.NRefactory.TypeSystem.Implementation.DefaultUnresolvedMethod.DummyConstructor" />
		// Token: 0x06000346 RID: 838 RVA: 0x00008134 File Offset: 0x00007134
		public static IMethod GetDummyConstructor(ICompilation compilation)
		{
			IUnresolvedMethod dummyConstructor = DefaultUnresolvedMethod.DummyConstructor;
			return (IMethod)compilation.CacheManager.GetOrAddShared(dummyConstructor, (object _) => dummyConstructor.CreateResolved(compilation.TypeResolveContext));
		}

		/// <summary>
		/// Gets a dummy constructor for the specified type.
		/// </summary>
		/// <returns>
		/// A public instance constructor with IsSynthetic=true and the specified declaring type.
		/// </returns>
		/// <seealso cref="P:ICSharpCode.NRefactory.TypeSystem.Implementation.DefaultUnresolvedMethod.DummyConstructor" />
		// Token: 0x06000347 RID: 839 RVA: 0x00008180 File Offset: 0x00007180
		public static IMethod GetDummyConstructor(ICompilation compilation, IType declaringType)
		{
			IMethod dummyConstructor = DefaultResolvedMethod.GetDummyConstructor(compilation);
			return new SpecializedMethod(dummyConstructor, TypeParameterSubstitution.Identity)
			{
				DeclaringType = declaringType
			};
		}

		// Token: 0x040000CE RID: 206
		private IUnresolvedMethod[] parts;

		// Token: 0x02000066 RID: 102
		private class ListOfLists<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
		{
			// Token: 0x06000349 RID: 841 RVA: 0x000081A8 File Offset: 0x000071A8
			public void AddList(IList<T> list)
			{
				this.lists.Add(list);
			}

			// Token: 0x0600034A RID: 842 RVA: 0x000081B6 File Offset: 0x000071B6
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x0600034B RID: 843 RVA: 0x0000826C File Offset: 0x0000726C
			public IEnumerator<T> GetEnumerator()
			{
				for (int i = 0; i < this.Count; i++)
				{
					yield return this[i];
				}
				yield break;
			}

			// Token: 0x0600034C RID: 844 RVA: 0x00008288 File Offset: 0x00007288
			public void Add(T item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600034D RID: 845 RVA: 0x0000828F File Offset: 0x0000728F
			public void Clear()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600034E RID: 846 RVA: 0x00008298 File Offset: 0x00007298
			public bool Contains(T item)
			{
				EqualityComparer<T> @default = EqualityComparer<T>.Default;
				for (int i = 0; i < this.Count; i++)
				{
					if (@default.Equals(this[i], item))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600034F RID: 847 RVA: 0x000082D0 File Offset: 0x000072D0
			public void CopyTo(T[] array, int arrayIndex)
			{
				for (int i = 0; i < this.Count; i++)
				{
					array[arrayIndex + i] = this[i];
				}
			}

			// Token: 0x06000350 RID: 848 RVA: 0x000082FE File Offset: 0x000072FE
			public bool Remove(T item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x17000151 RID: 337
			// (get) Token: 0x06000351 RID: 849 RVA: 0x0000830D File Offset: 0x0000730D
			public int Count
			{
				get
				{
					return this.lists.Sum((IList<T> l) => l.Count);
				}
			}

			// Token: 0x17000152 RID: 338
			// (get) Token: 0x06000352 RID: 850 RVA: 0x00008337 File Offset: 0x00007337
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06000353 RID: 851 RVA: 0x0000833C File Offset: 0x0000733C
			public int IndexOf(T item)
			{
				EqualityComparer<T> @default = EqualityComparer<T>.Default;
				for (int i = 0; i < this.Count; i++)
				{
					if (@default.Equals(this[i], item))
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06000354 RID: 852 RVA: 0x00008373 File Offset: 0x00007373
			public void Insert(int index, T item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000355 RID: 853 RVA: 0x0000837A File Offset: 0x0000737A
			public void RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x17000153 RID: 339
			public T this[int index]
			{
				get
				{
					foreach (IList<T> list in this.lists)
					{
						if (index < list.Count)
						{
							return list[index];
						}
						index -= list.Count;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x040000D4 RID: 212
			private List<IList<T>> lists = new List<IList<T>>();
		}
	}
}
