using System;

namespace CocoStudio.Model
{
	// Token: 0x02000010 RID: 16
	[Flags]
	public enum OperationMask
	{
		// Token: 0x04000038 RID: 56
		NoneFlag = 0,
		// Token: 0x04000039 RID: 57
		MoveFlag = 1,
		// Token: 0x0400003A RID: 58
		ScaleFlag = 2,
		// Token: 0x0400003B RID: 59
		RotationFlag = 4,
		// Token: 0x0400003C RID: 60
		AnchorMoveFlag = 8,
		// Token: 0x0400003D RID: 61
		SizeFlag = 16,
		// Token: 0x0400003E RID: 62
		VisibleFlag = 32,
		// Token: 0x0400003F RID: 63
		AlignFlag = 64,
		// Token: 0x04000040 RID: 64
		LayoutFlag = 128,
		// Token: 0x04000041 RID: 65
		AllFlag = 65535
	}
}
