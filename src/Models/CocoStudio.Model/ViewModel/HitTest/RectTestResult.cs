using System;

namespace CocoStudio.Model.ViewModel.HitTest
{
	public class RectTestResult : BaseTestResult
	{
		public RectF HitRect { get; private set; }

		public RectTestResult(RectF hitRect, bool isContinueTest) : this(null, hitRect, isContinueTest)
		{
		}

		public RectTestResult(VisualObject hitVisual, RectF hitRect) : this(hitVisual, hitRect, hitVisual != null && hitVisual.Visible)
		{
		}

		public RectTestResult(VisualObject hitVisual, RectF hitRect, bool isContinueTest) : base(hitVisual, isContinueTest)
		{
		}
	}
}
