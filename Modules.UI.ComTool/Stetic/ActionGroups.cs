using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000D RID: 13
	internal class ActionGroups
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00002C5C File Offset: 0x00000E5C
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002C7C File Offset: 0x00000E7C
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
