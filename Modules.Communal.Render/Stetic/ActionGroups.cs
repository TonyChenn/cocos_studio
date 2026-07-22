using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000020 RID: 32
	internal class ActionGroups
	{
		// Token: 0x0600010D RID: 269 RVA: 0x00006FC8 File Offset: 0x000051C8
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006FE8 File Offset: 0x000051E8
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
