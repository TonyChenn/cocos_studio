using System;
using System.IO;
using System.Xml.Linq;
using CocoStudio.Basic;
using CocoStudio.Projects;
using CocoStudio.Projects.ExtensionModel;
using Mono.Addins;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000019 RID: 25
	[Extension(Type = typeof(IFileUpgrader))]
	internal class CocosFileUpgrader_232 : GameFileUpgrader
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000041BA File Offset: 0x000023BA
		public override Version Version
		{
			get
			{
				return CocosFileUpgrader_232.version;
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000041C4 File Offset: 0x000023C4
		public override bool Upgrade(string filePath)
		{
			try
			{
				XElement xelement = XElement.Load(filePath);
				XElement xelement2 = xelement.Element("PropertyGroup");
				string value = xelement2.Attribute("Type").Value;
				if (value == NodeType.Scene3D.ToString())
				{
					this.UpgradeSceneCamera(filePath);
					this.is3DGameFile = true;
				}
				return true;
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Upgrade CocosFile failed. version is " + CocosFileUpgrader_232.version.ToString(), exception);
			}
			return false;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000425C File Offset: 0x0000245C
		protected override bool OnUpgrade(GameFileData gameFileData)
		{
			AbstractNodeObjectData objectData = gameFileData.ObjectData;
			if (objectData == null)
			{
				return false;
			}
			gameFileData.ObjectData = this.ConvertRootObject(objectData);
			return true;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004283 File Offset: 0x00002483
		protected AbstractNodeObjectData ConvertRootObject(AbstractNodeObjectData nodeData)
		{
			if (this.is3DGameFile)
			{
				this.is3DGameFile = false;
				nodeData = this.ConvertRootObject3D(nodeData);
			}
			return nodeData;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000042A0 File Offset: 0x000024A0
		private AbstractNodeObjectData ConvertRootObject3D(AbstractNodeObjectData nodeData)
		{
			AbstractNodeObjectData result = nodeData;
			if (nodeData is GameNodeObjectData)
			{
				result = new GameNode3DObjectData(nodeData);
			}
			return result;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000042C0 File Offset: 0x000024C0
		private void ConvertRootObject2D(AbstractNodeObjectData abstractData)
		{
			NodeObjectData nodeObjectData = abstractData as NodeObjectData;
			if (nodeObjectData != null)
			{
				if (nodeObjectData.PositionPercentXEnabled)
				{
					nodeObjectData.HorizontalEdge = HorizontalBerthEdge.BothEdge;
				}
				if (nodeObjectData.PositionPercentYEnabled)
				{
					nodeObjectData.VerticalEdge = VerticalBerthEdge.BothEdge;
				}
			}
			if (abstractData.Children != null)
			{
				foreach (AbstractNodeObjectData abstractData2 in abstractData.Children)
				{
					this.ConvertRootObject2D(abstractData2);
				}
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004344 File Offset: 0x00002544
		private void UpgradeSceneCamera(string filePath)
		{
			using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite))
			{
				XElement xelement = XElement.Load(fileStream);
				XElement xelement2 = xelement.Element("Content").Element("Content").Element("SceneCamera");
				if (!xelement2.IsEmpty)
				{
					XElement xelement3 = xelement2.Element("Position");
					float x = float.Parse(xelement3.Attribute("X").Value);
					float y = float.Parse(xelement3.Attribute("Y").Value);
					float z = float.Parse(xelement3.Attribute("Z").Value);
					Point3F position = new Point3F(x, y, z);
					XElement xelement4 = xelement2.Element("Rotation");
					x = float.Parse(xelement4.Attribute("X").Value);
					y = float.Parse(xelement4.Attribute("Y").Value);
					z = 0f;
					Point3F rotation = new Point3F(x, y, z);
					CameraData data = new CameraData(position, rotation);
					this.UpgradeUdfFile(filePath, data);
				}
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000044AC File Offset: 0x000026AC
		private void UpgradeUdfFile(string filePath, CameraData data)
		{
			string filePath2 = filePath + ".udf";
			new UserData(filePath2)
			{
				Properties = 
				{
					{
						"CameraData",
						data
					}
				}
			}.Save();
		}

		// Token: 0x0400005E RID: 94
		private bool is3DGameFile;

		// Token: 0x0400005F RID: 95
		private static readonly Version version = new Version("2.3.2.0");
	}
}
