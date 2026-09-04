using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000B RID: 11
	internal class ActionGroups
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002E97 File Offset: 0x00001097
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002EA4 File Offset: 0x000010A4
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
