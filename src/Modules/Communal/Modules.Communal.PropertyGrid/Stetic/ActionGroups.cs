using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000020 RID: 32
	internal class ActionGroups
	{
		// Token: 0x060000DE RID: 222 RVA: 0x00005570 File Offset: 0x00003770
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005590 File Offset: 0x00003790
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
