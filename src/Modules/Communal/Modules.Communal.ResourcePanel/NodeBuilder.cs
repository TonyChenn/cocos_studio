using System;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200000E RID: 14
	[TypeExtensionPoint(ExtensionAttributeType = typeof(ResourcePanelExtensionAttribute), NodeType = typeof(ResourcePanelExtensionNode))]
	public abstract class NodeBuilder : IDisposable
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000054 RID: 84
		public abstract Type NodeDataType { get; }

		// Token: 0x06000055 RID: 85 RVA: 0x0000316F File Offset: 0x0000136F
		internal NodeBuilder()
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003178 File Offset: 0x00001378
		internal void SetContext(ITreeBuilderContext context)
		{
			this.context = context;
			try
			{
				this.Initialize();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000031A8 File Offset: 0x000013A8
		protected virtual void Initialize()
		{
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000031AA File Offset: 0x000013AA
		protected ITreeBuilderContext Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000031B2 File Offset: 0x000013B2
		public void BuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			this.OnBuildNodeBefore(treeBuilder, dataObject, nodeInfo);
			this.OnBuildNode(treeBuilder, dataObject, nodeInfo);
			this.OnBuildNodeAfter(treeBuilder, dataObject, nodeInfo);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000031CF File Offset: 0x000013CF
		protected virtual void OnBuildNodeBefore(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000031D1 File Offset: 0x000013D1
		protected virtual void OnBuildNodeAfter(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000031D3 File Offset: 0x000013D3
		protected virtual void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			nodeInfo.IconInfo = this.GetIcon(dataObject);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000031E2 File Offset: 0x000013E2
		public virtual void BuildChildNodes(ITreeBuild treeBuilder, object dataObject)
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000031E4 File Offset: 0x000013E4
		public virtual void Dispose()
		{
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000031E6 File Offset: 0x000013E6
		public virtual void AddChild(ITreeBuild treeBuilder, object dateObject)
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000031E8 File Offset: 0x000013E8
		public virtual void OnNodeAdded(object dateObject)
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000031EA File Offset: 0x000013EA
		public virtual void OnNodeRemoved(object dateObject)
		{
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000031EC File Offset: 0x000013EC
		public virtual bool CanRename()
		{
			return true;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000031EF File Offset: 0x000013EF
		public bool CanDrag(object dataObject)
		{
			return this.OnCanDrag(dataObject);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000031F8 File Offset: 0x000013F8
		protected virtual bool OnCanDrag(object dataObject)
		{
			return true;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000031FB File Offset: 0x000013FB
		public void Rename(ITreeBuild treeBuilder, object dataObject, string newName)
		{
			this.OnRename(treeBuilder, dataObject, newName);
			Services.Workspace.SaveCurrentSolution();
			Services.Workbench.SaveAll();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000321A File Offset: 0x0000141A
		protected virtual void OnRename(ITreeBuild treeBuilder, object dataObject, string newName)
		{
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000321C File Offset: 0x0000141C
		internal virtual bool CanDelete()
		{
			return true;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000321F File Offset: 0x0000141F
		public void Delete(ITreeBuild treeBuilder, object dataObject, IProgressMonitor monitor, bool isDelete = false)
		{
			if (isDelete)
			{
				this.OnDelete(treeBuilder, dataObject, monitor);
			}
			else
			{
				this.OnRemove(treeBuilder, dataObject, monitor);
			}
			Services.Workspace.Save(Services.ProgressMonitors.Default);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000324D File Offset: 0x0000144D
		protected virtual void OnDelete(ITreeBuild treeBuilder, object dataObject, IProgressMonitor monitor)
		{
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000324F File Offset: 0x0000144F
		protected virtual void OnRemove(ITreeBuild treeBuilder, object dataObject, IProgressMonitor monitor)
		{
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003251 File Offset: 0x00001451
		public virtual ResourceFolder GetTargetFolder(object dataObject)
		{
			return null;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003254 File Offset: 0x00001454
		public virtual string CanMove(object moveSource, object moveTarget, TreeViewDropPosition pos)
		{
			return null;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003257 File Offset: 0x00001457
		public string GetRedNameString(string name)
		{
			return string.Format("<span foreground='#c32e2e'>{0}</span>", name);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003264 File Offset: 0x00001464
		protected virtual IconInfo GetIcon(object dataObject)
		{
			return new IconInfo();
		}

		// Token: 0x0400002D RID: 45
		private ITreeBuilderContext context;
	}
}
