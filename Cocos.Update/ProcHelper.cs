using System;
using System.Diagnostics;

namespace Cocos.Update
{
	// Token: 0x02000004 RID: 4
	public static class ProcHelper
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000021BC File Offset: 0x000003BC
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

		// Token: 0x04000003 RID: 3
		public const string UpdaterName = "Editor.AutoUpdate";

		// Token: 0x04000004 RID: 4
		public const string UpdaterVshostName = "Editor.AutoUpdate.vshost";

		// Token: 0x04000005 RID: 5
		public const string EditorName = "CocosStudio";
	}
}
