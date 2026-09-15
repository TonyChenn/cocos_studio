using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	public sealed class ParameterReference : ISymbolReference
	{
		public ParameterReference(ITypeReference type, string name, DomRegion region, bool isRef, bool isOut, bool isParams, bool isOptional, object defaultValue)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.type = type;
			this.name = name;
			this.region = region;
			this.isRef = isRef;
			this.isOut = isOut;
			this.isParams = isParams;
			this.isOptional = isOptional;
			this.defaultValue = defaultValue;
		}

		public ISymbol Resolve(ITypeResolveContext context)
		{
			return new DefaultParameter(this.type.Resolve(context), this.name, null, this.region, null, this.isRef, this.isOut, this.isParams, this.isOptional, this.defaultValue);
		}

		private readonly ITypeReference type;

		private readonly string name;

		private readonly DomRegion region;

		private readonly bool isRef;

		private readonly bool isOut;

		private readonly bool isParams;

		private readonly bool isOptional;

		private readonly object defaultValue;
	}
}
