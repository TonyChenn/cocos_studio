using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000015 RID: 21
	internal class ActionGroups
	{
		// Token: 0x06000076 RID: 118 RVA: 0x00004264 File Offset: 0x00002464
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004284 File Offset: 0x00002484
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
