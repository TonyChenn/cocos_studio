using System;
using ICSharpCode.NRefactory.Semantics;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// A simple constant value that is independent of the resolve context.
	/// </summary>
	// Token: 0x020000D9 RID: 217
	[Serializable]
	public sealed class SimpleConstantValue : IConstantValue, ISupportsInterning
	{
		// Token: 0x0600080B RID: 2059 RVA: 0x00015599 File Offset: 0x00014599
		public SimpleConstantValue(ITypeReference type, object value)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.type = type;
			this.value = value;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x000155BD File Offset: 0x000145BD
		public ResolveResult Resolve(ITypeResolveContext context)
		{
			return new ConstantResolveResult(this.type.Resolve(context), this.value);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x000155D6 File Offset: 0x000145D6
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

		// Token: 0x0600080E RID: 2062 RVA: 0x0001560F File Offset: 0x0001460F
		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.type.GetHashCode() ^ ((this.value != null) ? this.value.GetHashCode() : 0);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00015634 File Offset: 0x00014634
		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			SimpleConstantValue simpleConstantValue = other as SimpleConstantValue;
			return simpleConstantValue != null && this.type == simpleConstantValue.type && this.value == simpleConstantValue.value;
		}

		// Token: 0x04000254 RID: 596
		private readonly ITypeReference type;

		// Token: 0x04000255 RID: 597
		private readonly object value;
	}
}
