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
	[Extension(Type = typeof(IFileUpgrader))]
	internal class GameFileUpgrader_233 : GameFileUpgrader
	{
		public override Version Version
		{
			get
			{
				return GameFileUpgrader_233.version;
			}
		}

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

		private static readonly Version version = new Version("2.3.3");

		private static readonly string rootNodeName = "GameProjectFile";

		private bool is3DGameFile;
	}
}
