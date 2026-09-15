using System;
using System.Drawing;

namespace Nuclex.Game.Packing
{
	public abstract class RectanglePacker
	{
		protected RectanglePacker(int packingAreaWidth, int packingAreaHeight)
		{
			this.packingAreaWidth = packingAreaWidth;
			this.packingAreaHeight = packingAreaHeight;
		}

		public virtual Point Pack(int rectangleWidth, int rectangleHeight)
		{
			Point result;
			if (!this.TryPack(rectangleWidth, rectangleHeight, out result))
			{
				throw new OutOfSpaceException("Rectangle does not fit in packing area");
			}
			return result;
		}

		public abstract bool TryPack(int rectangleWidth, int rectangleHeight, out Point placement);

		protected int PackingAreaWidth
		{
			get
			{
				return this.packingAreaWidth;
			}
		}

		protected int PackingAreaHeight
		{
			get
			{
				return this.packingAreaHeight;
			}
		}

		private int packingAreaWidth;

		private int packingAreaHeight;
	}
}
