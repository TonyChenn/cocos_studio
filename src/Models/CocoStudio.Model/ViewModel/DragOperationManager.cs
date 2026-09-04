using System;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Model.Interface;
using CocoStudio.Projects;
using Mono.Addins;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000DC RID: 220
	public class DragOperationManager
	{
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x0001B540 File Offset: 0x00019740
		// (set) Token: 0x060006D0 RID: 1744 RVA: 0x0001B556 File Offset: 0x00019756
		public static IDragOperation Current { get; private set; }

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001B55E File Offset: 0x0001975E
		static DragOperationManager()
		{
			DragOperationManager.Initialize();
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001B568 File Offset: 0x00019768
		public static void Initialize()
		{
			try
			{
				IDragOperation[] extensionObjects = AddinManager.GetExtensionObjects<IDragOperation>(false);
				DragOperationManager.dragOperationArray = extensionObjects;
				DragOperationManager.Current = DragOperationManager.dragOperationArray.FirstOrDefault<IDragOperation>();
				if (DragOperationManager.Current == null)
				{
					DragOperationManager.SetDefaultOperation();
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Load IOperationMode failed.", exception);
				DragOperationManager.SetDefaultOperation();
			}
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001B5DC File Offset: 0x000197DC
		private static void SetDefaultOperation()
		{
			DragOperationManager.Current = new DragOperation2D();
			DragOperationManager.dragOperationArray = new IDragOperation[]
			{
				DragOperationManager.Current
			};
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001B60C File Offset: 0x0001980C
		private static IDragOperation GetOperationMode(CocosItem project)
		{
			IDragOperation result;
			if (project == null)
			{
				result = null;
			}
			else
			{
				if (null != DragOperationManager.dragOperationArray)
				{
					foreach (IDragOperation dragOperation in DragOperationManager.dragOperationArray)
					{
						if (dragOperation.CanHandle(project))
						{
							return dragOperation;
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001B674 File Offset: 0x00019874
		public static void OnProjectChanged(CocosItem project)
		{
			DragOperationManager.Current = DragOperationManager.GetOperationMode(project);
		}

		// Token: 0x040002E0 RID: 736
		private static IDragOperation[] dragOperationArray;
	}
}
