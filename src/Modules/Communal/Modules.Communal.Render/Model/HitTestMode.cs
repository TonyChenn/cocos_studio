using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.ViewModel.HitTest;

namespace Modules.Communal.Render.Model
{
	public class HitTestMode : ICoordinateMapping
	{
		protected static bool IsContinueTest(BaseTestResult testResult)
		{
			return testResult == null || testResult.IsContinueTest;
		}

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

		public virtual HitTestResult GetHitVisual(VisualObject rootObject, PointF point)
		{
			PointF point2 = this.ConvertCoordinate(point);
			return this.GetAllHitVisual(rootObject, point2).FirstOrDefault<HitTestResult>();
		}

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

		public HitTestResult GetHitVisualFirstSelected(VisualObject rootObject, PointF point, IEnumerable<VisualObject> currentSelectedObject)
		{
			return this.GetHitVisual(rootObject, point);
		}

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

		public virtual List<VisualObject> GetVisualInRect(VisualObject rootObject, RectF rect)
		{
			PointF pointF = new PointF(rect.X, rect.Y + rect.Height);
			pointF = this.ConvertCoordinate(pointF);
			RectF rect2 = new RectF(pointF.X, pointF.Y, rect.Width, rect.Height);
			HashSet<VisualObject> hashSet = new HashSet<VisualObject>();
			HitTestMode.GetVisualInRect(rootObject, rect2, hashSet);
			return hashSet.ToList<VisualObject>();
		}

		public virtual PointF ConvertCoordinate(PointF screenPoint)
		{
			return GameWindow.Current.ConvertControlToScene(screenPoint);
		}

		protected virtual IComparer<VisualObject> GetObjectComparer()
		{
			return null;
		}

		protected virtual IComparer<HitTestResult> GetResultComparer()
		{
			return null;
		}
	}
}
