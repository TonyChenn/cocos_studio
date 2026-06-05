using System;
using System.Text;

namespace MonoDevelop.Core
{
	// Token: 0x02000232 RID: 562
	internal class WindowsSystemInformation : SystemInformation
	{
		// Token: 0x060014F6 RID: 5366 RVA: 0x00056198 File Offset: 0x00054398
		internal override void AppendOperatingSystem(StringBuilder sb)
		{
			sb.Append("\tWindows ");
			sb.Append(Environment.OSVersion.Version.ToString());
			if (IntPtr.Size == 8 || Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432") != null)
			{
				sb.Append(" (64-bit)");
			}
			sb.AppendLine();
		}
	}
}
