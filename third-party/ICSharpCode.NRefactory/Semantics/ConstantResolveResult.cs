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
	// Token: 0x0200003B RID: 59
	public class ConstantResolveResult : ResolveResult
	{
		// Token: 0x060001BA RID: 442 RVA: 0x00005E62 File Offset: 0x00004E62
		public ConstantResolveResult(IType type, object constantValue) : base(type)
		{
			this.constantValue = constantValue;
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00005E72 File Offset: 0x00004E72
		public override bool IsCompileTimeConstant
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00005E75 File Offset: 0x00004E75
		public override object ConstantValue
		{
			get
			{
				return this.constantValue;
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00005E80 File Offset: 0x00004E80
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0} {1} = {2}]", new object[]
			{
				base.GetType().Name,
				base.Type,
				this.constantValue
			});
		}

		// Token: 0x04000067 RID: 103
		private object constantValue;
	}
}
