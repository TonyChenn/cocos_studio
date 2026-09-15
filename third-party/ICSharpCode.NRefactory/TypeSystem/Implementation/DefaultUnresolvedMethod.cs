using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedMethod" /> interface.
	/// </summary>
	[Serializable]
	public class DefaultUnresolvedMethod : AbstractUnresolvedMember, IUnresolvedMethod, IUnresolvedParameterizedMember, IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		protected override void FreezeInternal()
		{
			this.returnTypeAttributes = FreezableHelper.FreezeListAndElements<IUnresolvedAttribute>(this.returnTypeAttributes);
			this.typeParameters = FreezableHelper.FreezeListAndElements<IUnresolvedTypeParameter>(this.typeParameters);
			this.parameters = FreezableHelper.FreezeListAndElements<IUnresolvedParameter>(this.parameters);
			base.FreezeInternal();
		}

		public override object Clone()
		{
			DefaultUnresolvedMethod defaultUnresolvedMethod = (DefaultUnresolvedMethod)base.Clone();
			if (this.returnTypeAttributes != null)
			{
				defaultUnresolvedMethod.returnTypeAttributes = new List<IUnresolvedAttribute>(this.returnTypeAttributes);
			}
			if (this.typeParameters != null)
			{
				defaultUnresolvedMethod.typeParameters = new List<IUnresolvedTypeParameter>(this.typeParameters);
			}
			if (this.parameters != null)
			{
				defaultUnresolvedMethod.parameters = new List<IUnresolvedParameter>(this.parameters);
			}
			return defaultUnresolvedMethod;
		}

		public override void ApplyInterningProvider(InterningProvider provider)
		{
			base.ApplyInterningProvider(provider);
			if (provider != null)
			{
				this.returnTypeAttributes = provider.InternList<IUnresolvedAttribute>(this.returnTypeAttributes);
				this.typeParameters = provider.InternList<IUnresolvedTypeParameter>(this.typeParameters);
				this.parameters = provider.InternList<IUnresolvedParameter>(this.parameters);
			}
		}

		public DefaultUnresolvedMethod()
		{
			base.SymbolKind = SymbolKind.Method;
		}

		public DefaultUnresolvedMethod(IUnresolvedTypeDefinition declaringType, string name)
		{
			base.SymbolKind = SymbolKind.Method;
			base.DeclaringTypeDefinition = declaringType;
			base.Name = name;
			if (declaringType != null)
			{
				base.UnresolvedFile = declaringType.UnresolvedFile;
			}
		}

		public IList<IUnresolvedAttribute> ReturnTypeAttributes
		{
			get
			{
				if (this.returnTypeAttributes == null)
				{
					this.returnTypeAttributes = new List<IUnresolvedAttribute>();
				}
				return this.returnTypeAttributes;
			}
		}

		public IList<IUnresolvedTypeParameter> TypeParameters
		{
			get
			{
				if (this.typeParameters == null)
				{
					this.typeParameters = new List<IUnresolvedTypeParameter>();
				}
				return this.typeParameters;
			}
		}

		public bool IsExtensionMethod
		{
			get
			{
				return this.flags[4096];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[4096] = value;
			}
		}

		public bool IsConstructor
		{
			get
			{
				return base.SymbolKind == SymbolKind.Constructor;
			}
		}

		public bool IsDestructor
		{
			get
			{
				return base.SymbolKind == SymbolKind.Destructor;
			}
		}

		public bool IsOperator
		{
			get
			{
				return base.SymbolKind == SymbolKind.Operator;
			}
		}

		public bool IsPartial
		{
			get
			{
				return this.flags[8192];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[8192] = value;
			}
		}

		public bool IsAsync
		{
			get
			{
				return this.flags[32768];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[32768] = value;
			}
		}

		public bool HasBody
		{
			get
			{
				return this.flags[16384];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[16384] = value;
			}
		}

		[Obsolete]
		public bool IsPartialMethodDeclaration
		{
			get
			{
				return this.IsPartial && !this.HasBody;
			}
			set
			{
				if (value)
				{
					this.IsPartial = true;
					this.HasBody = false;
					return;
				}
				if (!value && this.IsPartial && !this.HasBody)
				{
					this.IsPartial = false;
				}
			}
		}

		[Obsolete]
		public bool IsPartialMethodImplementation
		{
			get
			{
				return this.IsPartial && this.HasBody;
			}
			set
			{
				if (value)
				{
					this.IsPartial = true;
					this.HasBody = true;
					return;
				}
				if (!value && this.IsPartial && this.HasBody)
				{
					this.IsPartial = false;
				}
			}
		}

		public IList<IUnresolvedParameter> Parameters
		{
			get
			{
				if (this.parameters == null)
				{
					this.parameters = new List<IUnresolvedParameter>();
				}
				return this.parameters;
			}
		}

		public IUnresolvedMember AccessorOwner
		{
			get
			{
				return this.accessorOwner;
			}
			set
			{
				base.ThrowIfFrozen();
				this.accessorOwner = value;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("[");
			stringBuilder.Append(base.SymbolKind.ToString());
			stringBuilder.Append(' ');
			if (base.DeclaringTypeDefinition != null)
			{
				stringBuilder.Append(base.DeclaringTypeDefinition.Name);
				stringBuilder.Append('.');
			}
			stringBuilder.Append(base.Name);
			stringBuilder.Append('(');
			stringBuilder.Append(string.Join<IUnresolvedParameter>(", ", this.Parameters));
			stringBuilder.Append("):");
			stringBuilder.Append(base.ReturnType.ToString());
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		public override IMember CreateResolved(ITypeResolveContext context)
		{
			return new DefaultResolvedMethod(this, context);
		}

		public override IMember Resolve(ITypeResolveContext context)
		{
			if (this.accessorOwner != null)
			{
				IMember member = this.accessorOwner.Resolve(context);
				if (member != null)
				{
					IProperty property = member as IProperty;
					if (property != null)
					{
						if (property.CanGet && property.Getter.Name == base.Name)
						{
							return property.Getter;
						}
						if (property.CanSet && property.Setter.Name == base.Name)
						{
							return property.Setter;
						}
					}
					IEvent @event = member as IEvent;
					if (@event != null)
					{
						if (@event.CanAdd && @event.AddAccessor.Name == base.Name)
						{
							return @event.AddAccessor;
						}
						if (@event.CanRemove && @event.RemoveAccessor.Name == base.Name)
						{
							return @event.RemoveAccessor;
						}
						if (@event.CanInvoke && @event.InvokeAccessor.Name == base.Name)
						{
							return @event.InvokeAccessor;
						}
					}
				}
				return null;
			}
			ITypeReference explicitInterfaceTypeReference = null;
			if (base.IsExplicitInterfaceImplementation && base.ExplicitInterfaceImplementations.Count == 1)
			{
				explicitInterfaceTypeReference = base.ExplicitInterfaceImplementations[0].DeclaringTypeReference;
			}
			return AbstractUnresolvedMember.Resolve(AbstractUnresolvedMember.ExtendContextForType(context, base.DeclaringTypeDefinition), base.SymbolKind, base.Name, explicitInterfaceTypeReference, (from tp in this.TypeParameters
			select tp.Name).ToList<string>(), (from p in this.Parameters
			select p.Type).ToList<ITypeReference>());
		}

		IMethod IUnresolvedMethod.Resolve(ITypeResolveContext context)
		{
			return (IMethod)this.Resolve(context);
		}

		public static DefaultUnresolvedMethod CreateDefaultConstructor(IUnresolvedTypeDefinition typeDefinition)
		{
			if (typeDefinition == null)
			{
				throw new ArgumentNullException("typeDefinition");
			}
			DomRegion region = typeDefinition.Region;
			region = new DomRegion(region.FileName, region.BeginLine, region.BeginColumn);
			return new DefaultUnresolvedMethod(typeDefinition, ".ctor")
			{
				SymbolKind = SymbolKind.Constructor,
				Accessibility = (typeDefinition.IsAbstract ? Accessibility.Protected : Accessibility.Public),
				IsSynthetic = true,
				HasBody = true,
				Region = region,
				BodyRegion = region,
				ReturnType = KnownTypeReference.Void
			};
		}

		/// <summary>
		/// Returns a dummy constructor instance:
		/// </summary>
		/// <returns>
		/// A public instance constructor with IsSynthetic=true and no declaring type.
		/// </returns>
		public static IUnresolvedMethod DummyConstructor
		{
			get
			{
				return DefaultUnresolvedMethod.dummyConstructor;
			}
		}

		private static IUnresolvedMethod CreateDummyConstructor()
		{
			DefaultUnresolvedMethod defaultUnresolvedMethod = new DefaultUnresolvedMethod
			{
				SymbolKind = SymbolKind.Constructor,
				Name = ".ctor",
				Accessibility = Accessibility.Public,
				IsSynthetic = true,
				ReturnType = KnownTypeReference.Void
			};
			defaultUnresolvedMethod.Freeze();
			return defaultUnresolvedMethod;
		}

		private IList<IUnresolvedAttribute> returnTypeAttributes;

		private IList<IUnresolvedTypeParameter> typeParameters;

		private IList<IUnresolvedParameter> parameters;

		private IUnresolvedMember accessorOwner;

		private static readonly IUnresolvedMethod dummyConstructor = DefaultUnresolvedMethod.CreateDummyConstructor();
	}
}
