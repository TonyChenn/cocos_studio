using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000004 RID: 4
	internal class ActionGroups
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002178 File Offset: 0x00000378
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002198 File Offset: 0x00000398
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
