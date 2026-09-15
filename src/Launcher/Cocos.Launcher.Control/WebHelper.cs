using System;
using System.Diagnostics;
using CocoStudio.Basic;

namespace Cocos.Launcher.Control
{
	public static class WebHelper
	{
		public static void OnOpenWeb(object sender, LinkClickedEventArgs e)
		{
			WebHelper.OpenWeb(e.Tag.ToString());
		}

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
