using System;
using System.Diagnostics;

namespace Cocos.Update
{
	public static class ProcHelper
	{
		public static void KillProc(string strProcName)
		{
			try
			{
				foreach (Process process in Process.GetProcessesByName(strProcName))
				{
					if (!process.CloseMainWindow())
					{
						process.Kill();
					}
				}
				foreach (Process process2 in Process.GetProcessesByName(strProcName + ".exe"))
				{
					if (!process2.CloseMainWindow())
					{
						process2.Kill();
					}
				}
			}
			catch (Exception ex)
			{
				DebugHelper.WriteLogInfo(ex.ToString());
			}
		}

		public const string UpdaterName = "Editor.AutoUpdate";

		public const string UpdaterVshostName = "Editor.AutoUpdate.vshost";

		public const string EditorName = "CocosStudio";
	}
}
