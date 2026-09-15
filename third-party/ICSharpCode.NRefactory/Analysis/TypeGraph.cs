using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Analysis
{
	/// <summary>
	/// A graph where type definitions are nodes; and edges are given by inheritance.
	/// </summary>
	public class TypeGraph
	{
		/// <summary>
		/// Builds a graph of all type definitions in the specified set of assemblies.
		/// </summary>
		/// <param name="assemblies">The input assemblies. The assemblies may belong to multiple compilations.</param>
		/// <remarks>The resulting graph may be cyclic if there are cyclic type definitions.</remarks>
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

		public TypeGraphNode GetNode(ITypeDefinition typeDefinition)
		{
			if (typeDefinition == null)
			{
				return null;
			}
			return this.GetNode(new AssemblyQualifiedTypeName(typeDefinition));
		}

		public TypeGraphNode GetNode(AssemblyQualifiedTypeName typeName)
		{
			TypeGraphNode result;
			if (this.dict.TryGetValue(typeName, out result))
			{
				return result;
			}
			return null;
		}

		private Dictionary<AssemblyQualifiedTypeName, TypeGraphNode> dict;
	}
}
