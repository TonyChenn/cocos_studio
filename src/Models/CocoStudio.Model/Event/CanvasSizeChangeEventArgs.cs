using System;

namespace CocoStudio.Model.Event
{
	public class CanvasSizeChangeEventArgs
	{
		public string CanvasName { get; private set; }

		public SizeF NewSize { get; private set; }

		public CanvasSizeChangeEventArgs(string canvasName, SizeF newSize)
		{
			this.CanvasName = canvasName;
			this.NewSize = newSize;
		}
	}
}
