using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// The resolved expression refers to a type name.
	/// </summary>
	public class TypeResolveResult : ResolveResult
	{
		public TypeResolveResult(IType type) : base(type)
		{
		}

		public override bool IsError
		{
			get
			{
				return base.Type.Kind == TypeKind.Unknown;
			}
		}

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
