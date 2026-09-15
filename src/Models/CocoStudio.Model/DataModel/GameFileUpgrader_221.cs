using System;
using CocoStudio.Projects.ExtensionModel;
using Mono.Addins;

namespace CocoStudio.Model.DataModel
{
	[Extension(Type = typeof(IFileUpgrader))]
	internal class GameFileUpgrader_221 : GameFileUpgrader
	{
		public override Version Version
		{
			get
			{
				return GameFileUpgrader_221.version;
			}
		}

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

		private static readonly Version version = new Version("2.2.1.0");
	}
}
