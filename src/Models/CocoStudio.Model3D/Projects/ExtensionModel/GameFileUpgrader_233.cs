using System;
using System.IO;
using System.Xml.Linq;
using CocoStudio.Basic;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Visiter;
using Mono.Addins;

namespace CocoStudio.Projects.ExtensionModel
{
	// Token: 0x0200001A RID: 26
	[Extension(Type = typeof(IFileUpgrader))]
	internal class GameFileUpgrader_233 : GameFileUpgrader
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x000044FC File Offset: 0x000026FC
		public override Version Version
		{
			get
			{
				return GameFileUpgrader_233.version;
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004504 File Offset: 0x00002704
		public override bool Upgrade(string filePath)
		{
			bool result;
			try
			{
				result = this.OnUpgrade(filePath);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("GameFileUpgrader_233.Upgrade failed.", exception);
				result = false;
			}
			return result;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004544 File Offset: 0x00002744
		private bool OnUpgrade(string filePath)
		{
			bool flag = false;
			XElement xelement = XElement.Load(filePath);
			if (xelement.Name == GameFileUpgrader_233.rootNodeName)
			{
				flag = true;
				xelement.Name = typeof(GameFile).Name;
			}
			string text = xelement.ToString(SaveOptions.DisableFormatting);
			XElement xelement2 = xelement.Element("PropertyGroup");
			string value = xelement2.Attribute("Type").Value;
			if (value == NodeType.Scene3D.ToString())
			{
				this.is3DGameFile = true;
				flag = true;
				string oldValue = "UserCameraFlagMode=\"DEFAULT\"";
				text = text.Replace(oldValue, "");
			}
			if (flag)
			{
				File.WriteAllText(filePath, text);
			}
			return flag;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000045FA File Offset: 0x000027FA
		protected override bool OnUpgrade(GameFileData projectData)
		{
			if (this.is3DGameFile)
			{
				this.Convert3DNode(projectData.ObjectData);
				this.is3DGameFile = false;
			}
			else
			{
				this.ConvertProjectNode(projectData.ObjectData);
			}
			return true;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004628 File Offset: 0x00002828
		private void ConvertProjectNode(AbstractNodeObjectData rootnode)
		{
			FileNodeObjectData fileNodeObjectData = rootnode as FileNodeObjectData;
			if (fileNodeObjectData != null)
			{
				CocosItem cocosItem = fileNodeObjectData.FileData.CreateViewModel() as CocosItem;
				if (cocosItem != null && cocosItem.IsSkeletonFile())
				{
					fileNodeObjectData.PreSize = SizeF.Zero;
					fileNodeObjectData.Size = SizeF.Zero;
				}
			}
			if (rootnode.Children != null)
			{
				foreach (AbstractNodeObjectData rootnode2 in rootnode.Children)
				{
					this.ConvertProjectNode(rootnode2);
				}
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000046C0 File Offset: 0x000028C0
		private void Convert3DNode(AbstractNodeObjectData rootnode)
		{
			Node3DObjectData node3DObjectData = rootnode as Node3DObjectData;
			if (node3DObjectData != null && (node3DObjectData.CameraFlagMode & 1) != 0)
			{
				node3DObjectData.CameraFlagMode &= -2;
			}
			if (rootnode.Children != null)
			{
				foreach (AbstractNodeObjectData rootnode2 in rootnode.Children)
				{
					this.Convert3DNode(rootnode2);
				}
			}
		}

		// Token: 0x04000060 RID: 96
		private static readonly Version version = new Version("2.3.3");

		// Token: 0x04000061 RID: 97
		private static readonly string rootNodeName = "GameProjectFile";

		// Token: 0x04000062 RID: 98
		private bool is3DGameFile;
	}
}
