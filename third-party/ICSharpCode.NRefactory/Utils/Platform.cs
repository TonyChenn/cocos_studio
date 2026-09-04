using System;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// Platform-specific code.
	/// </summary>
	// Token: 0x02000122 RID: 290
	public static class Platform
	{
		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x0001E6D8 File Offset: 0x0001D6D8
		public static StringComparer FileNameComparer
		{
			get
			{
				switch (Environment.OSVersion.Platform)
				{
				case PlatformID.Unix:
				case PlatformID.MacOSX:
					return StringComparer.Ordinal;
				}
				return StringComparer.OrdinalIgnoreCase;
			}
		}
	}
}
