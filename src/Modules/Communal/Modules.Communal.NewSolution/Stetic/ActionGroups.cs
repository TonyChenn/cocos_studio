using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000E RID: 14
	internal class ActionGroups
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00002DE3 File Offset: 0x00000FE3
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002DF0 File Offset: 0x00000FF0
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
