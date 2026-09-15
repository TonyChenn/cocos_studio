using System;

namespace CocoStudio.Model
{
	[Flags]
	public enum OperationMask
	{
		NoneFlag = 0,
		MoveFlag = 1,
		ScaleFlag = 2,
		RotationFlag = 4,
		AnchorMoveFlag = 8,
		SizeFlag = 16,
		VisibleFlag = 32,
		AlignFlag = 64,
		LayoutFlag = 128,
		AllFlag = 65535
	}
}
