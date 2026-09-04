using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Analysis
{
	/// <summary>
	/// A graph where type definitions are nodes; and edges are given by inheritance.
	/// </summary>
	// Token: 0x02000005 RID: 5
	public class TypeGraph
	{
		/// <summary>
		/// Builds a graph of all type definitions in the specified set of assemblies.
		/// </summary>
		/// <param name="assemblies">The input assemblies. The assemblies may belong to multiple compilations.</param>
		/// <remarks>The resulting graph may be cyclic if there are cyclic type definitions.</remarks>
		// Token: 0x06000013 RID: 19 RVA: 0x0000283C File Offset: 0x0000183C
		public TypeGraph(IEnumerable<IAssembly> assemblies)
		{
			if (assemblies == null)
			{
				throw new ArgumentNullException("assemblies");
			}
			this.dict = new Dictionary<AssemblyQualifiedTypeName, TypeGraphNode>();
			foreach (IAssembly assembly in assemblies)
			{
				foreach (ITypeDefinition typeDefinition in assembly.GetAllTypeDefinitions())
				{
					this.dict[new AssemblyQualifiedTypeName(typeDefinition)] = new TypeGraphNode(typeDefinition);
				}
			}
			foreach (IAssembly assembly2 in assemblies)
			{
				foreach (ITypeDefinition typeDefinition2 in assembly2.GetAllTypeDefinitions())
				{
					TypeGraphNode typeGraphNode = this.dict[new AssemblyQualifiedTypeName(typeDefinition2)];
					foreach (IType type in typeDefinition2.DirectBaseTypes)
					{
						ITypeDefinition definition = type.GetDefinition();
						TypeGraphNode typeGraphNode2;
						if (definition != null && this.dict.TryGetValue(new AssemblyQualifiedTypeName(definition), out typeGraphNode2))
						{
							typeGraphNode.BaseTypes.Add(typeGraphNode2);
							typeGraphNode2.DerivedTypes.Add(typeGraphNode);
						}
					}
				}
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000029FC File Offset: 0x000019FC
		public TypeGraphNode GetNode(ITypeDefinition typeDefinition)
		{
			if (typeDefinition == null)
			{
				return null;
			}
			return this.GetNode(new AssemblyQualifiedTypeName(typeDefinition));
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002A10 File Offset: 0x00001A10
		public TypeGraphNode GetNode(AssemblyQualifiedTypeName typeName)
		{
			TypeGraphNode result;
			if (this.dict.TryGetValue(typeName, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0400000A RID: 10
		private Dictionary<AssemblyQualifiedTypeName, TypeGraphNode> dict;
	}
}
