using System;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Gdk;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000024 RID: 36
	public interface IGLView
	{
		// Token: 0x06000131 RID: 305
		PointF ConvertControlToScene(PointF controlPoint);

		// Token: 0x06000132 RID: 306
		PointF ConvertScreenToScene(PointF screenPoint);

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000133 RID: 307
		float ActualWidth { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000134 RID: 308
		float ActualHeight { get; }

		// Token: 0x1700002B RID: 43
		// (set) Token: 0x06000135 RID: 309
		Cursor Cursor { set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000136 RID: 310
		GameWindow GameWindow { get; }
	}
}
