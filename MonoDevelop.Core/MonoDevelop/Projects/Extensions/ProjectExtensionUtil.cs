using System;
using System.IO;
using System.Threading;
using MonoDevelop.Core;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x02000196 RID: 406
	public static class ProjectExtensionUtil
	{
		// Token: 0x06000F9E RID: 3998 RVA: 0x0003A540 File Offset: 0x00038740
		public static ISolutionItemHandler GetItemHandler(SolutionItem item)
		{
			return item.GetItemHandler();
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0003A548 File Offset: 0x00038748
		public static void InstallHandler(ISolutionItemHandler handler, SolutionItem item)
		{
			item.SetItemHandler(handler);
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0003A554 File Offset: 0x00038754
		public static SolutionEntityItem LoadSolutionItem(IProgressMonitor monitor, string fileName, ItemLoadCallback callback)
		{
			SolutionEntityItem result;
			using (Counters.ReadSolutionItem.BeginTiming("Read project " + fileName))
			{
				result = Services.ProjectService.GetExtensionChain(null).LoadSolutionItem(monitor, fileName, callback);
			}
			return result;
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0003A5A8 File Offset: 0x000387A8
		public static BuildResult Compile(IProgressMonitor monitor, SolutionEntityItem item, BuildData buildData, ItemCompileCallback callback)
		{
			return Services.ProjectService.GetExtensionChain(item).Compile(monitor, item, buildData, callback);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0003A5C0 File Offset: 0x000387C0
		public static void BeginLoadOperation()
		{
			Interlocked.Increment(ref ProjectExtensionUtil.loading);
			LoadOperation loadOperation = (LoadOperation)Thread.GetData(ProjectExtensionUtil.loadControlSlot);
			if (loadOperation == null)
			{
				loadOperation = new LoadOperation();
				Thread.SetData(ProjectExtensionUtil.loadControlSlot, loadOperation);
			}
			loadOperation.LoadingCount++;
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0003A60C File Offset: 0x0003880C
		public static void EndLoadOperation()
		{
			Interlocked.Decrement(ref ProjectExtensionUtil.loading);
			LoadOperation loadOperation = (LoadOperation)Thread.GetData(ProjectExtensionUtil.loadControlSlot);
			if (loadOperation != null && --loadOperation.LoadingCount == 0)
			{
				Thread.SetData(ProjectExtensionUtil.loadControlSlot, null);
				loadOperation.End();
			}
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0003A65C File Offset: 0x0003885C
		public static void LoadControl(ILoadController rc)
		{
			if (ProjectExtensionUtil.loading == 0)
			{
				return;
			}
			LoadOperation loadOperation = (LoadOperation)Thread.GetData(ProjectExtensionUtil.loadControlSlot);
			if (loadOperation != null)
			{
				loadOperation.Add(rc);
			}
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0003A68C File Offset: 0x0003888C
		public static string EncodePath(SolutionEntityItem item, string path, string oldPath)
		{
			IPathHandler pathHandler = item.GetItemHandler() as IPathHandler;
			if (pathHandler != null)
			{
				return pathHandler.EncodePath(path, oldPath);
			}
			string directoryName = Path.GetDirectoryName(item.FileName);
			return FileService.RelativeToAbsolutePath(directoryName, path);
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0003A6CC File Offset: 0x000388CC
		public static string DecodePath(SolutionEntityItem item, string path)
		{
			IPathHandler pathHandler = item.GetItemHandler() as IPathHandler;
			if (pathHandler != null)
			{
				return pathHandler.DecodePath(path);
			}
			string directoryName = Path.GetDirectoryName(item.FileName);
			return FileService.AbsoluteToRelativePath(directoryName, path);
		}

		// Token: 0x04000484 RID: 1156
		private static int loading;

		// Token: 0x04000485 RID: 1157
		private static LocalDataStoreSlot loadControlSlot = Thread.AllocateDataSlot();
	}
}
