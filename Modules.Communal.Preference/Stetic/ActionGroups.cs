using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000005 RID: 5
	internal class ActionGroups
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002173 File Offset: 0x00000373
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002180 File Offset: 0x00000380
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
