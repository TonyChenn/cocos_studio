using System;

namespace CocoStudio.Model.ViewModel.HitTest
{
	public class HitTestResult : BaseTestResult
	{
		public MouseOperationType OperationType { get; set; }

		public ControlPointType ControlPointType { get; set; }

		public PointF HitPoint { get; private set; }

		public float Rotate { get; private set; }

		public float Distance { get; set; }

		public HitTestResult(PointF hitPoint, bool isContinueTest) : this(null, hitPoint, MouseOperationType.OPERATION_NONE, ControlPointType.POINT_NONE, isContinueTest)
		{
		}

		public HitTestResult(VisualObject hitVisual, PointF hitPoint, MouseOperationType operationType, ControlPointType controlPointType) : this(hitVisual, hitPoint, operationType, controlPointType, hitVisual != null && hitVisual.Visible)
		{
		}

		public HitTestResult(VisualObject hitVisual, PointF hitPoint, MouseOperationType operationType, ControlPointType controlPointType, float rotate) : this(hitVisual, hitPoint, operationType, controlPointType, hitVisual != null && hitVisual.Visible)
		{
			this.Rotate = rotate;
		}

		public HitTestResult(VisualObject hitVisual, PointF hitPoint, MouseOperationType operationType, ControlPointType controlPointType, bool isContinueTest) : base(hitVisual, isContinueTest)
		{
			this.HitPoint = hitPoint;
			this.OperationType = operationType;
			this.ControlPointType = controlPointType;
		}
	}
}
