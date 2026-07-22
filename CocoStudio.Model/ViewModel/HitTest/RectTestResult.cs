using System;

namespace CocoStudio.Model.ViewModel.HitTest
{
	// Token: 0x02000121 RID: 289
	public class RectTestResult : BaseTestResult
	{
		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x0002B698 File Offset: 0x00029898
		// (set) Token: 0x06000AF8 RID: 2808 RVA: 0x0002B6AF File Offset: 0x000298AF
		public RectF HitRect { get; private set; }

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0002B6B8 File Offset: 0x000298B8
		public RectTestResult(RectF hitRect, bool isContinueTest) : this(null, hitRect, isContinueTest)
		{
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0002B6C6 File Offset: 0x000298C6
		public RectTestResult(VisualObject hitVisual, RectF hitRect) : this(hitVisual, hitRect, hitVisual != null && hitVisual.Visible)
		{
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0002B6E0 File Offset: 0x000298E0
		public RectTestResult(VisualObject hitVisual, RectF hitRect, bool isContinueTest) : base(hitVisual, isContinueTest)
		{
		}
	}
}
