using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Anonymous type.
	/// </summary>
	public class AnonymousType : AbstractType
	{
		public AnonymousType(ICompilation compilation, IList<IUnresolvedProperty> properties)
		{
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}
			this.compilation = compilation;
			this.unresolvedProperties = properties.ToArray<IUnresolvedProperty>();
			SimpleTypeResolveContext context = new SimpleTypeResolveContext(compilation.MainAssembly);
			this.resolvedProperties = new ProjectedList<ITypeResolveContext, IUnresolvedProperty, IProperty>(context, this.unresolvedProperties, (ITypeResolveContext c, IUnresolvedProperty p) => new AnonymousType.AnonymousTypeProperty(p, c, this));
		}

		public override ITypeReference ToTypeReference()
		{
			return new AnonymousTypeReference(this.unresolvedProperties);
		}

		public override string Name
		{
			get
			{
				return "Anonymous Type";
			}
		}

		public override TypeKind Kind
		{
			get
			{
				return TypeKind.Anonymous;
			}
		}

		public override IEnumerable<IType> DirectBaseTypes
		{
			get
			{
				yield return this.compilation.FindType(KnownTypeCode.Object);
				yield break;
			}
		}

		public override bool? IsReferenceType
		{
			get
			{
				return new bool?(true);
			}
		}

		public IList<IProperty> Properties
		{
			get
			{
				return this.resolvedProperties;
			}
		}

		public override IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return this.compilation.FindType(KnownTypeCode.Object).GetMethods(filter, options);
		}

		public override IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return this.compilation.FindType(KnownTypeCode.Object).GetMethods(typeArguments, filter, options);
		}

		public override IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			for (int i = 0; i < this.unresolvedProperties.Length; i++)
			{
				if (filter == null || filter(this.unresolvedProperties[i]))
				{
					yield return this.resolvedProperties[i];
				}
			}
			yield break;
		}

		public override IEnumerable<IMethod> GetAccessors(Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			for (int i = 0; i < this.unresolvedProperties.Length; i++)
			{
				if (this.unresolvedProperties[i].CanGet && (filter == null || filter(this.unresolvedProperties[i].Getter)))
				{
					yield return this.resolvedProperties[i].Getter;
				}
				if (this.unresolvedProperties[i].CanSet && (filter == null || filter(this.unresolvedProperties[i].Setter)))
				{
					yield return this.resolvedProperties[i].Setter;
				}
			}
			yield break;
		}

		public override int GetHashCode()
		{
			int num = this.resolvedProperties.Count;
			foreach (IProperty property in this.resolvedProperties)
			{
				num *= 31;
				num += (property.Name.GetHashCode() ^ property.ReturnType.GetHashCode());
			}
			return num;
		}

		public override bool Equals(IType other)
		{
			AnonymousType anonymousType = other as AnonymousType;
			if (anonymousType == null || this.resolvedProperties.Count != anonymousType.resolvedProperties.Count)
			{
				return false;
			}
			for (int i = 0; i < this.resolvedProperties.Count; i++)
			{
				IProperty property = this.resolvedProperties[i];
				IProperty property2 = anonymousType.resolvedProperties[i];
				if (property.Name != property2.Name || !property.ReturnType.Equals(property2.ReturnType))
				{
					return false;
				}
			}
			return true;
		}

		private ICompilation compilation;

		private IUnresolvedProperty[] unresolvedProperties;

		private IList<IProperty> resolvedProperties;

		private sealed class AnonymousTypeProperty : DefaultResolvedProperty
		{
			public AnonymousTypeProperty(IUnresolvedProperty unresolved, ITypeResolveContext parentContext, AnonymousType declaringType) : base(unresolved, parentContext)
			{
				this.declaringType = declaringType;
			}

			public override IType DeclaringType
			{
				get
				{
					return this.declaringType;
				}
			}

			public override bool Equals(object obj)
			{
				AnonymousType.AnonymousTypeProperty anonymousTypeProperty = obj as AnonymousType.AnonymousTypeProperty;
				return anonymousTypeProperty != null && base.Name == anonymousTypeProperty.Name && this.declaringType.Equals(anonymousTypeProperty.declaringType);
			}

			public override int GetHashCode()
			{
				return this.declaringType.GetHashCode() ^ 27 * base.Name.GetHashCode();
			}

			protected override IMethod CreateResolvedAccessor(IUnresolvedMethod unresolvedAccessor)
			{
				return new AnonymousType.AnonymousTypeAccessor(unresolvedAccessor, this.context, this);
			}

			private readonly AnonymousType declaringType;
		}

		private sealed class AnonymousTypeAccessor : DefaultResolvedMethod
		{
			public AnonymousTypeAccessor(IUnresolvedMethod unresolved, ITypeResolveContext parentContext, AnonymousType.AnonymousTypeProperty owner) : base(unresolved, parentContext, false)
			{
				this.owner = owner;
			}

			public override IMember AccessorOwner
			{
				get
				{
					return this.owner;
				}
			}

			public override IType DeclaringType
			{
				get
				{
					return this.owner.DeclaringType;
				}
			}

			public override bool Equals(object obj)
			{
				AnonymousType.AnonymousTypeAccessor anonymousTypeAccessor = obj as AnonymousType.AnonymousTypeAccessor;
				return anonymousTypeAccessor != null && base.Name == anonymousTypeAccessor.Name && this.owner.DeclaringType.Equals(anonymousTypeAccessor.owner.DeclaringType);
			}

			public override int GetHashCode()
			{
				return this.owner.DeclaringType.GetHashCode() ^ 27 * base.Name.GetHashCode();
			}

			private readonly AnonymousType.AnonymousTypeProperty owner;
		}
	}
}
