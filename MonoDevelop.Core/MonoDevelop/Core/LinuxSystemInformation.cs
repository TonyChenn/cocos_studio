using System;
using System.Text;

namespace MonoDevelop.Core
{
	// Token: 0x02000233 RID: 563
	internal class LinuxSystemInformation : UnixSystemInformation
	{
		// Token: 0x060014F8 RID: 5368 RVA: 0x000561F6 File Offset: 0x000543F6
		internal override void AppendOperatingSystem(StringBuilder sb)
		{
			sb.AppendLine("\tLinux");
			base.AppendOperatingSystem(sb);
		}
	}
}
