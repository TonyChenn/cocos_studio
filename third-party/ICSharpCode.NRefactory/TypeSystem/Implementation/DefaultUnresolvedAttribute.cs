using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedAttribute" />.
	/// </summary>
	[Serializable]
	public sealed class DefaultUnresolvedAttribute : AbstractFreezable, IUnresolvedAttribute, IFreezable, ISupportsInterning
	{
		public DefaultUnresolvedAttribute(ITypeReference attributeType)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			this.attributeType = attributeType;
		}

		public DefaultUnresolvedAttribute(ITypeReference attributeType, IEnumerable<ITypeReference> constructorParameterTypes)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			this.attributeType = attributeType;
			this.ConstructorParameterTypes.AddRange(constructorParameterTypes);
		}

		protected override void FreezeInternal()
		{
			base.FreezeInternal();
			this.constructorParameterTypes = FreezableHelper.FreezeList<ITypeReference>(this.constructorParameterTypes);
			this.positionalArguments = FreezableHelper.FreezeListAndElements<IConstantValue>(this.positionalArguments);
			this.namedArguments = FreezableHelper.FreezeList<KeyValuePair<IMemberReference, IConstantValue>>(this.namedArguments);
			foreach (KeyValuePair<IMemberReference, IConstantValue> keyValuePair in this.namedArguments)
			{
				FreezableHelper.Freeze(keyValuePair.Key);
				FreezableHelper.Freeze(keyValuePair.Value);
			}
		}

		public ITypeReference AttributeType
		{
			get
			{
				return this.attributeType;
			}
		}

		public DomRegion Region
		{
			get
			{
				return this.region;
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.region = value;
			}
		}

		public IList<ITypeReference> ConstructorParameterTypes
		{
			get
			{
				if (this.constructorParameterTypes == null)
				{
					this.constructorParameterTypes = new List<ITypeReference>();
				}
				return this.constructorParameterTypes;
			}
		}

		public IList<IConstantValue> PositionalArguments
		{
			get
			{
				if (this.positionalArguments == null)
				{
					this.positionalArguments = new List<IConstantValue>();
				}
				return this.positionalArguments;
			}
		}

		public IList<KeyValuePair<IMemberReference, IConstantValue>> NamedArguments
		{
			get
			{
				if (this.namedArguments == null)
				{
					this.namedArguments = new List<KeyValuePair<IMemberReference, IConstantValue>>();
				}
				return this.namedArguments;
			}
		}

		public void AddNamedFieldArgument(string fieldName, IConstantValue value)
		{
			this.NamedArguments.Add(new KeyValuePair<IMemberReference, IConstantValue>(new DefaultMemberReference(SymbolKind.Field, this.attributeType, fieldName, 0, null), value));
		}

		public void AddNamedPropertyArgument(string propertyName, IConstantValue value)
		{
			this.NamedArguments.Add(new KeyValuePair<IMemberReference, IConstantValue>(new DefaultMemberReference(SymbolKind.Property, this.attributeType, propertyName, 0, null), value));
		}

		public IAttribute CreateResolvedAttribute(ITypeResolveContext context)
		{
			return new DefaultUnresolvedAttribute.DefaultResolvedAttribute(this, context);
		}

		int ISupportsInterning.GetHashCodeForInterning()
		{
			int num = this.attributeType.GetHashCode() ^ this.constructorParameterTypes.GetHashCode();
			if (this.constructorParameterTypes != null)
			{
				foreach (ITypeReference typeReference in this.constructorParameterTypes)
				{
					num *= 27;
					num += typeReference.GetHashCode();
				}
			}
			if (this.positionalArguments != null)
			{
				foreach (IConstantValue constantValue in this.positionalArguments)
				{
					num *= 31;
					num += constantValue.GetHashCode();
				}
			}
			if (this.namedArguments != null)
			{
				foreach (KeyValuePair<IMemberReference, IConstantValue> keyValuePair in this.namedArguments)
				{
					num *= 71;
					num += keyValuePair.Key.GetHashCode() + keyValuePair.Value.GetHashCode() * 73;
				}
			}
			return num;
		}

		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			DefaultUnresolvedAttribute defaultUnresolvedAttribute = other as DefaultUnresolvedAttribute;
			return defaultUnresolvedAttribute != null && this.attributeType == defaultUnresolvedAttribute.attributeType && DefaultUnresolvedAttribute.ListEquals<ITypeReference>(this.constructorParameterTypes, defaultUnresolvedAttribute.constructorParameterTypes) && DefaultUnresolvedAttribute.ListEquals<IConstantValue>(this.positionalArguments, defaultUnresolvedAttribute.positionalArguments) && DefaultUnresolvedAttribute.ListEquals(this.namedArguments ?? EmptyList<KeyValuePair<IMemberReference, IConstantValue>>.Instance, defaultUnresolvedAttribute.namedArguments ?? EmptyList<KeyValuePair<IMemberReference, IConstantValue>>.Instance);
		}

		private static bool ListEquals<T>(IList<T> list1, IList<T> list2) where T : class
		{
			if (list1 == null)
			{
				list1 = EmptyList<T>.Instance;
			}
			if (list2 == null)
			{
				list2 = EmptyList<T>.Instance;
			}
			if (list1 == list2)
			{
				return true;
			}
			if (list1.Count != list2.Count)
			{
				return false;
			}
			for (int i = 0; i < list1.Count; i++)
			{
				if (list1[i] != list2[i])
				{
					return false;
				}
			}
			return true;
		}

		private static bool ListEquals(IList<KeyValuePair<IMemberReference, IConstantValue>> list1, IList<KeyValuePair<IMemberReference, IConstantValue>> list2)
		{
			if (list1 == list2)
			{
				return true;
			}
			if (list1.Count != list2.Count)
			{
				return false;
			}
			for (int i = 0; i < list1.Count; i++)
			{
				KeyValuePair<IMemberReference, IConstantValue> keyValuePair = list1[i];
				KeyValuePair<IMemberReference, IConstantValue> keyValuePair2 = list2[i];
				if (keyValuePair.Key != keyValuePair2.Key || keyValuePair.Value != keyValuePair2.Value)
				{
					return false;
				}
			}
			return true;
		}

		private ITypeReference attributeType;

		private DomRegion region;

		private IList<ITypeReference> constructorParameterTypes;

		private IList<IConstantValue> positionalArguments;

		private IList<KeyValuePair<IMemberReference, IConstantValue>> namedArguments;

		private sealed class DefaultResolvedAttribute : IAttribute, ICompilationProvider
		{
			public DefaultResolvedAttribute(DefaultUnresolvedAttribute unresolved, ITypeResolveContext context)
			{
				this.unresolved = unresolved;
				this.context = context;
				this.attributeType = unresolved.AttributeType.Resolve(context);
				this.positionalArguments = unresolved.PositionalArguments.Resolve(context);
			}

			public IType AttributeType
			{
				get
				{
					return this.attributeType;
				}
			}

			public DomRegion Region
			{
				get
				{
					return this.unresolved.Region;
				}
			}

			public IMethod Constructor
			{
				get
				{
					if (!this.constructorResolved)
					{
						this.constructor = this.ResolveConstructor();
						this.constructorResolved = true;
					}
					return this.constructor;
				}
			}

			private IMethod ResolveConstructor()
			{
				IList<IType> parameterTypes = this.unresolved.ConstructorParameterTypes.Resolve(this.context);
				foreach (IMethod method in this.attributeType.GetConstructors((IUnresolvedMethod m) => m.Parameters.Count == parameterTypes.Count, GetMemberOptions.IgnoreInheritedMembers))
				{
					bool flag = true;
					for (int i = 0; i < parameterTypes.Count; i++)
					{
						if (!method.Parameters[i].Type.Equals(parameterTypes[i]))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return method;
					}
				}
				return null;
			}

			public IList<ResolveResult> PositionalArguments
			{
				get
				{
					return this.positionalArguments;
				}
			}

			public IList<KeyValuePair<IMember, ResolveResult>> NamedArguments
			{
				get
				{
					IList<KeyValuePair<IMember, ResolveResult>> list = LazyInit.VolatileRead<IList<KeyValuePair<IMember, ResolveResult>>>(ref this.namedArguments);
					if (list != null)
					{
						return list;
					}
					list = new List<KeyValuePair<IMember, ResolveResult>>();
					foreach (KeyValuePair<IMemberReference, IConstantValue> keyValuePair in this.unresolved.NamedArguments)
					{
						IMember member = keyValuePair.Key.Resolve(this.context);
						if (member != null)
						{
							ResolveResult value = keyValuePair.Value.Resolve(this.context);
							list.Add(new KeyValuePair<IMember, ResolveResult>(member, value));
						}
					}
					return LazyInit.GetOrSet<IList<KeyValuePair<IMember, ResolveResult>>>(ref this.namedArguments, list);
				}
			}

			public ICompilation Compilation
			{
				get
				{
					return this.context.Compilation;
				}
			}

			public override string ToString()
			{
				if (this.positionalArguments.Count == 0)
				{
					return "[" + this.attributeType.ToString() + "]";
				}
				return "[" + this.attributeType.ToString() + "(...)]";
			}

			private readonly DefaultUnresolvedAttribute unresolved;

			private readonly ITypeResolveContext context;

			private readonly IType attributeType;

			private readonly IList<ResolveResult> positionalArguments;

			private IList<KeyValuePair<IMember, ResolveResult>> namedArguments;

			private IMethod constructor;

			private volatile bool constructorResolved;
		}
	}
}
