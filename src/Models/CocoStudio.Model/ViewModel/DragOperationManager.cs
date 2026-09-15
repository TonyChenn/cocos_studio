using System;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Model.Interface;
using CocoStudio.Projects;
using Mono.Addins;

namespace CocoStudio.Model.ViewModel
{
	public class DragOperationManager
	{
		public static IDragOperation Current { get; private set; }

		static DragOperationManager()
		{
			DragOperationManager.Initialize();
		}

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

		private static void SetDefaultOperation()
		{
			DragOperationManager.Current = new DragOperation2D();
			DragOperationManager.dragOperationArray = new IDragOperation[]
			{
				DragOperationManager.Current
			};
		}

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

		public static void OnProjectChanged(CocosItem project)
		{
			DragOperationManager.Current = DragOperationManager.GetOperationMode(project);
		}

		private static IDragOperation[] dragOperationArray;
	}
}
