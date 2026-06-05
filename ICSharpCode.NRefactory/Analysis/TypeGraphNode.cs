using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Analysis
{
	// Token: 0x02000006 RID: 6
	public sealed class TypeGraphNode
	{
		/// <summary>
		/// Creates a new unconnected type graph node.
		/// </summary>
		// Token: 0x06000016 RID: 22 RVA: 0x00002A30 File Offset: 0x00001A30
		public TypeGraphNode(ITypeDefinition typeDef)
		{
			this.typeDef = typeDef;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002A55 File Offset: 0x00001A55
		public ITypeDefinition TypeDefinition
		{
			get
			{
				return this.typeDef;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002A5D File Offset: 0x00001A5D
		public IList<TypeGraphNode> DerivedTypes
		{
			get
			{
				return this.derivedTypes;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002A65 File Offset: 0x00001A65
		public IList<TypeGraphNode> BaseTypes
		{
			get
			{
				return this.baseTypes;
			}
		}

		// Token: 0x0400000B RID: 11
		private readonly ITypeDefinition typeDef;

		// Token: 0x0400000C RID: 12
		private readonly List<TypeGraphNode> baseTypes = new List<TypeGraphNode>();

		// Token: 0x0400000D RID: 13
		private readonly List<TypeGraphNode> derivedTypes = new List<TypeGraphNode>();
	}
}
