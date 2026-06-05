using System;
using System.Diagnostics;
using System.Text;

namespace MonoDevelop.Core
{
	// Token: 0x02000230 RID: 560
	public class UnixSystemInformation : SystemInformation
	{
		// Token: 0x060014EE RID: 5358 RVA: 0x00055FC8 File Offset: 0x000541C8
		internal override void AppendOperatingSystem(StringBuilder sb)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo("uname", "-a")
			{
				RedirectStandardOutput = true,
				UseShellExecute = false
			};
			Process process = Process.Start(startInfo);
			process.WaitForExit(500);
			if (process.HasExited && process.ExitCode == 0)
			{
				string text = process.StandardOutput.ReadLine();
				if (Platform.IsMac && text != null)
				{
					string[] array = text.Split(new string[]
					{
						";",
						": "
					}, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = array[i].Trim();
					}
					text = string.Join("\n    ", array);
				}
				sb.AppendLine(text);
			}
		}
	}
}
