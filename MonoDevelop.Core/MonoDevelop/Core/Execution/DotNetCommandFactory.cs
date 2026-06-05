using System;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000249 RID: 585
	internal class DotNetCommandFactory : ICommandFactory
	{
		// Token: 0x06001591 RID: 5521 RVA: 0x0005787C File Offset: 0x00055A7C
		public ProcessExecutionCommand CreateCommand(string file)
		{
			string text = file.ToLower();
			if (text.EndsWith(".exe") || text.EndsWith(".dll"))
			{
				return new DotNetExecutionCommand(file);
			}
			return null;
		}
	}
}
