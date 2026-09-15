using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;

namespace Modules.Communal.Render.Model
{
	internal static class GuidesHitTestHelp
	{
		public static GuidesObject HitTest(GuidesList list, float position)
		{
			GuidesHitTestHelp.guidesObjectEmpty.Position = position;
			int num = list.BinarySearch(GuidesHitTestHelp.guidesObjectEmpty, GuidesHitTestHelp.hitTestCompare);
			GuidesObject result;
			if (num >= 0)
			{
				result = list[num];
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static DockGuidesResult GetDockGuides(GuidesList list, float position)
		{
			DockGuidesResult result;
			if (list.Count == 0)
			{
				result = null;
			}
			else
			{
				GuidesObject guidesObject = GuidesHitTestHelp.SearchNearestGuides(list, position);
				if (guidesObject != null)
				{
					float num = 10f / GuidesService.Instance.CanvasObject.Scale.ScaleX;
					float num2 = guidesObject.Position - position;
					float num3 = Math.Abs(num2);
					if (num3 < num)
					{
						return new DockGuidesResult
						{
							Guides = guidesObject,
							Distance = num3,
							Offset = num2
						};
					}
				}
				result = null;
			}
			return result;
		}

		private static GuidesObject SearchNearestGuides(GuidesList list, float position)
		{
			int num = 0;
			int i = list.Count - 1;
			int num2 = 0;
			GuidesObject guidesObject = null;
			while (i >= num)
			{
				num2 = (num + i) / 2;
				guidesObject = list[num2];
				if (guidesObject.Position == position)
				{
					return guidesObject;
				}
				if (guidesObject.Position < position)
				{
					num = num2 + 1;
				}
				else
				{
					i = num2 - 1;
				}
			}
			num = num2 - 1;
			if (num >= 0)
			{
				GuidesObject guidesObject2 = list[num];
				float num3 = Math.Abs(guidesObject.Position - position);
				float num4 = Math.Abs(guidesObject2.Position - position);
				if (num4 < num3)
				{
					guidesObject = guidesObject2;
				}
			}
			i = num2 + 1;
			if (i < list.Count)
			{
				GuidesObject guidesObject3 = list[i];
				float num3 = Math.Abs(guidesObject.Position - position);
				float num5 = Math.Abs(guidesObject3.Position - position);
				if (num5 < num3)
				{
					guidesObject = guidesObject3;
				}
			}
			return guidesObject;
		}

		private const float valueBound = 3f;

		private const float dockBound = 10f;

		private static GuidesHitTestHelp.HitTestCompare hitTestCompare = new GuidesHitTestHelp.HitTestCompare();

		private static GuidesHitTestHelp.GuidesObjectEmpty guidesObjectEmpty = new GuidesHitTestHelp.GuidesObjectEmpty();

		private sealed class HitTestCompare : IComparer<GuidesObject>
		{
			public int Compare(GuidesObject x, GuidesObject y)
			{
				float num = x.Position - y.Position;
				float num2 = 3f / GuidesService.Instance.CanvasObject.Scale.ScaleX;
				int result;
				if (Math.Abs(num) <= num2)
				{
					result = 0;
				}
				else if (num > 0f)
				{
					result = 1;
				}
				else
				{
					result = -1;
				}
				return result;
			}
		}

		private sealed class GuidesObjectEmpty : GuidesObject
		{
			public override float Position { get; set; }
		}
	}
}
