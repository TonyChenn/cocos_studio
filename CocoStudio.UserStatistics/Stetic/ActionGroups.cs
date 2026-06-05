using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000017 RID: 23
	internal class ActionGroups
	{
		// Token: 0x0600006D RID: 109 RVA: 0x00003C20 File Offset: 0x00001E20
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003C40 File Offset: 0x00001E40
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
