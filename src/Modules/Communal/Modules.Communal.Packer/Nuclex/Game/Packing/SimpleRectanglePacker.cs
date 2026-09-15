using System;
using System.Drawing;

namespace Nuclex.Game.Packing
{
	public class SimpleRectanglePacker : RectanglePacker
	{
		public SimpleRectanglePacker(int packingAreaWidth, int packingAreaHeight) : base(packingAreaWidth, packingAreaHeight)
		{
		}

		public override bool TryPack(int rectangleWidth, int rectangleHeight, out Point placement)
		{
			bool result;
			if (rectangleWidth > base.PackingAreaWidth || rectangleHeight > base.PackingAreaHeight)
			{
				placement = Point.Empty;
				result = false;
			}
			else
			{
				if (this.column + rectangleWidth > base.PackingAreaWidth)
				{
					this.currentLine += this.lineHeight;
					this.lineHeight = 0;
					this.column = 0;
				}
				if (this.currentLine + rectangleHeight > base.PackingAreaHeight)
				{
					placement = Point.Empty;
					result = false;
				}
				else
				{
					placement = new Point(this.column, this.currentLine);
					this.column += rectangleWidth;
					if (rectangleHeight > this.lineHeight)
					{
						this.lineHeight = rectangleHeight;
					}
					result = true;
				}
			}
			return result;
		}

		private int currentLine;

		private int lineHeight;

		private int column;
	}
}
