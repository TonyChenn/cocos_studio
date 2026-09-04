using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000004 RID: 4
	internal class ActionGroups
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000021CC File Offset: 0x000003CC
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021EC File Offset: 0x000003EC
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
