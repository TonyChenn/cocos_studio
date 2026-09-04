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
	// Token: 0x02000059 RID: 89
	public class AnonymousType : AbstractType
	{
		// Token: 0x060002A7 RID: 679 RVA: 0x00006F64 File Offset: 0x00005F64
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

		// Token: 0x060002A8 RID: 680 RVA: 0x00006FD7 File Offset: 0x00005FD7
		public override ITypeReference ToTypeReference()
		{
			return new AnonymousTypeReference(this.unresolvedProperties);
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x00006FE4 File Offset: 0x00005FE4
		public override string Name
		{
			get
			{
				return "Anonymous Type";
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002AA RID: 682 RVA: 0x00006FEB File Offset: 0x00005FEB
		public override TypeKind Kind
		{
			get
			{
				return TypeKind.Anonymous;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002AB RID: 683 RVA: 0x000070CC File Offset: 0x000060CC
		public override IEnumerable<IType> DirectBaseTypes
		{
			get
			{
				yield return this.compilation.FindType(KnownTypeCode.Object);
				yield break;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002AC RID: 684 RVA: 0x000070E9 File Offset: 0x000060E9
		public override bool? IsReferenceType
		{
			get
			{
				return new bool?(true);
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002AD RID: 685 RVA: 0x000070F1 File Offset: 0x000060F1
		public IList<IProperty> Properties
		{
			get
			{
				return this.resolvedProperties;
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000070F9 File Offset: 0x000060F9
		public override IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return this.compilation.FindType(KnownTypeCode.Object).GetMethods(filter, options);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000711A File Offset: 0x0000611A
		public override IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = GetMemberOptions.None)
		{
			if ((options & GetMemberOptions.IgnoreInheritedMembers) == GetMemberOptions.IgnoreInheritedMembers)
			{
				return EmptyList<IMethod>.Instance;
			}
			return this.compilation.FindType(KnownTypeCode.Object).GetMethods(typeArguments, filter, options);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00007280 File Offset: 0x00006280
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

		// Token: 0x060002B1 RID: 689 RVA: 0x0000748C File Offset: 0x0000648C
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

		// Token: 0x060002B2 RID: 690 RVA: 0x000074B0 File Offset: 0x000064B0
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

		// Token: 0x060002B3 RID: 691 RVA: 0x00007524 File Offset: 0x00006524
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

		// Token: 0x040000BD RID: 189
		private ICompilation compilation;

		// Token: 0x040000BE RID: 190
		private IUnresolvedProperty[] unresolvedProperties;

		// Token: 0x040000BF RID: 191
		private IList<IProperty> resolvedProperties;

		// Token: 0x02000063 RID: 99
		private sealed class AnonymousTypeProperty : DefaultResolvedProperty
		{
			// Token: 0x06000315 RID: 789 RVA: 0x00007C9F File Offset: 0x00006C9F
			public AnonymousTypeProperty(IUnresolvedProperty unresolved, ITypeResolveContext parentContext, AnonymousType declaringType) : base(unresolved, parentContext)
			{
				this.declaringType = declaringType;
			}

			// Token: 0x17000131 RID: 305
			// (get) Token: 0x06000316 RID: 790 RVA: 0x00007CB0 File Offset: 0x00006CB0
			public override IType DeclaringType
			{
				get
				{
					return this.declaringType;
				}
			}

			// Token: 0x06000317 RID: 791 RVA: 0x00007CB8 File Offset: 0x00006CB8
			public override bool Equals(object obj)
			{
				AnonymousType.AnonymousTypeProperty anonymousTypeProperty = obj as AnonymousType.AnonymousTypeProperty;
				return anonymousTypeProperty != null && base.Name == anonymousTypeProperty.Name && this.declaringType.Equals(anonymousTypeProperty.declaringType);
			}

			// Token: 0x06000318 RID: 792 RVA: 0x00007CF5 File Offset: 0x00006CF5
			public override int GetHashCode()
			{
				return this.declaringType.GetHashCode() ^ 27 * base.Name.GetHashCode();
			}

			// Token: 0x06000319 RID: 793 RVA: 0x00007D11 File Offset: 0x00006D11
			protected override IMethod CreateResolvedAccessor(IUnresolvedMethod unresolvedAccessor)
			{
				return new AnonymousType.AnonymousTypeAccessor(unresolvedAccessor, this.context, this);
			}

			// Token: 0x040000CD RID: 205
			private readonly AnonymousType declaringType;
		}

		// Token: 0x02000067 RID: 103
		private sealed class AnonymousTypeAccessor : DefaultResolvedMethod
		{
			// Token: 0x0600035A RID: 858 RVA: 0x0000840E File Offset: 0x0000740E
			public AnonymousTypeAccessor(IUnresolvedMethod unresolved, ITypeResolveContext parentContext, AnonymousType.AnonymousTypeProperty owner) : base(unresolved, parentContext, false)
			{
				this.owner = owner;
			}

			// Token: 0x17000154 RID: 340
			// (get) Token: 0x0600035B RID: 859 RVA: 0x00008420 File Offset: 0x00007420
			public override IMember AccessorOwner
			{
				get
				{
					return this.owner;
				}
			}

			// Token: 0x17000155 RID: 341
			// (get) Token: 0x0600035C RID: 860 RVA: 0x00008428 File Offset: 0x00007428
			public override IType DeclaringType
			{
				get
				{
					return this.owner.DeclaringType;
				}
			}

			// Token: 0x0600035D RID: 861 RVA: 0x00008438 File Offset: 0x00007438
			public override bool Equals(object obj)
			{
				AnonymousType.AnonymousTypeAccessor anonymousTypeAccessor = obj as AnonymousType.AnonymousTypeAccessor;
				return anonymousTypeAccessor != null && base.Name == anonymousTypeAccessor.Name && this.owner.DeclaringType.Equals(anonymousTypeAccessor.owner.DeclaringType);
			}

			// Token: 0x0600035E RID: 862 RVA: 0x0000847F File Offset: 0x0000747F
			public override int GetHashCode()
			{
				return this.owner.DeclaringType.GetHashCode() ^ 27 * base.Name.GetHashCode();
			}

			// Token: 0x040000D6 RID: 214
			private readonly AnonymousType.AnonymousTypeProperty owner;
		}
	}
}
