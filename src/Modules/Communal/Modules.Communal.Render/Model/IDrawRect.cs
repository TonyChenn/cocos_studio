using System;
using CocoStudio.Model;

namespace Modules.Communal.Render.Model
{
	public interface IDrawRect
	{
		bool Visible { get; set; }

		void Clear();

		void DrawRectangle(PointF leftTop, PointF rightButtom);
	}
}
