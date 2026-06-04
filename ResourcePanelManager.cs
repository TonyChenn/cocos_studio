using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Basic;
using Mono.Addins;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000008 RID: 8
	internal class ResourcePanelManager
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002DC4 File Offset: 0x00000FC4
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002DCB File Offset: 0x00000FCB
		public static ResourcePanelManager Instance { get; private set; } = new ResourcePanelManager();

		// Token: 0x06000036 RID: 54 RVA: 0x00002DDF File Offset: 0x00000FDF
		private ResourcePanelManager()
		{
			this.Initialize();
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002DED File Offset: 0x00000FED
		public List<NodeBuilder> NodeBuildes
		{
			get
			{
				return this.resourcePanelBuilder.Values.ToList<NodeBuilder>();
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002E00 File Offset: 0x00001000
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

		// Token: 0x06000039 RID: 57 RVA: 0x00002EA0 File Offset: 0x000010A0
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

		// Token: 0x0600003A RID: 58 RVA: 0x00002EE3 File Offset: 0x000010E3
		private void OnResourcePanelExtensionChange(object sender, ExtensionNodeEventArgs args)
		{
		}

		// Token: 0x04000024 RID: 36
		private Dictionary<Type, NodeBuilder> resourcePanelBuilder;
	}
}
