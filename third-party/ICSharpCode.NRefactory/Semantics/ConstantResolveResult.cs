using System;
using System.Globalization;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// ResolveResult representing a compile-time constant.
	/// Note: this class is mainly used for literals; there may be other ResolveResult classes
	/// which are compile-time constants as well.
	/// For example, a reference to a <c>const</c> field results in a <see cref="T:ICSharpCode.NRefactory.Semantics.MemberResolveResult" />.
	///
	/// Check <see cref="P:ICSharpCode.NRefactory.Semantics.ResolveResult.IsCompileTimeConstant" /> to determine is a resolve result is a constant.
	/// </summary>
	public class ConstantResolveResult : ResolveResult
	{
		public ConstantResolveResult(IType type, object constantValue) : base(type)
		{
			this.constantValue = constantValue;
		}

		public override bool IsCompileTimeConstant
		{
			get
			{
				return true;
			}
		}

		public override object ConstantValue
		{
			get
			{
				return this.constantValue;
			}
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0} {1} = {2}]", new object[]
			{
				base.GetType().Name,
				base.Type,
				this.constantValue
			});
		}

		private object constantValue;
	}
}
