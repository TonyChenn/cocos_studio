using System;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core.AddIns;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000B4 RID: 180
	public class MsNetTargetRuntimeFactory : ITargetRuntimeFactory
	{
		// Token: 0x0600062D RID: 1581 RVA: 0x00017284 File Offset: 0x00015484
		public IEnumerable<TargetRuntime> CreateRuntimes()
		{
			if (Platform.IsWindows)
			{
				if (Type.GetType("Mono.Runtime") == null)
				{
					yield return new MsNetTargetRuntime(true);
				}
				else
				{
					string msnetDir = Environment.SystemDirectory + "\\..\\Microsoft.NET\\Framework";
					if (Directory.Exists(msnetDir))
					{
						yield return new MsNetTargetRuntime(false);
					}
				}
			}
			yield break;
		}
	}
}
