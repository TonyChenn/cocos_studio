using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000028 RID: 40
	internal class ActionGroups
	{
		// Token: 0x0600014B RID: 331 RVA: 0x00006404 File Offset: 0x00004604
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00006411 File Offset: 0x00004611
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
