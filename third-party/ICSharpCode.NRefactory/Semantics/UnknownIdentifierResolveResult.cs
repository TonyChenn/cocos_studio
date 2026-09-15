using System;
using System.Globalization;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents an unknown identifier.
	/// </summary>
	public class UnknownIdentifierResolveResult : ResolveResult
	{
		public UnknownIdentifierResolveResult(string identifier, int typeArgumentCount = 0) : base(SpecialType.UnknownType)
		{
			this.identifier = identifier;
			this.typeArgumentCount = typeArgumentCount;
		}

		public string Identifier
		{
			get
			{
				return this.identifier;
			}
		}

		public int TypeArgumentCount
		{
			get
			{
				return this.typeArgumentCount;
			}
		}

		public override bool IsError
		{
			get
			{
				return true;
			}
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0} {1}]", new object[]
			{
				base.GetType().Name,
				this.identifier
			});
		}

		private readonly string identifier;

		private readonly int typeArgumentCount;
	}
}
