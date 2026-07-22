using System;

namespace CocoStudio.Model.ViewModel.HitTest
{
	// Token: 0x02000120 RID: 288
	public class HitTestResult : BaseTestResult
	{
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x0002B57C File Offset: 0x0002977C
		// (set) Token: 0x06000AEA RID: 2794 RVA: 0x0002B593 File Offset: 0x00029793
		public MouseOperationType OperationType { get; set; }

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x0002B59C File Offset: 0x0002979C
		// (set) Token: 0x06000AEC RID: 2796 RVA: 0x0002B5B3 File Offset: 0x000297B3
		public ControlPointType ControlPointType { get; set; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x0002B5BC File Offset: 0x000297BC
		// (set) Token: 0x06000AEE RID: 2798 RVA: 0x0002B5D3 File Offset: 0x000297D3
		public PointF HitPoint { get; private set; }

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x0002B5DC File Offset: 0x000297DC
		// (set) Token: 0x06000AF0 RID: 2800 RVA: 0x0002B5F3 File Offset: 0x000297F3
		public float Rotate { get; private set; }

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0002B5FC File Offset: 0x000297FC
		// (set) Token: 0x06000AF2 RID: 2802 RVA: 0x0002B613 File Offset: 0x00029813
		public float Distance { get; set; }

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0002B61C File Offset: 0x0002981C
		public HitTestResult(PointF hitPoint, bool isContinueTest) : this(null, hitPoint, MouseOperationType.OPERATION_NONE, ControlPointType.POINT_NONE, isContinueTest)
		{
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0002B62C File Offset: 0x0002982C
		public HitTestResult(VisualObject hitVisual, PointF hitPoint, MouseOperationType operationType, ControlPointType controlPointType) : this(hitVisual, hitPoint, operationType, controlPointType, hitVisual != null && hitVisual.Visible)
		{
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0002B649 File Offset: 0x00029849
		public HitTestResult(VisualObject hitVisual, PointF hitPoint, MouseOperationType operationType, ControlPointType controlPointType, float rotate) : this(hitVisual, hitPoint, operationType, controlPointType, hitVisual != null && hitVisual.Visible)
		{
			this.Rotate = rotate;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0002B66F File Offset: 0x0002986F
		public HitTestResult(VisualObject hitVisual, PointF hitPoint, MouseOperationType operationType, ControlPointType controlPointType, bool isContinueTest) : base(hitVisual, isContinueTest)
		{
			this.HitPoint = hitPoint;
			this.OperationType = operationType;
			this.ControlPointType = controlPointType;
		}
	}
}
