using System;
using CocoStudio.Model;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000005 RID: 5
	public interface IDrawRect
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000048 RID: 72
		// (set) Token: 0x06000049 RID: 73
		bool Visible { get; set; }

		// Token: 0x0600004A RID: 74
		void Clear();

		// Token: 0x0600004B RID: 75
		void DrawRectangle(PointF leftTop, PointF rightButtom);
	}
}
