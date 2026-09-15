using System;
using CocoStudio.Core;
using CocoStudio.Core.Events;

namespace CocoStudio.Model.DataModel
{
	public class ActionTagManager
	{
		static ActionTagManager()
		{
			Services.ProjectOperations.CurrentSelectedSolutionChanged += ActionTagManager.HandleCurrentSelectedSolutionChanged;
		}

		private static void HandleCurrentSelectedSolutionChanged(object sender, SolutionEventArgs e)
		{
			ActionTagManager.ResetObjectActionTag();
		}

		public static int CreateObjectActionTag()
		{
			string text = Guid.NewGuid().ToString();
			return text.GetHashCode();
		}

		public static void RefreshObjectActionTag(int iTag)
		{
			if (ActionTagManager.tag < iTag)
			{
				ActionTagManager.tag = iTag;
			}
		}

		public static void ResetObjectActionTag()
		{
			ActionTagManager.tag = 0;
		}

		private static int tag = 0;
	}
}
