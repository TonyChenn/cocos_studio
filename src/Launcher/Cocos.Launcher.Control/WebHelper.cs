using System;
using System.Diagnostics;
using CocoStudio.Basic;

namespace Cocos.Launcher.Control
{
	// Token: 0x02000009 RID: 9
	public static class WebHelper
	{
		// Token: 0x06000049 RID: 73 RVA: 0x000027B8 File Offset: 0x000009B8
		public static void OnOpenWeb(object sender, LinkClickedEventArgs e)
		{
			WebHelper.OpenWeb(e.Tag.ToString());
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000027CC File Offset: 0x000009CC
		public static void OpenWeb(string url)
		{
			try
			{
				using (Process.Start(url))
				{
				}
			}
			catch (Exception exception)
			{
				LogConfig.Output.Error("Open web address failed.", exception);
			}
		}
	}
}
