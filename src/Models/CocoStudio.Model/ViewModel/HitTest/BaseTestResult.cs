using System;

namespace CocoStudio.Model.ViewModel.HitTest
{
	// Token: 0x0200011D RID: 285
	public abstract class BaseTestResult : IComparable
	{
		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x0002B4B4 File Offset: 0x000296B4
		// (set) Token: 0x06000AE4 RID: 2788 RVA: 0x0002B4CB File Offset: 0x000296CB
		public VisualObject HitVisual { get; private set; }

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x0002B4D4 File Offset: 0x000296D4
		// (set) Token: 0x06000AE6 RID: 2790 RVA: 0x0002B4EB File Offset: 0x000296EB
		public bool IsContinueTest { get; private set; }

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0002B4F4 File Offset: 0x000296F4
		public BaseTestResult(VisualObject hitVisual, bool isContinueTest)
		{
			this.HitVisual = hitVisual;
			this.IsContinueTest = isContinueTest;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0002B510 File Offset: 0x00029710
		public int CompareTo(object obj)
		{
			int result;
			if (obj == null || !(obj is BaseTestResult))
			{
				result = 1;
			}
			else
			{
				BaseTestResult baseTestResult = obj as BaseTestResult;
				if (baseTestResult.HitVisual == null)
				{
					result = 1;
				}
				else if (this.HitVisual == null)
				{
					result = -1;
				}
				else
				{
					result = this.HitVisual.CompareTo(baseTestResult.HitVisual);
				}
			}
			return result;
		}
	}
}
