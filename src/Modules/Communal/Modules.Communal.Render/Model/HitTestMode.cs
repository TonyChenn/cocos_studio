using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.ViewModel.HitTest;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000004 RID: 4
	public class HitTestMode : ICoordinateMapping
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00004D78 File Offset: 0x00002F78
		protected static bool IsContinueTest(BaseTestResult testResult)
		{
			return testResult == null || testResult.IsContinueTest;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00004DA0 File Offset: 0x00002FA0
		protected static int FindLastUnderParentChild(List<VisualObject> children)
		{
			int result;
			if (children.Count == 0)
			{
				result = 0;
			}
			else
			{
				for (int i = 0; i < children.Count; i++)
				{
					if (children[i].ZOrder >= 0)
					{
						return i;
					}
				}
				result = children.Count - 1;
			}
			return result;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00004DFC File Offset: 0x00002FFC
		protected virtual void InnerGetHitVisual(VisualObject parentObject, PointF point, List<HitTestResult> list)
		{
			if (parentObject != null)
			{
				HitTestResult hitTestResult = parentObject.HitTest(point);
				IEnumerable<VisualObject> visualChildren = parentObject.GetVisualChildren();
				if (visualChildren != null && HitTestMode.IsContinueTest(hitTestResult))
				{
					List<VisualObject> list2 = visualChildren.ToList<VisualObject>();
					IComparer<VisualObject> objectComparer = this.GetObjectComparer();
					if (objectComparer != null)
					{
						list2.Sort(objectComparer);
					}
					int num = HitTestMode.FindLastUnderParentChild(list2);
					for (int i = list2.Count - 1; i >= num; i--)
					{
						this.InnerGetHitVisual(list2[i], point, list);
					}
					if (hitTestResult != null && hitTestResult.HitVisual != null)
					{
						list.Add(hitTestResult);
					}
					for (int i = num - 1; i >= 0; i--)
					{
						this.InnerGetHitVisual(list2[i], point, list);
					}
				}
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00004EF8 File Offset: 0x000030F8
		public virtual HitTestResult GetHitVisual(VisualObject rootObject, PointF point)
		{
			PointF point2 = this.ConvertCoordinate(point);
			return this.GetAllHitVisual(rootObject, point2).FirstOrDefault<HitTestResult>();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00004F24 File Offset: 0x00003124
		protected List<HitTestResult> GetAllHitVisual(VisualObject rootObject, PointF point)
		{
			List<HitTestResult> list = new List<HitTestResult>();
			this.InnerGetHitVisual(rootObject, point, list);
			IComparer<HitTestResult> resultComparer = this.GetResultComparer();
			if (resultComparer != null)
			{
				list.Sort(resultComparer);
			}
			return list;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00004F60 File Offset: 0x00003160
		public static HitTestResult GetHoverVisualBetweenSelected(PointF point, IEnumerable<VisualObject> selectedObjectList)
		{
			foreach (VisualObject visualObject in selectedObjectList)
			{
				if (!(visualObject is CanvasObject) && visualObject != null)
				{
					HitTestResult hitTestResult = visualObject.HitTest(point);
					if (hitTestResult != null && hitTestResult.HitVisual != null)
					{
						return hitTestResult;
					}
				}
			}
			return null;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00004FF8 File Offset: 0x000031F8
		public HitTestResult GetHitVisualFirstSelected(VisualObject rootObject, PointF point, IEnumerable<VisualObject> currentSelectedObject)
		{
			return this.GetHitVisual(rootObject, point);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00005014 File Offset: 0x00003214
		private static void InnerFilterCoveredChildren(VisualObject parentObject, List<VisualObject> parentObjectList)
		{
			if (parentObject.IsSelected)
			{
				parentObjectList.Add(parentObject);
			}
			else
			{
				IEnumerable<VisualObject> visualChildren = parentObject.GetVisualChildren();
				if (visualChildren != null && visualChildren.Count<VisualObject>() > 0)
				{
					foreach (VisualObject parentObject2 in visualChildren)
					{
						HitTestMode.InnerFilterCoveredChildren(parentObject2, parentObjectList);
					}
				}
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000050A8 File Offset: 0x000032A8
		public void FilterCoveredChildren(VisualObject rootObject, List<VisualObject> parentObjectList, List<VisualObject> selectedObjectList)
		{
			if (rootObject != null && parentObjectList != null)
			{
				parentObjectList.Clear();
				IEnumerable<VisualObject> visualChildren = rootObject.GetVisualChildren();
				if (visualChildren != null)
				{
					if (rootObject is CanvasObject)
					{
						if (rootObject.IsSelected && selectedObjectList.Count == 1)
						{
							parentObjectList.Add(rootObject);
						}
						else
						{
							foreach (VisualObject parentObject in visualChildren)
							{
								HitTestMode.InnerFilterCoveredChildren(parentObject, parentObjectList);
							}
						}
					}
					else
					{
						HitTestMode.InnerFilterCoveredChildren(rootObject, parentObjectList);
					}
				}
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00005178 File Offset: 0x00003378
		public void SelectAllObjects(VisualObject rootObject, List<VisualObject> selectedObjectList, List<VisualObject> selectedParentObjectList)
		{
			if (rootObject == null)
			{
				throw new ArgumentNullException("rootObject");
			}
			selectedObjectList.Clear();
			selectedParentObjectList.Clear();
			IEnumerable<VisualObject> visualChildren = rootObject.GetVisualChildren();
			if (visualChildren != null)
			{
				foreach (VisualObject visualObject in visualChildren)
				{
					if (visualObject.Visible)
					{
						if (visualObject.CanEdit)
						{
							selectedParentObjectList.Add(visualObject);
							selectedObjectList.Add(visualObject);
						}
						HitTestMode.InnerSelectAllObject(visualObject, selectedObjectList, selectedParentObjectList, visualObject.CanEdit);
					}
				}
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000523C File Offset: 0x0000343C
		private static void InnerSelectAllObject(VisualObject parentObject, List<VisualObject> selectedObjectList, List<VisualObject> selectedParentObjectList, bool ancestorCanEdit)
		{
			IEnumerable<VisualObject> visualChildren = parentObject.GetVisualChildren();
			if (!parentObject.IsSelected && parentObject.CanEdit)
			{
				parentObject.IsSelected = true;
			}
			if (visualChildren != null && parentObject.Visible)
			{
				foreach (VisualObject visualObject in visualChildren)
				{
					if (visualObject.CanEdit)
					{
						selectedObjectList.Add(visualObject);
					}
					if (!ancestorCanEdit)
					{
						selectedParentObjectList.Add(visualObject);
						ancestorCanEdit = true;
					}
					HitTestMode.InnerSelectAllObject(visualObject, selectedObjectList, selectedParentObjectList, ancestorCanEdit);
				}
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000052FC File Offset: 0x000034FC
		protected static void GetVisualInRect(VisualObject parentObject, RectF rect, HashSet<VisualObject> list)
		{
			if (parentObject != null)
			{
				RectTestResult rectTestResult = parentObject.RectTest(rect);
				if (rectTestResult != null && rectTestResult.HitVisual != null)
				{
					if (!list.Contains(parentObject))
					{
						list.Add(parentObject);
					}
				}
				IEnumerable<VisualObject> visualChildren = parentObject.GetVisualChildren();
				if (visualChildren != null && HitTestMode.IsContinueTest(rectTestResult))
				{
					foreach (VisualObject parentObject2 in visualChildren)
					{
						HitTestMode.GetVisualInRect(parentObject2, rect, list);
					}
				}
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000053B8 File Offset: 0x000035B8
		public virtual List<VisualObject> GetVisualInRect(VisualObject rootObject, RectF rect)
		{
			PointF pointF = new PointF(rect.X, rect.Y + rect.Height);
			pointF = this.ConvertCoordinate(pointF);
			RectF rect2 = new RectF(pointF.X, pointF.Y, rect.Width, rect.Height);
			HashSet<VisualObject> hashSet = new HashSet<VisualObject>();
			HitTestMode.GetVisualInRect(rootObject, rect2, hashSet);
			return hashSet.ToList<VisualObject>();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00005420 File Offset: 0x00003620
		public virtual PointF ConvertCoordinate(PointF screenPoint)
		{
			return GameWindow.Current.ConvertControlToScene(screenPoint);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00005440 File Offset: 0x00003640
		protected virtual IComparer<VisualObject> GetObjectComparer()
		{
			return null;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00005454 File Offset: 0x00003654
		protected virtual IComparer<HitTestResult> GetResultComparer()
		{
			return null;
		}
	}
}
