using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000027 RID: 39
	internal static class GuidesHitTestHelp
	{
		// Token: 0x0600014D RID: 333 RVA: 0x000085D0 File Offset: 0x000067D0
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

		// Token: 0x0600014E RID: 334 RVA: 0x00008614 File Offset: 0x00006814
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

		// Token: 0x0600014F RID: 335 RVA: 0x000086B8 File Offset: 0x000068B8
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

		// Token: 0x04000056 RID: 86
		private const float valueBound = 3f;

		// Token: 0x04000057 RID: 87
		private const float dockBound = 10f;

		// Token: 0x04000058 RID: 88
		private static GuidesHitTestHelp.HitTestCompare hitTestCompare = new GuidesHitTestHelp.HitTestCompare();

		// Token: 0x04000059 RID: 89
		private static GuidesHitTestHelp.GuidesObjectEmpty guidesObjectEmpty = new GuidesHitTestHelp.GuidesObjectEmpty();

		// Token: 0x02000028 RID: 40
		private sealed class HitTestCompare : IComparer<GuidesObject>
		{
			// Token: 0x06000150 RID: 336 RVA: 0x000087D4 File Offset: 0x000069D4
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

		// Token: 0x02000029 RID: 41
		private sealed class GuidesObjectEmpty : GuidesObject
		{
			// Token: 0x17000033 RID: 51
			// (get) Token: 0x06000152 RID: 338 RVA: 0x00008840 File Offset: 0x00006A40
			// (set) Token: 0x06000153 RID: 339 RVA: 0x00008857 File Offset: 0x00006A57
			public override float Position { get; set; }
		}
	}
}
