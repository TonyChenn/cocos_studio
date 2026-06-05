using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000058 RID: 88
	internal class ActionGroups
	{
		// Token: 0x060001DF RID: 479 RVA: 0x00008808 File Offset: 0x00006A08
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00008828 File Offset: 0x00006A28
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
