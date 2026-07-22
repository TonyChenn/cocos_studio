using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000003 RID: 3
	internal class ActionGroups
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002078 File Offset: 0x00000278
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002098 File Offset: 0x00000298
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
