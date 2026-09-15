using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IVariable" />.
	/// </summary>
	public sealed class DefaultVariable : IVariable, ISymbol
	{
		public DefaultVariable(IType type, string name)
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
		}

		public DefaultVariable(IType type, string name, DomRegion region = default(DomRegion), bool isConst = false, object constantValue = null) : this(type, name)
		{
			this.region = region;
			this.isConst = isConst;
			this.constantValue = constantValue;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public DomRegion Region
		{
			get
			{
				return this.region;
			}
		}

		public IType Type
		{
			get
			{
				return this.type;
			}
		}

		public bool IsConst
		{
			get
			{
				return this.isConst;
			}
		}

		public object ConstantValue
		{
			get
			{
				return this.constantValue;
			}
		}

		public SymbolKind SymbolKind
		{
			get
			{
				return SymbolKind.Variable;
			}
		}

		public ISymbolReference ToReference()
		{
			return new VariableReference(this.type.ToTypeReference(), this.name, this.region, this.isConst, this.constantValue);
		}

		private readonly string name;

		private readonly DomRegion region;

		private readonly IType type;

		private readonly object constantValue;

		private readonly bool isConst;
	}
}
