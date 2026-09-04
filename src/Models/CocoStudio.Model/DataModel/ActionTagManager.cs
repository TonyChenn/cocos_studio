using System;
using CocoStudio.Core;
using CocoStudio.Core.Events;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000003 RID: 3
	public class ActionTagManager
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020B2 File Offset: 0x000002B2
		static ActionTagManager()
		{
			Services.ProjectOperations.CurrentSelectedSolutionChanged += ActionTagManager.HandleCurrentSelectedSolutionChanged;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020D3 File Offset: 0x000002D3
		private static void HandleCurrentSelectedSolutionChanged(object sender, SolutionEventArgs e)
		{
			ActionTagManager.ResetObjectActionTag();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020DC File Offset: 0x000002DC
		public static int CreateObjectActionTag()
		{
			string text = Guid.NewGuid().ToString();
			return text.GetHashCode();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002108 File Offset: 0x00000308
		public static void RefreshObjectActionTag(int iTag)
		{
			if (ActionTagManager.tag < iTag)
			{
				ActionTagManager.tag = iTag;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000212D File Offset: 0x0000032D
		public static void ResetObjectActionTag()
		{
			ActionTagManager.tag = 0;
		}

		// Token: 0x04000001 RID: 1
		private static int tag = 0;
	}
}
