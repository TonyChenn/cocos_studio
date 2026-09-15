using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.ViewModel.HitTest;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Skeleton
{
	internal class HitTestModeSkeleton : HitTestMode
	{
		public HitTestModeSkeleton()
		{
			this.boneComparer = new HitTestModeSkeleton.BoneComparer();
		}

		protected override IComparer<VisualObject> GetObjectComparer()
		{
			return this.boneComparer;
		}

		protected override IComparer<HitTestResult> GetResultComparer()
		{
			return this.boneComparer;
		}

		private HitTestModeSkeleton.BoneComparer boneComparer;

		private class BoneComparer : IComparer<VisualObject>, IComparer<HitTestResult>
		{
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

			public int Compare(HitTestResult x, HitTestResult y)
			{
				return this.Compare(x.HitVisual, y.HitVisual) * -1;
			}
		}
	}
}
