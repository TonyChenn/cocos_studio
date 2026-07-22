using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000B RID: 11
	internal class ActionGroups
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00003007 File Offset: 0x00001207
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003014 File Offset: 0x00001214
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
