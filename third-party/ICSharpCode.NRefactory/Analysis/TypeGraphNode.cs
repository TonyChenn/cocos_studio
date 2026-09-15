using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Analysis
{
	public sealed class TypeGraphNode
	{
		/// <summary>
		/// Creates a new unconnected type graph node.
		/// </summary>
		public TypeGraphNode(ITypeDefinition typeDef)
		{
			this.typeDef = typeDef;
		}

		public ITypeDefinition TypeDefinition
		{
			get
			{
				return this.typeDef;
			}
		}

		public IList<TypeGraphNode> DerivedTypes
		{
			get
			{
				return this.derivedTypes;
			}
		}

		public IList<TypeGraphNode> BaseTypes
		{
			get
			{
				return this.baseTypes;
			}
		}

		private readonly ITypeDefinition typeDef;

		private readonly List<TypeGraphNode> baseTypes = new List<TypeGraphNode>();

		private readonly List<TypeGraphNode> derivedTypes = new List<TypeGraphNode>();
	}
}
