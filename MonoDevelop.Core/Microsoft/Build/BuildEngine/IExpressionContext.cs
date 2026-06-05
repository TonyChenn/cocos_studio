using System;

namespace Microsoft.Build.BuildEngine
{
	// Token: 0x020001AB RID: 427
	internal interface IExpressionContext
	{
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06001009 RID: 4105
		string FullFileName { get; }

		// Token: 0x0600100A RID: 4106
		string EvaluateString(string value);
	}
}
