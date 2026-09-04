using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x0200001F RID: 31
	internal class ObjectRectangelHelper
	{
		// Token: 0x060000EA RID: 234 RVA: 0x00005D9C File Offset: 0x00003F9C
		public static PointF GetObjectListCenter(IReadOnlyList<VisualObject> ObjectList)
		{
			ObjectRectangelHelper.CreateObjectRectangleFromVisualObject(ObjectList);
			PointF pointF = Services.ProjectOperations.CurrentSelectedProject.GetRootNode().TransformToScene(new PointF(0f, 0f));
			return new PointF((ObjectRectangelHelper.LeftX + ObjectRectangelHelper.RightX) * 0.5f + pointF.X, (ObjectRectangelHelper.TopY + ObjectRectangelHelper.BottomY) * 0.5f + pointF.Y);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005E10 File Offset: 0x00004010
		public static bool CheckObjectAlign(AlignType alignType, IReadOnlyList<VisualObject> ObjectList)
		{
			return ObjectRectangelHelper.RefrshObjectAlign(alignType, ObjectList, true);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005E2C File Offset: 0x0000402C
		public static bool AlignObject(AlignType alignType, IReadOnlyList<VisualObject> ObjectList)
		{
			return ObjectRectangelHelper.RefrshObjectAlign(alignType, ObjectList, false);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005E48 File Offset: 0x00004048
		private static bool RefrshObjectAlign(AlignType alignType, IReadOnlyList<VisualObject> ObjectList, bool isCheck)
		{
			bool result;
			if (ObjectList == null || ObjectList.Count <= 0)
			{
				result = false;
			}
			else
			{
				bool flag = false;
				PointF pointF = Services.ProjectOperations.CurrentSelectedProject.GetRootNode().TransformToScene(new PointF(0f, 0f));
				List<ObjectRectangle> objectRectList = ObjectRectangelHelper.CreateObjectRectangleFromVisualObject(ObjectList);
				switch (alignType)
				{
				case AlignType.Align_Center:
				case AlignType.Align_Left:
				case AlignType.Align_Right:
				case AlignType.Align_Top:
				case AlignType.Align_Bottom:
					flag = ObjectRectangelHelper.AlignEage(alignType, objectRectList, isCheck);
					break;
				case AlignType.Align_HorizontalCenter:
				case AlignType.Align_VerticalCenter:
					flag = ObjectRectangelHelper.AlignHorizontalOrVerical(alignType, objectRectList, isCheck);
					break;
				case AlignType.Horizontal_Equidistance:
				case AlignType.Vertical_Equidistance:
					flag = ObjectRectangelHelper.AlignEquidStance(alignType, objectRectList, isCheck);
					break;
				}
				result = flag;
			}
			return result;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005EFC File Offset: 0x000040FC
		private static bool AlignEquidStance(AlignType alignType, List<ObjectRectangle> ObjectRectList, bool isCheck)
		{
			bool result = false;
			if (alignType == AlignType.Horizontal_Equidistance)
			{
				IList<ObjectRectangle> list = (from b in ObjectRectList
				orderby b.AbsoluteLeft
				select b).ToList<ObjectRectangle>();
				for (int i = 1; i < list.Count; i++)
				{
					ObjectRectangle objectRectangle = list[i - 1];
					objectRectangle.RefreshProperty();
					ObjectRectangle objectRectangle2 = list[i];
					float num = objectRectangle.AbsoluteRight + ObjectRectangelHelper.IntervalObjectWidth - objectRectangle2.AbsoluteLeft;
					if (num != 0f)
					{
						result = true;
						ObjectRectangelHelper.MoveObject(objectRectangle2, num, 0f, isCheck);
					}
				}
			}
			else if (alignType == AlignType.Vertical_Equidistance)
			{
				IList<ObjectRectangle> list = (from b in ObjectRectList
				orderby b.AbsoluteBottom
				select b).ToList<ObjectRectangle>();
				for (int i = 1; i < list.Count; i++)
				{
					ObjectRectangle objectRectangle = list[i - 1];
					objectRectangle.RefreshProperty();
					ObjectRectangle objectRectangle2 = list[i];
					float num2 = objectRectangle.AbsoluteTop + ObjectRectangelHelper.IntervalObjectHeight - objectRectangle2.AbsoluteBottom;
					if (num2 != 0f)
					{
						result = true;
						ObjectRectangelHelper.MoveObject(objectRectangle2, 0f, num2, isCheck);
					}
				}
			}
			return result;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006074 File Offset: 0x00004274
		private static bool AlignEage(AlignType alignType, List<ObjectRectangle> ObjectRectList, bool isCheck)
		{
			bool result = false;
			PointF pointF = new PointF((ObjectRectangelHelper.LeftX + ObjectRectangelHelper.RightX) * 0.5f, (ObjectRectangelHelper.TopY + ObjectRectangelHelper.BottomY) * 0.5f);
			if (alignType == AlignType.Align_Center)
			{
				foreach (ObjectRectangle objectRectangle in ObjectRectList)
				{
					if (objectRectangle.AbsoluteCenterX != pointF.X || objectRectangle.AbsoluteCenterY != pointF.Y)
					{
						result = true;
						float offsetX = pointF.X - objectRectangle.AbsoluteCenterX;
						float offsetY = pointF.Y - objectRectangle.AbsoluteCenterY;
						ObjectRectangelHelper.MoveObject(objectRectangle, offsetX, offsetY, isCheck);
					}
				}
			}
			else if (alignType == AlignType.Align_Left)
			{
				foreach (ObjectRectangle objectRectangle in ObjectRectList)
				{
					if (objectRectangle.AbsoluteLeft != ObjectRectangelHelper.LeftX)
					{
						result = true;
						float offsetX = ObjectRectangelHelper.LeftX - objectRectangle.AbsoluteLeft;
						ObjectRectangelHelper.MoveObject(objectRectangle, offsetX, 0f, isCheck);
					}
				}
			}
			else if (alignType == AlignType.Align_Right)
			{
				foreach (ObjectRectangle objectRectangle in ObjectRectList)
				{
					if (objectRectangle.AbsoluteRight != ObjectRectangelHelper.RightX)
					{
						result = true;
						float offsetX = ObjectRectangelHelper.RightX - objectRectangle.AbsoluteRight;
						ObjectRectangelHelper.MoveObject(objectRectangle, offsetX, 0f, isCheck);
					}
				}
			}
			else if (alignType == AlignType.Align_Top)
			{
				foreach (ObjectRectangle objectRectangle in ObjectRectList)
				{
					if (objectRectangle.AbsoluteTop != ObjectRectangelHelper.TopY)
					{
						result = true;
						float offsetY = ObjectRectangelHelper.TopY - objectRectangle.AbsoluteTop;
						ObjectRectangelHelper.MoveObject(objectRectangle, 0f, offsetY, isCheck);
					}
				}
			}
			else if (alignType == AlignType.Align_Bottom)
			{
				foreach (ObjectRectangle objectRectangle in ObjectRectList)
				{
					if (objectRectangle.AbsoluteBottom != ObjectRectangelHelper.BottomY)
					{
						result = true;
						float offsetY = ObjectRectangelHelper.BottomY - objectRectangle.AbsoluteBottom;
						ObjectRectangelHelper.MoveObject(objectRectangle, 0f, offsetY, isCheck);
					}
				}
			}
			return result;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000639C File Offset: 0x0000459C
		private static bool AlignHorizontalOrVerical(AlignType alignType, List<ObjectRectangle> ObjectRectList, bool isCheck)
		{
			bool result = false;
			PointF pointF = Services.ProjectOperations.CurrentSelectedProject.GetRootNode().TransformToScene(new PointF(0f, 0f));
			PointF pointF2 = new PointF((ObjectRectangelHelper.LeftX + ObjectRectangelHelper.RightX) * 0.5f, (ObjectRectangelHelper.TopY + ObjectRectangelHelper.BottomY) * 0.5f);
			if (alignType == AlignType.Align_VerticalCenter)
			{
				foreach (ObjectRectangle objectRectangle in ObjectRectList)
				{
					if (objectRectangle.AbsoluteCenterY != pointF2.Y)
					{
						result = true;
						float offsetY = pointF2.Y - objectRectangle.AbsoluteCenterY;
						ObjectRectangelHelper.MoveObject(objectRectangle, 0f, offsetY, isCheck);
					}
				}
			}
			else if (alignType == AlignType.Align_HorizontalCenter)
			{
				foreach (ObjectRectangle objectRectangle in ObjectRectList)
				{
					if (objectRectangle.AbsoluteCenterX != pointF2.X)
					{
						result = true;
						float offsetX = pointF2.X - objectRectangle.AbsoluteCenterX;
						ObjectRectangelHelper.MoveObject(objectRectangle, offsetX, 0f, isCheck);
					}
				}
			}
			return result;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006520 File Offset: 0x00004720
		private static void MoveObject(ObjectRectangle vObjectRect, float offsetX, float offsetY, bool isCheck)
		{
			if (!isCheck)
			{
				VisualObject vObject = vObjectRect.vObject;
				PointF pointF = vObject.TransformToScene(new PointF(vObject.AnchorPoint.ScaleX * vObject.Size.Width, vObject.AnchorPoint.ScaleY * vObject.Size.Height));
				pointF.X += offsetX;
				pointF.Y += offsetY;
				PointF position = vObject.TransformToParent(pointF);
				vObject.Position = position;
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000065A8 File Offset: 0x000047A8
		private static List<ObjectRectangle> CreateObjectRectangleFromVisualObject(IReadOnlyList<VisualObject> ObjectList)
		{
			ObjectRectangelHelper.LeftX = 10000f;
			ObjectRectangelHelper.RightX = -10000f;
			ObjectRectangelHelper.TopY = -10000f;
			ObjectRectangelHelper.BottomY = 10000f;
			ObjectRectangelHelper.TotalObjectWidth = 0f;
			ObjectRectangelHelper.TotalObjectHeight = 0f;
			ObjectRectangelHelper.IntervalObjectWidth = 0f;
			ObjectRectangelHelper.IntervalObjectHeight = 0f;
			List<ObjectRectangle> list = new List<ObjectRectangle>();
			foreach (VisualObject vObj in ObjectList)
			{
				ObjectRectangle objectRectangle = new ObjectRectangle(vObj);
				ObjectRectangelHelper.LeftX = Math.Min(ObjectRectangelHelper.LeftX, objectRectangle.AbsoluteLeft);
				ObjectRectangelHelper.RightX = Math.Max(ObjectRectangelHelper.RightX, objectRectangle.AbsoluteRight);
				ObjectRectangelHelper.TopY = Math.Max(ObjectRectangelHelper.TopY, objectRectangle.AbsoluteTop);
				ObjectRectangelHelper.BottomY = Math.Min(ObjectRectangelHelper.BottomY, objectRectangle.AbsoluteBottom);
				ObjectRectangelHelper.TotalObjectWidth += objectRectangle.AbsoluteRight - objectRectangle.AbsoluteLeft;
				ObjectRectangelHelper.TotalObjectHeight += objectRectangle.AbsoluteTop - objectRectangle.AbsoluteBottom;
				list.Add(objectRectangle);
			}
			if (ObjectList.Count > 2)
			{
				ObjectRectangelHelper.IntervalObjectWidth = Math.Abs((ObjectRectangelHelper.RightX - ObjectRectangelHelper.LeftX - ObjectRectangelHelper.TotalObjectWidth) / (float)(ObjectList.Count - 1));
				ObjectRectangelHelper.IntervalObjectHeight = Math.Abs((ObjectRectangelHelper.TopY - ObjectRectangelHelper.BottomY - ObjectRectangelHelper.TotalObjectHeight) / (float)(ObjectList.Count - 1));
			}
			return list;
		}

		// Token: 0x04000068 RID: 104
		private static float LeftX = 10000f;

		// Token: 0x04000069 RID: 105
		private static float RightX = -10000f;

		// Token: 0x0400006A RID: 106
		private static float TopY = -10000f;

		// Token: 0x0400006B RID: 107
		private static float BottomY = 10000f;

		// Token: 0x0400006C RID: 108
		private static float TotalObjectWidth = 0f;

		// Token: 0x0400006D RID: 109
		private static float TotalObjectHeight = 0f;

		// Token: 0x0400006E RID: 110
		private static float IntervalObjectWidth = 0f;

		// Token: 0x0400006F RID: 111
		private static float IntervalObjectHeight = 0f;
	}
}
