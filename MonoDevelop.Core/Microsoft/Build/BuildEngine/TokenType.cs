using System;

namespace Microsoft.Build.BuildEngine
{
	// Token: 0x020001E8 RID: 488
	internal enum TokenType
	{
		// Token: 0x04000553 RID: 1363
		EOF,
		// Token: 0x04000554 RID: 1364
		BOF,
		// Token: 0x04000555 RID: 1365
		Number,
		// Token: 0x04000556 RID: 1366
		String,
		// Token: 0x04000557 RID: 1367
		Punct,
		// Token: 0x04000558 RID: 1368
		WhiteSpace,
		// Token: 0x04000559 RID: 1369
		Item,
		// Token: 0x0400055A RID: 1370
		Property,
		// Token: 0x0400055B RID: 1371
		Metadata,
		// Token: 0x0400055C RID: 1372
		FunctionName,
		// Token: 0x0400055D RID: 1373
		Transform,
		// Token: 0x0400055E RID: 1374
		FirstPunct,
		// Token: 0x0400055F RID: 1375
		Less,
		// Token: 0x04000560 RID: 1376
		Greater,
		// Token: 0x04000561 RID: 1377
		LessOrEqual,
		// Token: 0x04000562 RID: 1378
		GreaterOrEqual,
		// Token: 0x04000563 RID: 1379
		Equal,
		// Token: 0x04000564 RID: 1380
		NotEqual,
		// Token: 0x04000565 RID: 1381
		LeftParen,
		// Token: 0x04000566 RID: 1382
		RightParen,
		// Token: 0x04000567 RID: 1383
		Dot,
		// Token: 0x04000568 RID: 1384
		Comma,
		// Token: 0x04000569 RID: 1385
		Not,
		// Token: 0x0400056A RID: 1386
		And,
		// Token: 0x0400056B RID: 1387
		Or,
		// Token: 0x0400056C RID: 1388
		Apostrophe,
		// Token: 0x0400056D RID: 1389
		LastPunct,
		// Token: 0x0400056E RID: 1390
		Invalid
	}
}
