using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Type Reference used when the fully qualified type name is known.
	/// </summary>
	// Token: 0x020000D2 RID: 210
	[Serializable]
	public sealed class GetClassTypeReference : ITypeReference, ISymbolReference, ISupportsInterning
	{
		/// <summary>
		/// Creates a new GetClassTypeReference that searches a type definition.
		/// </summary>
		/// <param name="fullTypeName">The full name of the type.</param>
		/// <param name="assembly">A reference to the assembly containing this type.
		/// If this parameter is null, the GetClassTypeReference will search in all
		/// assemblies belonging to the compilation.
		/// </param>
		// Token: 0x060007B4 RID: 1972 RVA: 0x000136B7 File Offset: 0x000126B7
		public GetClassTypeReference(FullTypeName fullTypeName, IAssemblyReference assembly = null)
		{
			this.fullTypeName = fullTypeName;
			this.assembly = assembly;
		}

		/// <summary>
		/// Creates a new GetClassTypeReference that searches a top-level type in all assemblies.
		/// </summary>
		/// <param name="namespaceName">The namespace name containing the type, e.g. "System.Collections.Generic".</param>
		/// <param name="name">The name of the type, e.g. "List".</param>
		/// <param name="typeParameterCount">The number of type parameters, (e.g. 1 for List&lt;T&gt;).</param>
		// Token: 0x060007B5 RID: 1973 RVA: 0x000136CD File Offset: 0x000126CD
		public GetClassTypeReference(string namespaceName, string name, int typeParameterCount = 0)
		{
			this.fullTypeName = new TopLevelTypeName(namespaceName, name, typeParameterCount);
		}

		/// <summary>
		/// Creates a new GetClassTypeReference that searches a top-level type in the specified assembly.
		/// </summary>
		/// <param name="assembly">A reference to the assembly containing this type.
		/// If this parameter is null, the GetClassTypeReference will search in all assemblies belonging to the ICompilation.</param>
		/// <param name="namespaceName">The namespace name containing the type, e.g. "System.Collections.Generic".</param>
		/// <param name="name">The name of the type, e.g. "List".</param>
		/// <param name="typeParameterCount">The number of type parameters, (e.g. 1 for List&lt;T&gt;).</param>
		// Token: 0x060007B6 RID: 1974 RVA: 0x000136E8 File Offset: 0x000126E8
		public GetClassTypeReference(IAssemblyReference assembly, string namespaceName, string name, int typeParameterCount = 0)
		{
			this.assembly = assembly;
			this.fullTypeName = new TopLevelTypeName(namespaceName, name, typeParameterCount);
		}

		/// <summary>
		/// Gets the assembly reference.
		/// This property returns null if the GetClassTypeReference is searching in all assemblies
		/// of the compilation.
		/// </summary>
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x0001370B File Offset: 0x0001270B
		public IAssemblyReference Assembly
		{
			get
			{
				return this.assembly;
			}
		}

		/// <summary>
		/// Gets the full name of the type this reference is searching for.
		/// </summary>
		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x00013713 File Offset: 0x00012713
		public FullTypeName FullTypeName
		{
			get
			{
				return this.fullTypeName;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x0001371C File Offset: 0x0001271C
		[Obsolete("Use the FullTypeName property instead. GetClassTypeReference now supports nested types, where the Namespace/Name/TPC tripel isn't sufficient for identifying the type.")]
		public string Namespace
		{
			get
			{
				return this.fullTypeName.TopLevelTypeName.Namespace;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x00013740 File Offset: 0x00012740
		[Obsolete("Use the FullTypeName property instead. GetClassTypeReference now supports nested types, where the Namespace/Name/TPC tripel isn't sufficient for identifying the type.")]
		public string Name
		{
			get
			{
				return this.fullTypeName.Name;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x0001375C File Offset: 0x0001275C
		[Obsolete("Use the FullTypeName property instead. GetClassTypeReference now supports nested types, where the Namespace/Name/TPC tripel isn't sufficient for identifying the type.")]
		public int TypeParameterCount
		{
			get
			{
				return this.fullTypeName.TypeParameterCount;
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00013778 File Offset: 0x00012778
		private IType ResolveInAllAssemblies(ITypeResolveContext context)
		{
			ICompilation compilation = context.Compilation;
			foreach (IAssembly assembly in compilation.Assemblies)
			{
				IType typeDefinition = assembly.GetTypeDefinition(this.fullTypeName);
				if (typeDefinition != null)
				{
					return typeDefinition;
				}
			}
			return null;
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x000137E4 File Offset: 0x000127E4
		public IType Resolve(ITypeResolveContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			IType type = null;
			if (this.assembly == null)
			{
				if (context.CurrentAssembly != null)
				{
					type = context.CurrentAssembly.GetTypeDefinition(this.fullTypeName);
				}
				if (type == null)
				{
					type = this.ResolveInAllAssemblies(context);
				}
			}
			else
			{
				IAssembly assembly = this.assembly.Resolve(context);
				if (assembly != null)
				{
					type = assembly.GetTypeDefinition(this.fullTypeName);
				}
				else
				{
					type = this.ResolveInAllAssemblies(context);
				}
			}
			return type ?? new UnknownType(this.fullTypeName);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00013868 File Offset: 0x00012868
		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			IType type = this.Resolve(context);
			if (type is ITypeDefinition)
			{
				return (ISymbol)type;
			}
			return null;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00013890 File Offset: 0x00012890
		public override string ToString()
		{
			return this.fullTypeName.ToString() + ((this.assembly != null) ? (", " + this.assembly.ToString()) : null);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x000138D8 File Offset: 0x000128D8
		int ISupportsInterning.GetHashCodeForInterning()
		{
			return 33 * this.assembly.GetHashCode() + this.fullTypeName.GetHashCode();
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00013908 File Offset: 0x00012908
		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			GetClassTypeReference getClassTypeReference = other as GetClassTypeReference;
			return getClassTypeReference != null && this.assembly == getClassTypeReference.assembly && this.fullTypeName == getClassTypeReference.fullTypeName;
		}

		// Token: 0x04000239 RID: 569
		private readonly IAssemblyReference assembly;

		// Token: 0x0400023A RID: 570
		private readonly FullTypeName fullTypeName;
	}
}
