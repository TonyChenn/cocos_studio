using System;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.ResourcePanel
{
	[TypeExtensionPoint(ExtensionAttributeType = typeof(ResourcePanelExtensionAttribute), NodeType = typeof(ResourcePanelExtensionNode))]
	public abstract class NodeBuilder : IDisposable
	{
		public abstract Type NodeDataType { get; }

		internal NodeBuilder()
		{
		}

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

		protected virtual void Initialize()
		{
		}

		protected ITreeBuilderContext Context
		{
			get
			{
				return this.context;
			}
		}

		public void BuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			this.OnBuildNodeBefore(treeBuilder, dataObject, nodeInfo);
			this.OnBuildNode(treeBuilder, dataObject, nodeInfo);
			this.OnBuildNodeAfter(treeBuilder, dataObject, nodeInfo);
		}

		protected virtual void OnBuildNodeBefore(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
		}

		protected virtual void OnBuildNodeAfter(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
		}

		protected virtual void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			nodeInfo.IconInfo = this.GetIcon(dataObject);
		}

		public virtual void BuildChildNodes(ITreeBuild treeBuilder, object dataObject)
		{
		}

		public virtual void Dispose()
		{
		}

		public virtual void AddChild(ITreeBuild treeBuilder, object dateObject)
		{
		}

		public virtual void OnNodeAdded(object dateObject)
		{
		}

		public virtual void OnNodeRemoved(object dateObject)
		{
		}

		public virtual bool CanRename()
		{
			return true;
		}

		public bool CanDrag(object dataObject)
		{
			return this.OnCanDrag(dataObject);
		}

		protected virtual bool OnCanDrag(object dataObject)
		{
			return true;
		}

		public void Rename(ITreeBuild treeBuilder, object dataObject, string newName)
		{
			this.OnRename(treeBuilder, dataObject, newName);
			Services.Workspace.SaveCurrentSolution();
			Services.Workbench.SaveAll();
		}

		protected virtual void OnRename(ITreeBuild treeBuilder, object dataObject, string newName)
		{
		}

		internal virtual bool CanDelete()
		{
			return true;
		}

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

		protected virtual void OnDelete(ITreeBuild treeBuilder, object dataObject, IProgressMonitor monitor)
		{
		}

		protected virtual void OnRemove(ITreeBuild treeBuilder, object dataObject, IProgressMonitor monitor)
		{
		}

		public virtual ResourceFolder GetTargetFolder(object dataObject)
		{
			return null;
		}

		public virtual string CanMove(object moveSource, object moveTarget, TreeViewDropPosition pos)
		{
			return null;
		}

		public string GetRedNameString(string name)
		{
			return string.Format("<span foreground='#c32e2e'>{0}</span>", name);
		}

		protected virtual IconInfo GetIcon(object dataObject)
		{
			return new IconInfo();
		}

		private ITreeBuilderContext context;
	}
}
