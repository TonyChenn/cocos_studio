using System;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects
{
	// Token: 0x020000FF RID: 255
	internal class DefaultProjectServiceExtension : ProjectServiceExtension
	{
		// Token: 0x06000944 RID: 2372 RVA: 0x0002505F File Offset: 0x0002325F
		public override object GetService(SolutionItem item, Type type)
		{
			return item.OnGetService(type);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00025068 File Offset: 0x00023268
		public override object GetService(WorkspaceItem item, Type type)
		{
			return item.OnGetService(type);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x00025071 File Offset: 0x00023271
		public override void Save(IProgressMonitor monitor, SolutionEntityItem entry)
		{
			FileService.RequestFileEdit(entry.GetItemFiles(false), true);
			entry.OnSave(monitor);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00025088 File Offset: 0x00023288
		public override void Save(IProgressMonitor monitor, WorkspaceItem entry)
		{
			entry.OnSave(monitor);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00025091 File Offset: 0x00023291
		public override List<FilePath> GetItemFiles(SolutionEntityItem entry, bool includeReferencedFiles)
		{
			return entry.OnGetItemFiles(includeReferencedFiles);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0002509A File Offset: 0x0002329A
		public override bool IsSolutionItemFile(string filename)
		{
			return Services.ProjectService.IsSolutionItemFileInternal(filename);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x000250A7 File Offset: 0x000232A7
		public override bool IsWorkspaceItemFile(string filename)
		{
			return Services.ProjectService.IsWorkspaceItemFileInternal(filename);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x000250B4 File Offset: 0x000232B4
		internal override SolutionEntityItem LoadSolutionItem(IProgressMonitor monitor, string fileName, ItemLoadCallback callback)
		{
			return callback(monitor, fileName);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x000250BE File Offset: 0x000232BE
		public override WorkspaceItem LoadWorkspaceItem(IProgressMonitor monitor, string fileName)
		{
			return Services.ProjectService.InternalReadWorkspaceItem(fileName, monitor);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x000250CC File Offset: 0x000232CC
		public override BuildResult RunTarget(IProgressMonitor monitor, IBuildTarget item, string target, ConfigurationSelector configuration)
		{
			BuildResult buildResult;
			if (item is WorkspaceItem)
			{
				buildResult = ((WorkspaceItem)item).OnRunTarget(monitor, target, configuration);
			}
			else
			{
				if (!(item is SolutionItem))
				{
					throw new InvalidOperationException("Unknown item type: " + item);
				}
				buildResult = ((SolutionItem)item).OnRunTarget(monitor, target, configuration);
			}
			if (buildResult != null)
			{
				buildResult.SourceTarget = item;
			}
			return buildResult;
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x00025129 File Offset: 0x00023329
		public override bool SupportsTarget(IBuildTarget item, string target)
		{
			if (item is WorkspaceItem)
			{
				return ((WorkspaceItem)item).OnGetSupportsTarget(target);
			}
			if (item is SolutionItem)
			{
				return ((SolutionItem)item).OnGetSupportsTarget(target);
			}
			throw new InvalidOperationException("Unknown item type: " + item);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00025165 File Offset: 0x00023365
		public override bool SupportsExecute(IBuildTarget item)
		{
			if (item is WorkspaceItem)
			{
				return ((WorkspaceItem)item).OnGetSupportsExecute();
			}
			if (item is SolutionItem)
			{
				return ((SolutionItem)item).OnGetSupportsExecute();
			}
			throw new InvalidOperationException("Unknown item type: " + item);
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x000251A0 File Offset: 0x000233A0
		public override void Execute(IProgressMonitor monitor, IBuildTarget item, ExecutionContext context, ConfigurationSelector configuration)
		{
			if (item is SolutionEntityItem)
			{
				SolutionEntityItem solutionEntityItem = (SolutionEntityItem)item;
				SolutionItemConfiguration configuration2 = solutionEntityItem.GetConfiguration(configuration);
				if (configuration2 != null && configuration2.CustomCommands.HasCommands(CustomCommandType.Execute))
				{
					configuration2.CustomCommands.ExecuteCommand(monitor, solutionEntityItem, CustomCommandType.Execute, context, configuration);
					return;
				}
				solutionEntityItem.OnExecute(monitor, context, configuration);
				return;
			}
			else
			{
				if (item is WorkspaceItem)
				{
					((WorkspaceItem)item).OnExecute(monitor, context, configuration);
					return;
				}
				if (item is SolutionItem)
				{
					((SolutionItem)item).OnExecute(monitor, context, configuration);
					return;
				}
				throw new InvalidOperationException("Unknown item type: " + item);
			}
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00025234 File Offset: 0x00023434
		public override bool CanExecute(IBuildTarget item, ExecutionContext context, ConfigurationSelector configuration)
		{
			if (item is SolutionEntityItem)
			{
				SolutionEntityItem solutionEntityItem = (SolutionEntityItem)item;
				SolutionItemConfiguration configuration2 = solutionEntityItem.GetConfiguration(configuration);
				if (configuration2 != null && configuration2.CustomCommands.HasCommands(CustomCommandType.Execute))
				{
					return configuration2.CustomCommands.CanExecute(solutionEntityItem, CustomCommandType.Execute, context, configuration);
				}
				return solutionEntityItem.OnGetCanExecute(context, configuration);
			}
			else
			{
				if (item is WorkspaceItem)
				{
					return ((WorkspaceItem)item).OnGetCanExecute(context, configuration);
				}
				if (item is SolutionItem)
				{
					return ((SolutionItem)item).OnGetCanExecute(context, configuration);
				}
				throw new InvalidOperationException("Unknown item type: " + item);
			}
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x000252BE File Offset: 0x000234BE
		public override IEnumerable<ExecutionTarget> GetExecutionTargets(IBuildTarget item, ConfigurationSelector configuration)
		{
			if (item is WorkspaceItem)
			{
				return ((WorkspaceItem)item).OnGetExecutionTargets(configuration);
			}
			if (item is SolutionItem)
			{
				return ((SolutionItem)item).OnGetExecutionTargets(configuration);
			}
			throw new InvalidOperationException("Unknown item type: " + item);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x000252FC File Offset: 0x000234FC
		public override bool GetNeedsBuilding(IBuildTarget item, ConfigurationSelector configuration)
		{
			if (item is SolutionItem)
			{
				SolutionItem solutionItem = (SolutionItem)item;
				bool flag = false;
				bool result;
				if (this.needsBuildingCache == null)
				{
					this.needsBuildingCache = new Dictionary<SolutionItem, bool>();
					flag = true;
				}
				else if (this.needsBuildingCache.TryGetValue(solutionItem, out result))
				{
					return result;
				}
				bool flag2 = solutionItem.OnGetNeedsBuilding(configuration);
				this.needsBuildingCache[solutionItem] = flag2;
				if (flag)
				{
					this.needsBuildingCache = null;
				}
				return flag2;
			}
			if (item is WorkspaceItem)
			{
				return ((WorkspaceItem)item).OnGetNeedsBuilding(configuration);
			}
			throw new InvalidOperationException("Unknown item type: " + item);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0002538C File Offset: 0x0002358C
		public override void SetNeedsBuilding(IBuildTarget item, bool val, ConfigurationSelector configuration)
		{
			if (item is SolutionItem)
			{
				SolutionItem solutionItem = (SolutionItem)item;
				solutionItem.OnSetNeedsBuilding(val, configuration);
				return;
			}
			if (item is WorkspaceItem)
			{
				((WorkspaceItem)item).OnSetNeedsBuilding(val, configuration);
				return;
			}
			throw new InvalidOperationException("Unknown item type: " + item);
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x000253D7 File Offset: 0x000235D7
		internal override BuildResult Compile(IProgressMonitor monitor, SolutionEntityItem item, BuildData buildData, ItemCompileCallback callback)
		{
			return callback(monitor, item, buildData);
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x000253E3 File Offset: 0x000235E3
		public override IEnumerable<string> GetReferencedAssemblies(DotNetProject project, ConfigurationSelector configuration, bool includeProjectReferences)
		{
			return project.OnGetReferencedAssemblies(configuration, includeProjectReferences);
		}

		// Token: 0x040002E1 RID: 737
		private Dictionary<SolutionItem, bool> needsBuildingCache;
	}
}
