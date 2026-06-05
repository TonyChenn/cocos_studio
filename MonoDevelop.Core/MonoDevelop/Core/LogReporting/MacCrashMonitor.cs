using System;
using System.IO;

namespace MonoDevelop.Core.LogReporting
{
	// Token: 0x02000238 RID: 568
	internal class MacCrashMonitor : CrashMonitor
	{
		// Token: 0x06001517 RID: 5399 RVA: 0x000564CC File Offset: 0x000546CC
		public MacCrashMonitor(int pid) : base(pid, MacCrashMonitor.CrashLogDirectory, "mono*")
		{
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x000564DF File Offset: 0x000546DF
		protected override void OnCrashDetected(CrashEventArgs e)
		{
			if (this.IsFromMonitoredPid(e.CrashLogPath))
			{
				base.OnCrashDetected(e);
			}
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x000564F8 File Offset: 0x000546F8
		private bool IsFromMonitoredPid(string logPath)
		{
			bool result;
			using (StreamReader streamReader = new StreamReader(File.OpenRead(logPath)))
			{
				string text = streamReader.ReadLine();
				if (string.IsNullOrEmpty(text) || !text.StartsWith("Process"))
				{
					result = false;
				}
				else
				{
					int num = text.LastIndexOf('[');
					int num2 = text.LastIndexOf(']');
					if (num < 0 || num2 < 0)
					{
						result = false;
					}
					else
					{
						num++;
						string s = text.Substring(num, num2 - num);
						int num3;
						if (!int.TryParse(s, out num3))
						{
							result = false;
						}
						else
						{
							Console.WriteLine("Parsed Pid was: {0}", num3);
							result = (num3 == base.Pid);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0400065F RID: 1631
		private static readonly string CrashLogDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Logs/CrashReporter");
	}
}
