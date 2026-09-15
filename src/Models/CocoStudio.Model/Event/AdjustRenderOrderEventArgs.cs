using System;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Event
{
	public class AdjustRenderOrderEventArgs
	{
		public NodeObject Node { get; private set; }

		public MoveOrderType Action { get; private set; }

		public AdjustRenderOrderEventArgs(NodeObject node, MoveOrderType action)
		{
			this.Node = node;
			this.Action = action;
		}
	}
}
