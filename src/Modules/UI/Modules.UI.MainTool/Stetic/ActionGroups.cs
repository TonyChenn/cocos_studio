using System;
using Gtk;

namespace Stetic
{
	internal class ActionGroups
	{
		public static ActionGroup GetActionGroup(Type type)
		{
			return ActionGroups.GetActionGroup(type.FullName);
		}

		public static ActionGroup GetActionGroup(string name)
		{
			return null;
		}
	}
}
