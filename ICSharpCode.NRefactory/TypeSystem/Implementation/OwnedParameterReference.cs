using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x020000AE RID: 174
	internal sealed class OwnedParameterReference : ISymbolReference
	{
		// Token: 0x060005B9 RID: 1465 RVA: 0x0000DB25 File Offset: 0x0000CB25
		public OwnedParameterReference(IMemberReference member, int index)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			this.memberReference = member;
			this.index = index;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0000DB4C File Offset: 0x0000CB4C
		public ISymbol Resolve(ITypeResolveContext context)
		{
			IParameterizedMember parameterizedMember = this.memberReference.Resolve(context) as IParameterizedMember;
			if (parameterizedMember != null && this.index >= 0 && this.index < parameterizedMember.Parameters.Count)
			{
				return parameterizedMember.Parameters[this.index];
			}
			return null;
		}

		// Token: 0x0400019B RID: 411
		private readonly IMemberReference memberReference;

		// Token: 0x0400019C RID: 412
		private readonly int index;
	}
}
