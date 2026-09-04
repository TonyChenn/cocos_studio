using System;
using System.Threading;

namespace Cocos.Update
{
	// Token: 0x02000005 RID: 5
	internal class MainClass
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002250 File Offset: 0x00000450
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
