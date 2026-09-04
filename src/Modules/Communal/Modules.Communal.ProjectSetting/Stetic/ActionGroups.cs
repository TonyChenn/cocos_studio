using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000008 RID: 8
	internal class ActionGroups
	{
		// Token: 0x06000018 RID: 24 RVA: 0x000023C7 File Offset: 0x000005C7
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000023D4 File Offset: 0x000005D4
		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
