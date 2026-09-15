using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Base class for <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedEntity" /> implementations.
	/// </summary>
	[Serializable]
	public abstract class AbstractUnresolvedEntity : IUnresolvedEntity, INamedElement, IHasAccessibility, IFreezable
	{
		public bool IsFrozen
		{
			get
			{
				return this.flags[1];
			}
		}

		public void Freeze()
		{
			if (!this.flags[1])
			{
				this.FreezeInternal();
				this.flags[1] = true;
			}
		}

		protected virtual void FreezeInternal()
		{
			this.attributes = FreezableHelper.FreezeListAndElements<IUnresolvedAttribute>(this.attributes);
			if (this.rareFields != null)
			{
				this.rareFields.FreezeInternal();
			}
		}

		/// <summary>
		/// Uses the specified interning provider to intern
		/// strings and lists in this entity.
		/// This method does not test arbitrary objects to see if they implement ISupportsInterning;
		/// instead we assume that those are interned immediately when they are created (before they are added to this entity).
		/// </summary>
		public virtual void ApplyInterningProvider(InterningProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			this.ThrowIfFrozen();
			this.name = provider.Intern(this.name);
			this.attributes = provider.InternList<IUnresolvedAttribute>(this.attributes);
			if (this.rareFields != null)
			{
				this.rareFields.ApplyInterningProvider(provider);
			}
		}

		/// <summary>
		/// Creates a shallow clone of this entity.
		/// Collections (e.g. a type's member list) will be cloned as well, but the elements
		/// of said list will not be.
		/// If this instance is frozen, the clone will be unfrozen.
		/// </summary>
		public virtual object Clone()
		{
			AbstractUnresolvedEntity abstractUnresolvedEntity = (AbstractUnresolvedEntity)base.MemberwiseClone();
			abstractUnresolvedEntity.flags[1] = false;
			if (this.attributes != null)
			{
				abstractUnresolvedEntity.attributes = new List<IUnresolvedAttribute>(this.attributes);
			}
			if (this.rareFields != null)
			{
				abstractUnresolvedEntity.rareFields = (AbstractUnresolvedEntity.RareFields)this.rareFields.Clone();
			}
			return abstractUnresolvedEntity;
		}

		protected void ThrowIfFrozen()
		{
			FreezableHelper.ThrowIfFrozen(this);
		}

		public SymbolKind SymbolKind
		{
			get
			{
				return this.symbolKind;
			}
			set
			{
				this.ThrowIfFrozen();
				this.symbolKind = value;
			}
		}

		internal virtual AbstractUnresolvedEntity.RareFields WriteRareFields()
		{
			this.ThrowIfFrozen();
			if (this.rareFields == null)
			{
				this.rareFields = new AbstractUnresolvedEntity.RareFields();
			}
			return this.rareFields;
		}

		public DomRegion Region
		{
			get
			{
				if (this.rareFields == null)
				{
					return DomRegion.Empty;
				}
				return this.rareFields.region;
			}
			set
			{
				if (value != DomRegion.Empty || this.rareFields != null)
				{
					this.WriteRareFields().region = value;
				}
			}
		}

		public DomRegion BodyRegion
		{
			get
			{
				if (this.rareFields == null)
				{
					return DomRegion.Empty;
				}
				return this.rareFields.bodyRegion;
			}
			set
			{
				if (value != DomRegion.Empty || this.rareFields != null)
				{
					this.WriteRareFields().bodyRegion = value;
				}
			}
		}

		public IUnresolvedFile UnresolvedFile
		{
			get
			{
				if (this.rareFields == null)
				{
					return null;
				}
				return this.rareFields.unresolvedFile;
			}
			set
			{
				if (value != null || this.rareFields != null)
				{
					this.WriteRareFields().unresolvedFile = value;
				}
			}
		}

		public IUnresolvedTypeDefinition DeclaringTypeDefinition
		{
			get
			{
				return this.declaringTypeDefinition;
			}
			set
			{
				this.ThrowIfFrozen();
				this.declaringTypeDefinition = value;
			}
		}

		public IList<IUnresolvedAttribute> Attributes
		{
			get
			{
				if (this.attributes == null)
				{
					this.attributes = new List<IUnresolvedAttribute>();
				}
				return this.attributes;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.ThrowIfFrozen();
				this.name = value;
			}
		}

		public virtual string FullName
		{
			get
			{
				if (this.declaringTypeDefinition != null)
				{
					return this.declaringTypeDefinition.FullName + "." + this.name;
				}
				if (!string.IsNullOrEmpty(this.Namespace))
				{
					return this.Namespace + "." + this.name;
				}
				return this.name;
			}
		}

		public virtual string Namespace
		{
			get
			{
				if (this.declaringTypeDefinition != null)
				{
					return this.declaringTypeDefinition.Namespace;
				}
				return string.Empty;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public virtual string ReflectionName
		{
			get
			{
				if (this.declaringTypeDefinition != null)
				{
					return this.declaringTypeDefinition.ReflectionName + "." + this.name;
				}
				return this.name;
			}
		}

		public Accessibility Accessibility
		{
			get
			{
				return this.accessibility;
			}
			set
			{
				this.ThrowIfFrozen();
				this.accessibility = value;
			}
		}

		public bool IsStatic
		{
			get
			{
				return this.flags[32];
			}
			set
			{
				this.ThrowIfFrozen();
				this.flags[32] = value;
			}
		}

		public bool IsAbstract
		{
			get
			{
				return this.flags[4];
			}
			set
			{
				this.ThrowIfFrozen();
				this.flags[4] = value;
			}
		}

		public bool IsSealed
		{
			get
			{
				return this.flags[2];
			}
			set
			{
				this.ThrowIfFrozen();
				this.flags[2] = value;
			}
		}

		public bool IsShadowing
		{
			get
			{
				return this.flags[8];
			}
			set
			{
				this.ThrowIfFrozen();
				this.flags[8] = value;
			}
		}

		public bool IsSynthetic
		{
			get
			{
				return this.flags[16];
			}
			set
			{
				this.ThrowIfFrozen();
				this.flags[16] = value;
			}
		}

		bool IHasAccessibility.IsPrivate
		{
			get
			{
				return this.accessibility == Accessibility.Private;
			}
		}

		bool IHasAccessibility.IsPublic
		{
			get
			{
				return this.accessibility == Accessibility.Public;
			}
		}

		bool IHasAccessibility.IsProtected
		{
			get
			{
				return this.accessibility == Accessibility.Protected;
			}
		}

		bool IHasAccessibility.IsInternal
		{
			get
			{
				return this.accessibility == Accessibility.Internal;
			}
		}

		bool IHasAccessibility.IsProtectedOrInternal
		{
			get
			{
				return this.accessibility == Accessibility.ProtectedOrInternal;
			}
		}

		bool IHasAccessibility.IsProtectedAndInternal
		{
			get
			{
				return this.accessibility == Accessibility.ProtectedAndInternal;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("[");
			stringBuilder.Append(base.GetType().Name);
			stringBuilder.Append(' ');
			if (this.DeclaringTypeDefinition != null)
			{
				stringBuilder.Append(this.DeclaringTypeDefinition.Name);
				stringBuilder.Append('.');
			}
			stringBuilder.Append(this.Name);
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		internal const ushort FlagFrozen = 1;

		internal const ushort FlagSealed = 2;

		internal const ushort FlagAbstract = 4;

		internal const ushort FlagShadowing = 8;

		internal const ushort FlagSynthetic = 16;

		internal const ushort FlagStatic = 32;

		internal const ushort FlagAddDefaultConstructorIfRequired = 64;

		internal const ushort FlagHasExtensionMethods = 128;

		internal const ushort FlagHasNoExtensionMethods = 256;

		internal const ushort FlagPartialTypeDefinition = 512;

		internal const ushort FlagExplicitInterfaceImplementation = 64;

		internal const ushort FlagVirtual = 128;

		internal const ushort FlagOverride = 256;

		internal const ushort FlagFieldIsReadOnly = 4096;

		internal const ushort FlagFieldIsVolatile = 8192;

		internal const ushort FlagFieldIsFixedSize = 16384;

		internal const ushort FlagExtensionMethod = 4096;

		internal const ushort FlagPartialMethod = 8192;

		internal const ushort FlagHasBody = 16384;

		internal const ushort FlagAsyncMethod = 32768;

		private IUnresolvedTypeDefinition declaringTypeDefinition;

		private string name = string.Empty;

		private IList<IUnresolvedAttribute> attributes;

		internal AbstractUnresolvedEntity.RareFields rareFields;

		private SymbolKind symbolKind;

		private Accessibility accessibility;

		internal BitVector16 flags;

		[Serializable]
		internal class RareFields
		{
			protected internal virtual void FreezeInternal()
			{
			}

			public virtual void ApplyInterningProvider(InterningProvider provider)
			{
			}

			public virtual object Clone()
			{
				return base.MemberwiseClone();
			}

			internal DomRegion region;

			internal DomRegion bodyRegion;

			internal IUnresolvedFile unresolvedFile;
		}
	}
}
