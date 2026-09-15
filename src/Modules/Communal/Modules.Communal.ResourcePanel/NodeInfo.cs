using System;

namespace Modules.Communal.ResourcePanel
{
	public class NodeInfo
	{
		public NodeInfo()
		{
			this.Reset();
		}

		public void Reset()
		{
			this.Name = string.Empty;
			this.IconInfo = new IconInfo();
			this.StatusMessage = string.Empty;
		}

		public string Name { get; set; }

		public IconInfo IconInfo { get; set; }

		public string StatusMessage { get; set; }

		public object DataItem { get; set; }

		public bool IsError;

		public NodeBuilder[] BuilderChain;
	}
}
