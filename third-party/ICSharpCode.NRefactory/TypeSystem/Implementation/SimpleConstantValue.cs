using System;
using ICSharpCode.NRefactory.Semantics;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// A simple constant value that is independent of the resolve context.
	/// </summary>
	[Serializable]
	public sealed class SimpleConstantValue : IConstantValue, ISupportsInterning
	{
		public SimpleConstantValue(ITypeReference type, object value)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.type = type;
			this.value = value;
		}

		public ResolveResult Resolve(ITypeResolveContext context)
		{
			return new ConstantResolveResult(this.type.Resolve(context), this.value);
		}

		public override string ToString()
		{
			if (this.value == null)
			{
				return "null";
			}
			if (this.value is bool)
			{
				return this.value.ToString().ToLowerInvariant();
			}
			return this.value.ToString();
		}

		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.type.GetHashCode() ^ ((this.value != null) ? this.value.GetHashCode() : 0);
		}

		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			SimpleConstantValue simpleConstantValue = other as SimpleConstantValue;
			return simpleConstantValue != null && this.type == simpleConstantValue.type && this.value == simpleConstantValue.value;
		}

		private readonly ITypeReference type;

		private readonly object value;
	}
}
