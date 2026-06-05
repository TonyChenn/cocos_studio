using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000E RID: 14
	internal class ActionGroups
	{
		// Token: 0x0600007B RID: 123 RVA: 0x00003A68 File Offset: 0x00001C68
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003A75 File Offset: 0x00001C75
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
