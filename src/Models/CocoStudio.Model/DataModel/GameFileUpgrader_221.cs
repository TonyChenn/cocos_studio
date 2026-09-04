using System;
using CocoStudio.Projects.ExtensionModel;
using Mono.Addins;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200000E RID: 14
	[Extension(Type = typeof(IFileUpgrader))]
	internal class GameFileUpgrader_221 : GameFileUpgrader
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002BB8 File Offset: 0x00000DB8
		public override Version Version
		{
			get
			{
				return GameFileUpgrader_221.version;
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002BD0 File Offset: 0x00000DD0
		protected override bool OnUpgrade(GameFileData projectData)
		{
			AbstractNodeObjectData objectData = projectData.ObjectData;
			bool result;
			if (objectData == null)
			{
				result = false;
			}
			else
			{
				projectData.ObjectData = this.ConvertRootObject(objectData);
				result = true;
			}
			return result;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002C08 File Offset: 0x00000E08
		protected AbstractNodeObjectData ConvertRootObject(AbstractNodeObjectData nodeData)
		{
			AbstractNodeObjectData result;
			if (nodeData is SingleNodeObjectData)
			{
				result = new GameNodeObjectData(nodeData);
			}
			else if (nodeData is LayerObjectData)
			{
				result = new GameLayerObjectData(nodeData);
			}
			else if (nodeData is PanelObjectData)
			{
				result = new GameLayerObjectData(nodeData);
			}
			else
			{
				result = new GameNodeObjectData(nodeData);
			}
			return result;
		}

		// Token: 0x04000016 RID: 22
		private static readonly Version version = new Version("2.2.1.0");
	}
}
