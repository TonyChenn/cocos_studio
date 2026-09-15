using System;
using System.Threading;

namespace Cocos.Update
{
	internal class MainClass
	{
		public static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				return;
			}
			try
			{
				string text = args[0];
				string text2 = (args.Length > 1) ? args[1] : "#SKIP#";
				DebugHelper.WriteLogInfo("studioInstallArgs:" + text);
				DebugHelper.WriteLogInfo("macRuntimeArgs:" + text2);
				Updater updater = new Updater(text, text2);
				updater.StartUpdate();
				Thread.Sleep(-1);
			}
			catch (Exception ex)
			{
				DebugHelper.WriteLogInfo(ex.ToString());
			}
		}
	}
}
