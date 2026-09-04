using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// The resolved expression refers to a type name.
	/// </summary>
	// Token: 0x02000034 RID: 52
	public class TypeResolveResult : ResolveResult
	{
		// Token: 0x0600019A RID: 410 RVA: 0x00005A70 File Offset: 0x00004A70
		public TypeResolveResult(IType type) : base(type)
		{
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00005A79 File Offset: 0x00004A79
		public override bool IsError
		{
			get
			{
				return base.Type.Kind == TypeKind.Unknown;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00005A8C File Offset: 0x00004A8C
		public override DomRegion GetDefinitionRegion()
		{
			ITypeDefinition definition = base.Type.GetDefinition();
			if (definition != null)
			{
				return definition.Region;
			}
			return DomRegion.Empty;
		}
	}
}
