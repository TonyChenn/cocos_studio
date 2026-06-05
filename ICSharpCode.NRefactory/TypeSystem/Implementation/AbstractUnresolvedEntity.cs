using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Base class for <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedEntity" /> implementations.
	/// </summary>
	// Token: 0x020000A3 RID: 163
	[Serializable]
	public abstract class AbstractUnresolvedEntity : IUnresolvedEntity, INamedElement, IHasAccessibility, IFreezable
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0000C7A6 File Offset: 0x0000B7A6
		public bool IsFrozen
		{
			get
			{
				return this.flags[1];
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0000C7B4 File Offset: 0x0000B7B4
		public void Freeze()
		{
			if (!this.flags[1])
			{
				this.FreezeInternal();
				this.flags[1] = true;
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0000C7D7 File Offset: 0x0000B7D7
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
		// Token: 0x0600053D RID: 1341 RVA: 0x0000C800 File Offset: 0x0000B800
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
		// Token: 0x0600053E RID: 1342 RVA: 0x0000C85C File Offset: 0x0000B85C
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

		// Token: 0x0600053F RID: 1343 RVA: 0x0000C8BA File Offset: 0x0000B8BA
		protected void ThrowIfFrozen()
		{
			FreezableHelper.ThrowIfFrozen(this);
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x0000C8C2 File Offset: 0x0000B8C2
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x0000C8CA File Offset: 0x0000B8CA
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

		// Token: 0x06000542 RID: 1346 RVA: 0x0000C8D9 File Offset: 0x0000B8D9
		internal virtual AbstractUnresolvedEntity.RareFields WriteRareFields()
		{
			this.ThrowIfFrozen();
			if (this.rareFields == null)
			{
				this.rareFields = new AbstractUnresolvedEntity.RareFields();
			}
			return this.rareFields;
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x0000C8FA File Offset: 0x0000B8FA
		// (set) Token: 0x06000544 RID: 1348 RVA: 0x0000C915 File Offset: 0x0000B915
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

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x0000C938 File Offset: 0x0000B938
		// (set) Token: 0x06000546 RID: 1350 RVA: 0x0000C953 File Offset: 0x0000B953
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

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x0000C976 File Offset: 0x0000B976
		// (set) Token: 0x06000548 RID: 1352 RVA: 0x0000C98D File Offset: 0x0000B98D
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

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x0000C9A6 File Offset: 0x0000B9A6
		// (set) Token: 0x0600054A RID: 1354 RVA: 0x0000C9AE File Offset: 0x0000B9AE
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

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x0000C9BD File Offset: 0x0000B9BD
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

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x0000C9D8 File Offset: 0x0000B9D8
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x0000C9E0 File Offset: 0x0000B9E0
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

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x0000CA00 File Offset: 0x0000BA00
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

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x0000CA5B File Offset: 0x0000BA5B
		// (set) Token: 0x06000550 RID: 1360 RVA: 0x0000CA76 File Offset: 0x0000BA76
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

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x0000CA7D File Offset: 0x0000BA7D
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

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x0000CAA9 File Offset: 0x0000BAA9
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x0000CAB1 File Offset: 0x0000BAB1
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

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x0000CAC0 File Offset: 0x0000BAC0
		// (set) Token: 0x06000555 RID: 1365 RVA: 0x0000CACF File Offset: 0x0000BACF
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

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x0000CAE5 File Offset: 0x0000BAE5
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x0000CAF3 File Offset: 0x0000BAF3
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

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x0000CB08 File Offset: 0x0000BB08
		// (set) Token: 0x06000559 RID: 1369 RVA: 0x0000CB16 File Offset: 0x0000BB16
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

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x0000CB2B File Offset: 0x0000BB2B
		// (set) Token: 0x0600055B RID: 1371 RVA: 0x0000CB39 File Offset: 0x0000BB39
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

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x0000CB4E File Offset: 0x0000BB4E
		// (set) Token: 0x0600055D RID: 1373 RVA: 0x0000CB5D File Offset: 0x0000BB5D
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

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0000CB73 File Offset: 0x0000BB73
		bool IHasAccessibility.IsPrivate
		{
			get
			{
				return this.accessibility == Accessibility.Private;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x0000CB7E File Offset: 0x0000BB7E
		bool IHasAccessibility.IsPublic
		{
			get
			{
				return this.accessibility == Accessibility.Public;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x0000CB89 File Offset: 0x0000BB89
		bool IHasAccessibility.IsProtected
		{
			get
			{
				return this.accessibility == Accessibility.Protected;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x0000CB94 File Offset: 0x0000BB94
		bool IHasAccessibility.IsInternal
		{
			get
			{
				return this.accessibility == Accessibility.Internal;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x0000CB9F File Offset: 0x0000BB9F
		bool IHasAccessibility.IsProtectedOrInternal
		{
			get
			{
				return this.accessibility == Accessibility.ProtectedOrInternal;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0000CBAA File Offset: 0x0000BBAA
		bool IHasAccessibility.IsProtectedAndInternal
		{
			get
			{
				return this.accessibility == Accessibility.ProtectedAndInternal;
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0000CBB8 File Offset: 0x0000BBB8
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

		// Token: 0x0400015E RID: 350
		internal const ushort FlagFrozen = 1;

		// Token: 0x0400015F RID: 351
		internal const ushort FlagSealed = 2;

		// Token: 0x04000160 RID: 352
		internal const ushort FlagAbstract = 4;

		// Token: 0x04000161 RID: 353
		internal const ushort FlagShadowing = 8;

		// Token: 0x04000162 RID: 354
		internal const ushort FlagSynthetic = 16;

		// Token: 0x04000163 RID: 355
		internal const ushort FlagStatic = 32;

		// Token: 0x04000164 RID: 356
		internal const ushort FlagAddDefaultConstructorIfRequired = 64;

		// Token: 0x04000165 RID: 357
		internal const ushort FlagHasExtensionMethods = 128;

		// Token: 0x04000166 RID: 358
		internal const ushort FlagHasNoExtensionMethods = 256;

		// Token: 0x04000167 RID: 359
		internal const ushort FlagPartialTypeDefinition = 512;

		// Token: 0x04000168 RID: 360
		internal const ushort FlagExplicitInterfaceImplementation = 64;

		// Token: 0x04000169 RID: 361
		internal const ushort FlagVirtual = 128;

		// Token: 0x0400016A RID: 362
		internal const ushort FlagOverride = 256;

		// Token: 0x0400016B RID: 363
		internal const ushort FlagFieldIsReadOnly = 4096;

		// Token: 0x0400016C RID: 364
		internal const ushort FlagFieldIsVolatile = 8192;

		// Token: 0x0400016D RID: 365
		internal const ushort FlagFieldIsFixedSize = 16384;

		// Token: 0x0400016E RID: 366
		internal const ushort FlagExtensionMethod = 4096;

		// Token: 0x0400016F RID: 367
		internal const ushort FlagPartialMethod = 8192;

		// Token: 0x04000170 RID: 368
		internal const ushort FlagHasBody = 16384;

		// Token: 0x04000171 RID: 369
		internal const ushort FlagAsyncMethod = 32768;

		// Token: 0x04000172 RID: 370
		private IUnresolvedTypeDefinition declaringTypeDefinition;

		// Token: 0x04000173 RID: 371
		private string name = string.Empty;

		// Token: 0x04000174 RID: 372
		private IList<IUnresolvedAttribute> attributes;

		// Token: 0x04000175 RID: 373
		internal AbstractUnresolvedEntity.RareFields rareFields;

		// Token: 0x04000176 RID: 374
		private SymbolKind symbolKind;

		// Token: 0x04000177 RID: 375
		private Accessibility accessibility;

		// Token: 0x04000178 RID: 376
		internal BitVector16 flags;

		// Token: 0x020000A4 RID: 164
		[Serializable]
		internal class RareFields
		{
			// Token: 0x06000566 RID: 1382 RVA: 0x0000CC3D File Offset: 0x0000BC3D
			protected internal virtual void FreezeInternal()
			{
			}

			// Token: 0x06000567 RID: 1383 RVA: 0x0000CC3F File Offset: 0x0000BC3F
			public virtual void ApplyInterningProvider(InterningProvider provider)
			{
			}

			// Token: 0x06000568 RID: 1384 RVA: 0x0000CC41 File Offset: 0x0000BC41
			public virtual object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x04000179 RID: 377
			internal DomRegion region;

			// Token: 0x0400017A RID: 378
			internal DomRegion bodyRegion;

			// Token: 0x0400017B RID: 379
			internal IUnresolvedFile unresolvedFile;
		}
	}
}
