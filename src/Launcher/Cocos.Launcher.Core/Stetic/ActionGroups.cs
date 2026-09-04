using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000010 RID: 16
	internal class ActionGroups
	{
		// Token: 0x06000081 RID: 129 RVA: 0x00004BF3 File Offset: 0x00002DF3
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004C00 File Offset: 0x00002E00
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
