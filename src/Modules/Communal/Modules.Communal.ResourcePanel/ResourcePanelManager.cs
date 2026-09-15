using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Basic;
using Mono.Addins;

namespace Modules.Communal.ResourcePanel
{
	internal class ResourcePanelManager
	{
		public static ResourcePanelManager Instance { get; private set; } = new ResourcePanelManager();

		private ResourcePanelManager()
		{
			this.Initialize();
		}

		public List<NodeBuilder> NodeBuildes
		{
			get
			{
				return this.resourcePanelBuilder.Values.ToList<NodeBuilder>();
			}
		}

		private void Initialize()
		{
			try
			{
				this.resourcePanelBuilder = new Dictionary<Type, NodeBuilder>();
				ExtensionNodeList<ResourcePanelExtensionNode> extensionNodes = AddinManager.GetExtensionNodes<ResourcePanelExtensionNode>(typeof(NodeBuilder));
				foreach (ResourcePanelExtensionNode extensionNode in extensionNodes)
				{
					this.RegisteDataModel(extensionNode);
				}
				AddinManager.AddExtensionNodeHandler(typeof(NodeBuilder), new ExtensionNodeEventHandler(this.OnResourcePanelExtensionChange));
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Debug("Load resource panel addins failed", exception);
			}
		}

		private void RegisteDataModel(ResourcePanelExtensionNode extensionNode)
		{
			if (extensionNode == null)
			{
				return;
			}
			if (this.resourcePanelBuilder.ContainsKey(extensionNode.Type))
			{
				return;
			}
			NodeBuilder value = extensionNode.CreateInstance() as NodeBuilder;
			this.resourcePanelBuilder.Add(extensionNode.Type, value);
		}

		private void OnResourcePanelExtensionChange(object sender, ExtensionNodeEventArgs args)
		{
		}

		private Dictionary<Type, NodeBuilder> resourcePanelBuilder;
	}
}
