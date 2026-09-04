using System;
using CocoStudio.Projects.ExtensionModel;
using Mono.Addins;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200000C RID: 12
	[Extension(Type = typeof(IFileUpgrader))]
	internal class GameFileUpgrader_206 : GameFileUpgrader
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000046 RID: 70 RVA: 0x000027A0 File Offset: 0x000009A0
		public override Version Version
		{
			get
			{
				return GameFileUpgrader_206.version;
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000027B8 File Offset: 0x000009B8
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

		// Token: 0x06000048 RID: 72 RVA: 0x000027EC File Offset: 0x000009EC
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

		// Token: 0x04000014 RID: 20
		private static readonly Version version = new Version("2.0.6.0");
	}
}
