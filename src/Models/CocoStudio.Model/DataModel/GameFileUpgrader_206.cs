using System;
using CocoStudio.Projects.ExtensionModel;
using Mono.Addins;

namespace CocoStudio.Model.DataModel
{
	[Extension(Type = typeof(IFileUpgrader))]
	internal class GameFileUpgrader_206 : GameFileUpgrader
	{
		public override Version Version
		{
			get
			{
				return GameFileUpgrader_206.version;
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
				this.ConvertObject(objectData);
				result = true;
			}
			return result;
		}

		protected void ConvertObject(AbstractNodeObjectData nData)
		{
			NodeObjectData nodeObjectData = nData as NodeObjectData;
			if (nodeObjectData != null && nodeObjectData.PrePositionEnabled)
			{
				nodeObjectData.PositionPercentXEnabled = (nodeObjectData.PositionPercentYEnabled = true);
			}
			if (nodeObjectData != null && nodeObjectData.PreSizeEnable)
			{
				nodeObjectData.PercentWidthEnable = (nodeObjectData.PercentHeightEnable = true);
			}
			if (nData.Children != null)
			{
				foreach (AbstractNodeObjectData nData2 in nData.Children)
				{
					this.ConvertObject(nData2);
				}
			}
		}

		private static readonly Version version = new Version("2.0.6.0");
	}
}
