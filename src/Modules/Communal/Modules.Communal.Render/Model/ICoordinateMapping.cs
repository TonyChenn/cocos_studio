using System;
using CocoStudio.Model;

namespace Modules.Communal.Render.Model
{
	public interface ICoordinateMapping
	{
		PointF ConvertCoordinate(PointF point);
	}
}
