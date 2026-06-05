using System;

namespace Microsoft.Build.BuildEngine
{
	// Token: 0x020001E7 RID: 487
	internal class Token
	{
		// Token: 0x06001267 RID: 4711 RVA: 0x0004AC32 File Offset: 0x00048E32
		public Token(string tokenValue, TokenType tokenType)
		{
			this.tokenValue = tokenValue;
			this.tokenType = tokenType;
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x0004AC48 File Offset: 0x00048E48
		public string Value
		{
			get
			{
				return this.tokenValue;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x0004AC50 File Offset: 0x00048E50
		public TokenType Type
		{
			get
			{
				return this.tokenType;
			}
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x0004AC58 File Offset: 0x00048E58
		public override string ToString()
		{
			return string.Format("Token (Type: {0} -> Value: {1})", this.tokenType, this.tokenValue);
		}

		// Token: 0x04000550 RID: 1360
		private string tokenValue;

		// Token: 0x04000551 RID: 1361
		private TokenType tokenType;
	}
}
