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
	public class DefaultResolvedMethod : AbstractResolvedMember, IMethod, IParameterizedMember, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		public DefaultResolvedMethod(DefaultUnresolvedMethod unresolved, ITypeResolveContext parentContext) : this(unresolved, parentContext, unresolved.IsExtensionMethod)
		{
		}

		public DefaultResolvedMethod(IUnresolvedMethod unresolved, ITypeResolveContext parentContext, bool isExtensionMethod) : base(unresolved, parentContext)
		{
			this.Parameters = unresolved.Parameters.CreateResolvedParameters(this.context);
			this.ReturnTypeAttributes = unresolved.ReturnTypeAttributes.CreateResolvedAttributes(parentContext);
			this.TypeParameters = unresolved.TypeParameters.CreateResolvedTypeParameters(this.context);
			this.IsExtensionMethod = isExtensionMethod;
		}

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

		public IList<IParameter> Parameters { get; private set; }

		public IList<IAttribute> ReturnTypeAttributes { get; private set; }

		public IList<ITypeParameter> TypeParameters { get; private set; }

		public IList<IType> TypeArguments
		{
			get
			{
				return this.TypeParameters.ToList<IType>();
			}
		}

		bool IMethod.IsParameterized
		{
			get
			{
				return false;
			}
		}

		public bool IsExtensionMethod { get; private set; }

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

		public bool IsConstructor
		{
			get
			{
				return ((IUnresolvedMethod)this.unresolved).IsConstructor;
			}
		}

		public bool IsDestructor
		{
			get
			{
				return ((IUnresolvedMethod)this.unresolved).IsDestructor;
			}
		}

		public bool IsOperator
		{
			get
			{
				return ((IUnresolvedMethod)this.unresolved).IsOperator;
			}
		}

		public bool IsPartial
		{
			get
			{
				return ((IUnresolvedMethod)this.unresolved).IsPartial;
			}
		}

		public bool IsAsync
		{
			get
			{
				return ((IUnresolvedMethod)this.unresolved).IsAsync;
			}
		}

		public bool HasBody
		{
			get
			{
				return ((IUnresolvedMethod)this.unresolved).HasBody;
			}
		}

		public bool IsAccessor
		{
			get
			{
				return ((IUnresolvedMethod)this.unresolved).AccessorOwner != null;
			}
		}

		IMethod IMethod.ReducedFrom
		{
			get
			{
				return null;
			}
		}

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

		public override IMemberReference ToMemberReference()
		{
			return (IMemberReference)this.ToReference();
		}

		public override IMember Specialize(TypeParameterSubstitution substitution)
		{
			if (TypeParameterSubstitution.Identity.Equals(substitution))
			{
				return this;
			}
			return new SpecializedMethod(this, substitution);
		}

		IMethod IMethod.Specialize(TypeParameterSubstitution substitution)
		{
			if (TypeParameterSubstitution.Identity.Equals(substitution))
			{
				return this;
			}
			return new SpecializedMethod(this, substitution);
		}

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
		public static IMethod GetDummyConstructor(ICompilation compilation, IType declaringType)
		{
			IMethod dummyConstructor = DefaultResolvedMethod.GetDummyConstructor(compilation);
			return new SpecializedMethod(dummyConstructor, TypeParameterSubstitution.Identity)
			{
				DeclaringType = declaringType
			};
		}

		private IUnresolvedMethod[] parts;

		private class ListOfLists<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
		{
			public void AddList(IList<T> list)
			{
				this.lists.Add(list);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			public IEnumerator<T> GetEnumerator()
			{
				for (int i = 0; i < this.Count; i++)
				{
					yield return this[i];
				}
				yield break;
			}

			public void Add(T item)
			{
				throw new NotSupportedException();
			}

			public void Clear()
			{
				throw new NotSupportedException();
			}

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

			public void CopyTo(T[] array, int arrayIndex)
			{
				for (int i = 0; i < this.Count; i++)
				{
					array[arrayIndex + i] = this[i];
				}
			}

			public bool Remove(T item)
			{
				throw new NotSupportedException();
			}

			public int Count
			{
				get
				{
					return this.lists.Sum((IList<T> l) => l.Count);
				}
			}

			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

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

			public void Insert(int index, T item)
			{
				throw new NotSupportedException();
			}

			public void RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

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

			private List<IList<T>> lists = new List<IList<T>>();
		}
	}
}
