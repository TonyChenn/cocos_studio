using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CocoStudio.Basic;
using CocoStudio.Projects.ExtensionModel;
using Mono.Addins;

namespace CocoStudio.Model.DataModel.Upgrade
{
	[Extension(Type = typeof(IFileUpgrader))]
	internal class GameFileUpgrader_231 : GameFileUpgrader
	{
		public override Version Version
		{
			get
			{
				return GameFileUpgrader_231.version;
			}
		}

		public Regex Regex
		{
			get
			{
				if (this.regex == null)
				{
					this.regex = new Regex(" Value(X|Y|Z)=");
				}
				return this.regex;
			}
		}

		public override bool Upgrade(string filePath)
		{
			try
			{
				XElement xelement = XElement.Load(filePath);
				XElement xelement2 = xelement.Element("PropertyGroup");
				string value = xelement2.Attribute("Type").Value;
				if (value != NodeType.Scene3D.ToString())
				{
					return false;
				}
				string text = File.ReadAllText(filePath);
				text = this.Regex.Replace(text, delegate(Match match)
				{
					string result;
					if (match.Value == " ValueX=")
					{
						result = " X=";
					}
					else if (match.Value == " ValueY=")
					{
						result = " Y=";
					}
					else if (match.Value == " ValueZ=")
					{
						result = " Z=";
					}
					else
					{
						result = match.Value;
					}
					return result;
				});
				File.WriteAllText(filePath, text);
				this.regex = null;
				return true;
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Upgrade project failed. version is " + GameFileUpgrader_231.version.ToString(), exception);
			}
			return false;
		}

		protected override bool OnUpgrade(GameFileData projectData)
		{
			TimelineActionData animation = projectData.Animation;
			this.docProjSpeed = animation.Speed;
			this.ConvertProjectNode(projectData.ObjectData);
			return this.isNestedSpeedUpgraded;
		}

		private void ConvertProjectNode(AbstractNodeObjectData rootnode)
		{
			this.projNodeData = (rootnode as FileNodeObjectData);
			if (this.projNodeData != null && this.projNodeData.InnerActionSpeed == 0f)
			{
				this.projNodeData.InnerActionSpeed = this.docProjSpeed;
				this.isNestedSpeedUpgraded = true;
			}
			if (rootnode.Children != null)
			{
				foreach (AbstractNodeObjectData rootnode2 in rootnode.Children)
				{
					this.ConvertProjectNode(rootnode2);
				}
			}
		}

		private static readonly Version version = new Version("2.3.1.0");

		private Regex regex;

		private float docProjSpeed = 0f;

		private FileNodeObjectData projNodeData;

		private bool isNestedSpeedUpgraded = false;
	}
}
