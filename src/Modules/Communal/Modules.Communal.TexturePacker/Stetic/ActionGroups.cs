using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000006 RID: 6
	internal class ActionGroups
	{
		// Token: 0x06000013 RID: 19 RVA: 0x000022AF File Offset: 0x000004AF
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022BC File Offset: 0x000004BC
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
