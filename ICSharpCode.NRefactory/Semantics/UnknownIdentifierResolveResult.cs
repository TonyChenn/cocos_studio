using System;
using System.Globalization;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents an unknown identifier.
	/// </summary>
	// Token: 0x02000051 RID: 81
	public class UnknownIdentifierResolveResult : ResolveResult
	{
		// Token: 0x0600024A RID: 586 RVA: 0x00006B0D File Offset: 0x00005B0D
		public UnknownIdentifierResolveResult(string identifier, int typeArgumentCount = 0) : base(SpecialType.UnknownType)
		{
			this.identifier = identifier;
			this.typeArgumentCount = typeArgumentCount;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600024B RID: 587 RVA: 0x00006B28 File Offset: 0x00005B28
		public string Identifier
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00006B30 File Offset: 0x00005B30
		public int TypeArgumentCount
		{
			get
			{
				return this.typeArgumentCount;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00006B38 File Offset: 0x00005B38
		public override bool IsError
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00006B3C File Offset: 0x00005B3C
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0} {1}]", new object[]
			{
				base.GetType().Name,
				this.identifier
			});
		}

		// Token: 0x040000AD RID: 173
		private readonly string identifier;

		// Token: 0x040000AE RID: 174
		private readonly int typeArgumentCount;
	}
}
