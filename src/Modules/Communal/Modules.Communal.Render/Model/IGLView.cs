using System;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Gdk;

namespace Modules.Communal.Render.Model
{
	public interface IGLView
	{
		PointF ConvertControlToScene(PointF controlPoint);

		PointF ConvertScreenToScene(PointF screenPoint);

		float ActualWidth { get; }

		float ActualHeight { get; }

		Cursor Cursor { set; }

		GameWindow GameWindow { get; }
	}
}
