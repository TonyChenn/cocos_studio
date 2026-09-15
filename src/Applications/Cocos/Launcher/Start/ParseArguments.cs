using System;

namespace Cocos.Launcher.Start
{
	public class ParseArguments
	{
		public int DefaultPageIndex
		{
			get
			{
				return this.defaultPageIndex;
			}
			private set
			{
				this.defaultPageIndex = value;
			}
		}

		public bool IsAutoStart
		{
			get
			{
				return this.isAutoStart;
			}
			private set
			{
				this.isAutoStart = value;
			}
		}

		public ParseArguments(string[] args)
		{
			if (args == null || args.Length == 0)
			{
				return;
			}
			foreach (string text in args)
			{
				if (text.StartsWith("-page"))
				{
					this.DefaultPageIndex = 2;
				}
				else if (text.StartsWith("-AutoStart"))
				{
					this.IsAutoStart = true;
				}
			}
		}

		private int defaultPageIndex;

		private bool isAutoStart;
	}
}
