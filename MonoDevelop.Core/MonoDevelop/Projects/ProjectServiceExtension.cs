using System;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects
{
	// Token: 0x020000FE RID: 254
	public class ProjectServiceExtension
	{
		// Token: 0x0600090F RID: 2319 RVA: 0x00024948 File Offset: 0x00022B48
		internal ProjectServiceExtension GetNext(IBuildTarget item)
		{
			if (this.Next.SupportsItem(item))
			{
				return this.Next;
			}
			return this.Next.GetNext(item);
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0002496B File Offset: 0x00022B6B
		public virtual bool SupportsItem(IBuildTarget item)
		{
			return true;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0002496E File Offset: 0x00022B6E
		public virtual object GetService(SolutionItem item, Type type)
		{
			if (type.IsInstanceOfType(this))
			{
				return this;
			}
			return this.GetNext(item).GetService(item, type);
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00024989 File Offset: 0x00022B89
		public virtual object GetService(WorkspaceItem item, Type type)
		{
			if (type.IsInstanceOfType(this))
			{
				return this;
			}
			return this.GetNext(item).GetService(item, type);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x000249A4 File Offset: 0x00022BA4
		public virtual void Save(IProgressMonitor monitor, SolutionEntityItem item)
		{
			this.GetNext(item).Save(monitor, item);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x000249B4 File Offset: 0x00022BB4
		public virtual void Save(IProgressMonitor monitor, WorkspaceItem item)
		{
			this.GetNext(item).Save(monitor, item);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x000249C4 File Offset: 0x00022BC4
		public virtual List<FilePath> GetItemFiles(SolutionEntityItem item, bool includeReferencedFiles)
		{
			return this.GetNext(item).GetItemFiles(item, includeReferencedFiles);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x000249D4 File Offset: 0x00022BD4
		public virtual bool IsSolutionItemFile(string fileName)
		{
			return this.GetNext(UnknownItem.Instance).IsSolutionItemFile(fileName);
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x000249E7 File Offset: 0x00022BE7
		public virtual bool IsWorkspaceItemFile(string fileName)
		{
			return this.GetNext(UnknownItem.Instance).IsWorkspaceItemFile(fileName);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x000249FC File Offset: 0x00022BFC
		internal virtual SolutionEntityItem LoadSolutionItem(IProgressMonitor monitor, string fileName, ItemLoadCallback callback)
		{
			this.loadCallbackStack.Push(callback);
			SolutionEntityItem result;
			try
			{
				SolutionEntityItem solutionEntityItem = this.LoadSolutionItem(monitor, fileName);
				result = solutionEntityItem;
			}
			finally
			{
				this.loadCallbackStack.Pop();
			}
			return result;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00024A40 File Offset: 0x00022C40
		protected virtual SolutionEntityItem LoadSolutionItem(IProgressMonitor monitor, string fileName)
		{
			return this.GetNext(UnknownItem.Instance).LoadSolutionItem(monitor, fileName, this.loadCallbackStack.Peek());
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x00024A5F File Offset: 0x00022C5F
		public virtual WorkspaceItem LoadWorkspaceItem(IProgressMonitor monitor, string fileName)
		{
			return this.GetNext(UnknownItem.Instance).LoadWorkspaceItem(monitor, fileName);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00024A74 File Offset: 0x00022C74
		public virtual BuildResult RunTarget(IProgressMonitor monitor, IBuildTarget item, string target, ConfigurationSelector configuration)
		{
			if (target == "Build")
			{
				return this.Build(monitor, item, configuration);
			}
			if (target == "Clean")
			{
				this.Clean(monitor, item, configuration);
				return null;
			}
			return this.GetNext(item).RunTarget(monitor, item, target, configuration);
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00024AC3 File Offset: 0x00022CC3
		public virtual bool SupportsTarget(IBuildTarget item, string target)
		{
			if (item is SolutionEntityItem)
			{
				return this.SupportsTarget((SolutionEntityItem)item, target);
			}
			if (item is WorkspaceItem)
			{
				return this.SupportsTarget((WorkspaceItem)item, target);
			}
			return this.GetNext(item).SupportsTarget(item, target);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x00024AFF File Offset: 0x00022CFF
		protected virtual bool SupportsTarget(SolutionEntityItem item, string target)
		{
			return this.GetNext(item).SupportsTarget(item, target);
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00024B0F File Offset: 0x00022D0F
		protected virtual bool SupportsTarget(Solution solution, string target)
		{
			return this.GetNext(solution).SupportsTarget(solution, target);
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00024B1F File Offset: 0x00022D1F
		protected virtual bool SupportsTarget(WorkspaceItem item, string target)
		{
			if (item is Solution)
			{
				return this.SupportsTarget((Solution)item, target);
			}
			return this.GetNext(item).SupportsTarget(item, target);
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00024B45 File Offset: 0x00022D45
		public virtual bool SupportsExecute(IBuildTarget item)
		{
			if (item is SolutionEntityItem)
			{
				return this.SupportsExecute((SolutionEntityItem)item);
			}
			if (item is WorkspaceItem)
			{
				return this.SupportsExecute((WorkspaceItem)item);
			}
			return this.GetNext(item).SupportsExecute(item);
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00024B7E File Offset: 0x00022D7E
		protected virtual bool SupportsExecute(SolutionEntityItem item)
		{
			return this.GetNext(item).SupportsExecute(item);
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00024B8D File Offset: 0x00022D8D
		protected virtual bool SupportsExecute(Solution solution)
		{
			return this.GetNext(solution).SupportsExecute(solution);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00024B9C File Offset: 0x00022D9C
		protected virtual bool SupportsExecute(WorkspaceItem item)
		{
			if (item is Solution)
			{
				return this.SupportsExecute((Solution)item);
			}
			return this.GetNext(item).SupportsExecute(item);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00024BC0 File Offset: 0x00022DC0
		protected virtual void Clean(IProgressMonitor monitor, IBuildTarget item, ConfigurationSelector configuration)
		{
			if (item is SolutionEntityItem)
			{
				this.Clean(monitor, (SolutionEntityItem)item, configuration);
				return;
			}
			if (item is WorkspaceItem)
			{
				this.Clean(monitor, (WorkspaceItem)item, configuration);
				return;
			}
			this.GetNext(item).RunTarget(monitor, item, "Clean", configuration);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00024C10 File Offset: 0x00022E10
		protected virtual void Clean(IProgressMonitor monitor, SolutionEntityItem item, ConfigurationSelector configuration)
		{
			this.GetNext(item).RunTarget(monitor, item, "Clean", configuration);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00024C27 File Offset: 0x00022E27
		protected virtual void Clean(IProgressMonitor monitor, Solution item, ConfigurationSelector configuration)
		{
			this.GetNext(item).RunTarget(monitor, item, "Clean", configuration);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x00024C3E File Offset: 0x00022E3E
		protected virtual void Clean(IProgressMonitor monitor, WorkspaceItem item, ConfigurationSelector configuration)
		{
			if (item is Solution)
			{
				this.Clean(monitor, (Solution)item, configuration);
				return;
			}
			this.GetNext(item).RunTarget(monitor, item, "Clean", configuration);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00024C6C File Offset: 0x00022E6C
		protected virtual BuildResult Build(IProgressMonitor monitor, IBuildTarget item, ConfigurationSelector configuration)
		{
			if (item is SolutionEntityItem)
			{
				return this.Build(monitor, (SolutionEntityItem)item, configuration);
			}
			if (item is WorkspaceItem)
			{
				return this.Build(monitor, (WorkspaceItem)item, configuration);
			}
			return this.GetNext(item).RunTarget(monitor, item, "Build", configuration);
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00024CBB File Offset: 0x00022EBB
		protected virtual BuildResult Build(IProgressMonitor monitor, SolutionEntityItem item, ConfigurationSelector configuration)
		{
			return this.GetNext(item).RunTarget(monitor, item, "Build", configuration);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x00024CD1 File Offset: 0x00022ED1
		protected virtual BuildResult Build(IProgressMonitor monitor, WorkspaceItem item, ConfigurationSelector configuration)
		{
			if (item is Solution)
			{
				return this.Build(monitor, (Solution)item, configuration);
			}
			return this.GetNext(item).RunTarget(monitor, item, "Build", configuration);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00024CFE File Offset: 0x00022EFE
		protected virtual BuildResult Build(IProgressMonitor monitor, Solution solution, ConfigurationSelector configuration)
		{
			return this.GetNext(solution).RunTarget(monitor, solution, "Build", configuration);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00024D14 File Offset: 0x00022F14
		public virtual void Execute(IProgressMonitor monitor, IBuildTarget item, ExecutionContext context, ConfigurationSelector configuration)
		{
			if (item is SolutionEntityItem)
			{
				this.Execute(monitor, (SolutionEntityItem)item, context, configuration);
				return;
			}
			if (item is WorkspaceItem)
			{
				this.Execute(monitor, (WorkspaceItem)item, context, configuration);
				return;
			}
			this.GetNext(item).Execute(monitor, item, context, configuration);
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00024D64 File Offset: 0x00022F64
		protected virtual void Execute(IProgressMonitor monitor, SolutionEntityItem item, ExecutionContext context, ConfigurationSelector configuration)
		{
			this.GetNext(item).Execute(monitor, item, context, configuration);
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x00024D77 File Offset: 0x00022F77
		protected virtual void Execute(IProgressMonitor monitor, Solution solution, ExecutionContext context, ConfigurationSelector configuration)
		{
			this.GetNext(solution).Execute(monitor, solution, context, configuration);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00024D8A File Offset: 0x00022F8A
		protected virtual void Execute(IProgressMonitor monitor, WorkspaceItem item, ExecutionContext context, ConfigurationSelector configuration)
		{
			if (item is Solution)
			{
				this.Execute(monitor, (Solution)item, context, configuration);
				return;
			}
			this.GetNext(item).Execute(monitor, item, context, configuration);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x00024DB6 File Offset: 0x00022FB6
		public virtual bool CanExecute(IBuildTarget item, ExecutionContext context, ConfigurationSelector configuration)
		{
			if (item is SolutionEntityItem)
			{
				return this.CanExecute((SolutionEntityItem)item, context, configuration);
			}
			if (item is WorkspaceItem)
			{
				return this.CanExecute((WorkspaceItem)item, context, configuration);
			}
			return this.GetNext(item).CanExecute(item, context, configuration);
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00024DF5 File Offset: 0x00022FF5
		protected virtual bool CanExecute(SolutionEntityItem item, ExecutionContext context, ConfigurationSelector configuration)
		{
			return this.GetNext(item).CanExecute(item, context, configuration);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00024E06 File Offset: 0x00023006
		protected virtual bool CanExecute(Solution solution, ExecutionContext context, ConfigurationSelector configuration)
		{
			return this.GetNext(solution).CanExecute(solution, context, configuration);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00024E17 File Offset: 0x00023017
		protected virtual bool CanExecute(WorkspaceItem item, ExecutionContext context, ConfigurationSelector configuration)
		{
			if (item is Solution)
			{
				return this.CanExecute((Solution)item, context, configuration);
			}
			return this.GetNext(item).CanExecute(item, context, configuration);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x00024E3F File Offset: 0x0002303F
		public virtual IEnumerable<ExecutionTarget> GetExecutionTargets(IBuildTarget item, ConfigurationSelector configuration)
		{
			if (item is SolutionEntityItem)
			{
				return this.GetExecutionTargets((SolutionEntityItem)item, configuration);
			}
			if (item is WorkspaceItem)
			{
				return this.GetExecutionTargets((WorkspaceItem)item, configuration);
			}
			return this.GetNext(item).GetExecutionTargets(item, configuration);
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00024E7B File Offset: 0x0002307B
		protected virtual IEnumerable<ExecutionTarget> GetExecutionTargets(SolutionEntityItem item, ConfigurationSelector configuration)
		{
			return this.GetNext(item).GetExecutionTargets(item, configuration);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00024E8B File Offset: 0x0002308B
		protected virtual IEnumerable<ExecutionTarget> GetExecutionTargets(Solution solution, ConfigurationSelector configuration)
		{
			return this.GetNext(solution).GetExecutionTargets(solution, configuration);
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x00024E9B File Offset: 0x0002309B
		protected virtual IEnumerable<ExecutionTarget> GetExecutionTargets(WorkspaceItem item, ConfigurationSelector configuration)
		{
			if (item is Solution)
			{
				return this.GetExecutionTargets((Solution)item, configuration);
			}
			return this.GetNext(item).GetExecutionTargets(item, configuration);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x00024EC1 File Offset: 0x000230C1
		public virtual bool GetNeedsBuilding(IBuildTarget item, ConfigurationSelector configuration)
		{
			if (item is SolutionEntityItem)
			{
				return this.GetNeedsBuilding((SolutionEntityItem)item, configuration);
			}
			if (item is WorkspaceItem)
			{
				return this.GetNeedsBuilding((WorkspaceItem)item, configuration);
			}
			return this.GetNext(item).GetNeedsBuilding(item, configuration);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00024EFD File Offset: 0x000230FD
		protected virtual bool GetNeedsBuilding(SolutionEntityItem item, ConfigurationSelector configuration)
		{
			return this.GetNext(item).GetNeedsBuilding(item, configuration);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00024F0D File Offset: 0x0002310D
		protected virtual bool GetNeedsBuilding(Solution item, ConfigurationSelector configuration)
		{
			return this.GetNext(item).GetNeedsBuilding(item, configuration);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00024F1D File Offset: 0x0002311D
		protected virtual bool GetNeedsBuilding(WorkspaceItem item, ConfigurationSelector configuration)
		{
			if (item is Solution)
			{
				return this.GetNeedsBuilding((Solution)item, configuration);
			}
			return this.GetNext(item).GetNeedsBuilding(item, configuration);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00024F43 File Offset: 0x00023143
		public virtual void SetNeedsBuilding(IBuildTarget item, bool val, ConfigurationSelector configuration)
		{
			if (item is SolutionEntityItem)
			{
				this.SetNeedsBuilding((SolutionEntityItem)item, val, configuration);
				return;
			}
			if (item is WorkspaceItem)
			{
				this.SetNeedsBuilding((WorkspaceItem)item, val, configuration);
				return;
			}
			this.GetNext(item).SetNeedsBuilding(item, val, configuration);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00024F82 File Offset: 0x00023182
		protected virtual void SetNeedsBuilding(SolutionEntityItem item, bool val, ConfigurationSelector configuration)
		{
			this.GetNext(item).SetNeedsBuilding(item, val, configuration);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00024F93 File Offset: 0x00023193
		protected virtual void SetNeedsBuilding(Solution item, bool val, ConfigurationSelector configuration)
		{
			this.GetNext(item).SetNeedsBuilding(item, val, configuration);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00024FA4 File Offset: 0x000231A4
		protected virtual void SetNeedsBuilding(WorkspaceItem item, bool val, ConfigurationSelector configuration)
		{
			if (item is Solution)
			{
				this.SetNeedsBuilding((Solution)item, val, configuration);
				return;
			}
			this.GetNext(item).SetNeedsBuilding(item, val, configuration);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00024FCC File Offset: 0x000231CC
		internal virtual BuildResult Compile(IProgressMonitor monitor, SolutionEntityItem item, BuildData buildData, ItemCompileCallback callback)
		{
			this.compileCallbackStack.Push(callback);
			BuildResult result;
			try
			{
				BuildResult buildResult = this.Compile(monitor, item, buildData);
				result = buildResult;
			}
			finally
			{
				this.compileCallbackStack.Pop();
			}
			return result;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00025014 File Offset: 0x00023214
		protected virtual BuildResult Compile(IProgressMonitor monitor, SolutionEntityItem item, BuildData buildData)
		{
			return this.GetNext(item).Compile(monitor, item, buildData, this.compileCallbackStack.Peek());
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00025030 File Offset: 0x00023230
		public virtual IEnumerable<string> GetReferencedAssemblies(DotNetProject project, ConfigurationSelector configuration, bool includeProjectReferences)
		{
			return this.GetNext(project).GetReferencedAssemblies(project, configuration, includeProjectReferences);
		}

		// Token: 0x040002DE RID: 734
		internal ProjectServiceExtension Next;

		// Token: 0x040002DF RID: 735
		private Stack<ItemLoadCallback> loadCallbackStack = new Stack<ItemLoadCallback>();

		// Token: 0x040002E0 RID: 736
		private Stack<ItemCompileCallback> compileCallbackStack = new Stack<ItemCompileCallback>();
	}
}
