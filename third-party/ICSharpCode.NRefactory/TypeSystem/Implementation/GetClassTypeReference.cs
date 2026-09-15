using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Type Reference used when the fully qualified type name is known.
	/// </summary>
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
		public FullTypeName FullTypeName
		{
			get
			{
				return this.fullTypeName;
			}
		}

		[Obsolete("Use the FullTypeName property instead. GetClassTypeReference now supports nested types, where the Namespace/Name/TPC tripel isn't sufficient for identifying the type.")]
		public string Namespace
		{
			get
			{
				return this.fullTypeName.TopLevelTypeName.Namespace;
			}
		}

		[Obsolete("Use the FullTypeName property instead. GetClassTypeReference now supports nested types, where the Namespace/Name/TPC tripel isn't sufficient for identifying the type.")]
		public string Name
		{
			get
			{
				return this.fullTypeName.Name;
			}
		}

		[Obsolete("Use the FullTypeName property instead. GetClassTypeReference now supports nested types, where the Namespace/Name/TPC tripel isn't sufficient for identifying the type.")]
		public int TypeParameterCount
		{
			get
			{
				return this.fullTypeName.TypeParameterCount;
			}
		}

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
			if (type != null)
			{
				return type;
			}
			return new UnknownType(this.fullTypeName);
		}

		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			IType type = this.Resolve(context);
			if (type is ITypeDefinition)
			{
				return (ISymbol)type;
			}
			return null;
		}

		public override string ToString()
		{
			return this.fullTypeName.ToString() + ((this.assembly != null) ? (", " + this.assembly.ToString()) : null);
		}

		int ISupportsInterning.GetHashCodeForInterning()
		{
			return 33 * this.assembly.GetHashCode() + this.fullTypeName.GetHashCode();
		}

		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			GetClassTypeReference getClassTypeReference = other as GetClassTypeReference;
			return getClassTypeReference != null && this.assembly == getClassTypeReference.assembly && this.fullTypeName == getClassTypeReference.fullTypeName;
		}

		private readonly IAssemblyReference assembly;

		private readonly FullTypeName fullTypeName;
	}
}
