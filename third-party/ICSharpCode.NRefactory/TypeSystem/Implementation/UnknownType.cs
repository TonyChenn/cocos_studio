using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// An unknown type where (part) of the name is known.
	/// </summary>
	// Token: 0x020000E6 RID: 230
	[Serializable]
	public class UnknownType : AbstractType, ITypeReference
	{
		/// <summary>
		/// Creates a new unknown type.
		/// </summary>
		/// <param name="namespaceName">Namespace name, if known. Can be null if unknown.</param>
		/// <param name="name">Name of the type, must not be null.</param>
		/// <param name="typeParameterCount">Type parameter count, zero if unknown.</param>
		// Token: 0x06000892 RID: 2194 RVA: 0x00016835 File Offset: 0x00015835
		public UnknownType(string namespaceName, string name, int typeParameterCount = 0)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.namespaceKnown = (namespaceName != null);
			this.fullTypeName = new TopLevelTypeName(namespaceName ?? string.Empty, name, typeParameterCount);
		}

		/// <summary>
		/// Creates a new unknown type.
		/// </summary>
		/// <param name="fullTypeName">Full name of the unknown type.</param>
		// Token: 0x06000893 RID: 2195 RVA: 0x00016874 File Offset: 0x00015874
		public UnknownType(FullTypeName fullTypeName)
		{
			if (fullTypeName.Name == null)
			{
				this.namespaceKnown = false;
				this.fullTypeName = new TopLevelTypeName(string.Empty, "?", 0);
				return;
			}
			this.namespaceKnown = true;
			this.fullTypeName = fullTypeName;
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x000168C1 File Offset: 0x000158C1
		public override TypeKind Kind
		{
			get
			{
				return TypeKind.Unknown;
			}
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x000168C4 File Offset: 0x000158C4
		public override ITypeReference ToTypeReference()
		{
			return this;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x000168C7 File Offset: 0x000158C7
		IType ITypeReference.Resolve(ITypeResolveContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			return this;
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x000168D8 File Offset: 0x000158D8
		public override string Name
		{
			get
			{
				return this.fullTypeName.Name;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x000168F4 File Offset: 0x000158F4
		public override string Namespace
		{
			get
			{
				return this.fullTypeName.TopLevelTypeName.Namespace;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x00016918 File Offset: 0x00015918
		public override string ReflectionName
		{
			get
			{
				if (!this.namespaceKnown)
				{
					return "?";
				}
				return this.fullTypeName.ReflectionName;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x00016944 File Offset: 0x00015944
		public override int TypeParameterCount
		{
			get
			{
				return this.fullTypeName.TypeParameterCount;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x00016960 File Offset: 0x00015960
		public override bool? IsReferenceType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00016978 File Offset: 0x00015978
		public override int GetHashCode()
		{
			return (this.namespaceKnown ? 812571 : 12651) ^ this.fullTypeName.GetHashCode();
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x000169B0 File Offset: 0x000159B0
		public override bool Equals(IType other)
		{
			UnknownType unknownType = other as UnknownType;
			return unknownType != null && this.namespaceKnown == unknownType.namespaceKnown && this.fullTypeName == unknownType.fullTypeName;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x000169EC File Offset: 0x000159EC
		public override string ToString()
		{
			return "[UnknownType " + this.fullTypeName.ReflectionName + "]";
		}

		// Token: 0x04000276 RID: 630
		private readonly bool namespaceKnown;

		// Token: 0x04000277 RID: 631
		private readonly FullTypeName fullTypeName;
	}
}
