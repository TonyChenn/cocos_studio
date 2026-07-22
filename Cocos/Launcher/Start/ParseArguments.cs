using System;

namespace Cocos.Launcher.Start
{
	// Token: 0x02000002 RID: 2
	public class ParseArguments
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
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

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002069 File Offset: 0x00000269
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

		// Token: 0x06000005 RID: 5 RVA: 0x00002074 File Offset: 0x00000274
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

		// Token: 0x04000001 RID: 1
		private int defaultPageIndex;

		// Token: 0x04000002 RID: 2
		private bool isAutoStart;
	}
}
