using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.ViewModel.HitTest;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Skeleton
{
	// Token: 0x02000015 RID: 21
	internal class HitTestModeSkeleton : HitTestMode
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x00005A4C File Offset: 0x00003C4C
		public HitTestModeSkeleton()
		{
			this.boneComparer = new HitTestModeSkeleton.BoneComparer();
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005A5F File Offset: 0x00003C5F
		protected override IComparer<VisualObject> GetObjectComparer()
		{
			return this.boneComparer;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005A67 File Offset: 0x00003C67
		protected override IComparer<HitTestResult> GetResultComparer()
		{
			return this.boneComparer;
		}

		// Token: 0x04000046 RID: 70
		private HitTestModeSkeleton.BoneComparer boneComparer;

		// Token: 0x02000016 RID: 22
		private class BoneComparer : IComparer<VisualObject>, IComparer<HitTestResult>
		{
			// Token: 0x060000D8 RID: 216 RVA: 0x00005A70 File Offset: 0x00003C70
			public int Compare(VisualObject x, VisualObject y)
			{
				BoneObject boneObject = x as BoneObject;
				BoneObject boneObject2 = y as BoneObject;
				if (boneObject != null && boneObject2 != null)
				{
					return boneObject.CompareTo(boneObject);
				}
				if (boneObject != null)
				{
					return 1;
				}
				if (boneObject2 != null)
				{
					return -1;
				}
				return x.CompareTo(y);
			}

			// Token: 0x060000D9 RID: 217 RVA: 0x00005AAA File Offset: 0x00003CAA
			public int Compare(HitTestResult x, HitTestResult y)
			{
				return this.Compare(x.HitVisual, y.HitVisual) * -1;
			}
		}
	}
}
