using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CocoStudio.Basic;
using CocoStudio.Projects.ExtensionModel;
using Mono.Addins;

namespace CocoStudio.Model.DataModel.Upgrade
{
	// Token: 0x02000043 RID: 67
	[Extension(Type = typeof(IFileUpgrader))]
	internal class GameFileUpgrader_231 : GameFileUpgrader
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000789C File Offset: 0x00005A9C
		public override Version Version
		{
			get
			{
				return GameFileUpgrader_231.version;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000282 RID: 642 RVA: 0x000078B4 File Offset: 0x00005AB4
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

		// Token: 0x06000283 RID: 643 RVA: 0x000078EC File Offset: 0x00005AEC
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

		// Token: 0x06000284 RID: 644 RVA: 0x000079D4 File Offset: 0x00005BD4
		protected override bool OnUpgrade(GameFileData projectData)
		{
			TimelineActionData animation = projectData.Animation;
			this.docProjSpeed = animation.Speed;
			this.ConvertProjectNode(projectData.ObjectData);
			return this.isNestedSpeedUpgraded;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00007A0C File Offset: 0x00005C0C
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

		// Token: 0x0400010E RID: 270
		private static readonly Version version = new Version("2.3.1.0");

		// Token: 0x0400010F RID: 271
		private Regex regex;

		// Token: 0x04000110 RID: 272
		private float docProjSpeed = 0f;

		// Token: 0x04000111 RID: 273
		private FileNodeObjectData projNodeData;

		// Token: 0x04000112 RID: 274
		private bool isNestedSpeedUpgraded = false;
	}
}
