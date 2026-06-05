using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using MonoDevelop.Projects.Text;
using MonoDevelop.Projects.Utility;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001C8 RID: 456
	public class MSBuildProject
	{
		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001164 RID: 4452 RVA: 0x0004660D File Offset: 0x0004480D
		internal static XmlNamespaceManager XmlNamespaceManager
		{
			get
			{
				if (MSBuildProject.manager == null)
				{
					MSBuildProject.manager = new XmlNamespaceManager(new NameTable());
					MSBuildProject.manager.AddNamespace("tns", "http://schemas.microsoft.com/developer/msbuild/2003");
				}
				return MSBuildProject.manager;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001165 RID: 4453 RVA: 0x0004663E File Offset: 0x0004483E
		public string FileName
		{
			get
			{
				return this.file;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001166 RID: 4454 RVA: 0x00046646 File Offset: 0x00044846
		public XmlDocument Document
		{
			get
			{
				return this.doc;
			}
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00046650 File Offset: 0x00044850
		public MSBuildProject()
		{
			this.doc = new XmlDocument();
			this.doc.PreserveWhitespace = false;
			this.doc.AppendChild(this.doc.CreateElement(null, "Project", "http://schemas.microsoft.com/developer/msbuild/2003"));
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x000466B4 File Offset: 0x000448B4
		public void Load(string file)
		{
			this.file = file;
			using (FileStream fileStream = File.OpenRead(file))
			{
				byte[] array = new byte[1024];
				int num;
				if ((num = fileStream.Read(array, 0, array.Length)) <= 0)
				{
					return;
				}
				int num2;
				if (ByteOrderMark.TryParse(array, num, out this.bom))
				{
					num2 = this.bom.Length;
				}
				else
				{
					num2 = 0;
				}
				for (;;)
				{
					if (num2 < num)
					{
						if (array[num2] == 13)
						{
							this.newLine = "\r\n";
						}
						else
						{
							if (array[num2] != 10)
							{
								num2++;
								continue;
							}
							this.newLine = "\n";
						}
					}
					if (this.newLine == null)
					{
						if ((num = fileStream.Read(array, 0, array.Length)) <= 0)
						{
							break;
						}
						num2 = 0;
					}
					if (this.newLine != null)
					{
						goto IL_AE;
					}
				}
				this.newLine = "\n";
				IL_AE:
				this.endsWithEmptyLine = (fileStream.Seek(-1L, SeekOrigin.End) > 0L && fileStream.ReadByte() == 10);
			}
			this.doc = new XmlDocument();
			this.doc.PreserveWhitespace = false;
			string xml = File.ReadAllText(file);
			this.doc.LoadXml(xml);
			this.Evaluate();
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x000467E0 File Offset: 0x000449E0
		public void Save(string fileName)
		{
			string content = this.SaveToString();
			TextFile.WriteFile(fileName, content, this.bom, true);
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00046808 File Offset: 0x00044A08
		public string SaveToString()
		{
			MSBuildProject.ProjectWriter projectWriter = new MSBuildProject.ProjectWriter(this.bom);
			projectWriter.NewLine = this.newLine;
			this.doc.Save(projectWriter);
			string text = projectWriter.ToString();
			if (this.endsWithEmptyLine && !text.EndsWith(this.newLine))
			{
				text += this.newLine;
			}
			return text;
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00046864 File Offset: 0x00044A64
		public void Evaluate()
		{
			this.Evaluate(new MSBuildEvaluationContext());
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x00046874 File Offset: 0x00044A74
		public void Evaluate(MSBuildEvaluationContext context)
		{
			context.InitEvaluation(this);
			foreach (MSBuildPropertyGroup msbuildPropertyGroup in this.PropertyGroups)
			{
				msbuildPropertyGroup.Evaluate(context);
			}
			foreach (MSBuildItemGroup msbuildItemGroup in this.ItemGroups)
			{
				msbuildItemGroup.Evaluate(context);
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x0600116D RID: 4461 RVA: 0x00046904 File Offset: 0x00044B04
		// (set) Token: 0x0600116E RID: 4462 RVA: 0x0004691B File Offset: 0x00044B1B
		public string DefaultTargets
		{
			get
			{
				return this.doc.DocumentElement.GetAttribute("DefaultTargets");
			}
			set
			{
				this.doc.DocumentElement.SetAttribute("DefaultTargets", value);
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x0600116F RID: 4463 RVA: 0x00046933 File Offset: 0x00044B33
		// (set) Token: 0x06001170 RID: 4464 RVA: 0x0004694A File Offset: 0x00044B4A
		public string ToolsVersion
		{
			get
			{
				return this.doc.DocumentElement.GetAttribute("ToolsVersion");
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					this.doc.DocumentElement.SetAttribute("ToolsVersion", value);
					return;
				}
				this.doc.DocumentElement.RemoveAttribute("ToolsVersion");
			}
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00046980 File Offset: 0x00044B80
		public MSBuildImport AddNewImport(string name, MSBuildImport beforeImport = null)
		{
			XmlElement xmlElement = this.doc.CreateElement(null, "Import", "http://schemas.microsoft.com/developer/msbuild/2003");
			xmlElement.SetAttribute("Project", name);
			if (beforeImport != null)
			{
				this.doc.DocumentElement.InsertBefore(xmlElement, beforeImport.Element);
			}
			else
			{
				XmlElement xmlElement2 = this.doc.DocumentElement.SelectSingleNode("tns:Import[last()]", MSBuildProject.XmlNamespaceManager) as XmlElement;
				if (xmlElement2 != null)
				{
					this.doc.DocumentElement.InsertAfter(xmlElement, xmlElement2);
				}
				else
				{
					this.doc.DocumentElement.AppendChild(xmlElement);
				}
			}
			return new MSBuildImport(xmlElement);
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00046A20 File Offset: 0x00044C20
		public void RemoveImport(string name)
		{
			XmlElement xmlElement = (XmlElement)this.doc.DocumentElement.SelectSingleNode("tns:Import[@Project='" + name + "']", MSBuildProject.XmlNamespaceManager);
			if (xmlElement != null)
			{
				xmlElement.ParentNode.RemoveChild(xmlElement);
				return;
			}
			Console.WriteLine("ppnf:");
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001173 RID: 4467 RVA: 0x00046C30 File Offset: 0x00044E30
		public IEnumerable<MSBuildImport> Imports
		{
			get
			{
				foreach (object obj in this.doc.DocumentElement.SelectNodes("tns:Import", MSBuildProject.XmlNamespaceManager))
				{
					XmlElement elem = (XmlElement)obj;
					yield return new MSBuildImport(elem);
				}
				yield break;
			}
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00046C50 File Offset: 0x00044E50
		public MSBuildPropertySet GetGlobalPropertyGroup()
		{
			MSBuildPropertyGroupMerged msbuildPropertyGroupMerged = new MSBuildPropertyGroupMerged();
			foreach (MSBuildPropertyGroup msbuildPropertyGroup in this.PropertyGroups)
			{
				if (msbuildPropertyGroup.Condition.Length == 0)
				{
					msbuildPropertyGroupMerged.Add(msbuildPropertyGroup);
				}
			}
			if (msbuildPropertyGroupMerged.GroupCount <= 0)
			{
				return null;
			}
			return msbuildPropertyGroupMerged;
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00046CBC File Offset: 0x00044EBC
		public MSBuildPropertyGroup AddNewPropertyGroup(bool insertAtEnd)
		{
			XmlElement xmlElement = this.doc.CreateElement(null, "PropertyGroup", "http://schemas.microsoft.com/developer/msbuild/2003");
			if (insertAtEnd)
			{
				XmlElement xmlElement2 = this.doc.DocumentElement.SelectSingleNode("tns:PropertyGroup[last()]", MSBuildProject.XmlNamespaceManager) as XmlElement;
				if (xmlElement2 != null)
				{
					this.doc.DocumentElement.InsertAfter(xmlElement, xmlElement2);
				}
			}
			else
			{
				XmlElement xmlElement3 = this.doc.DocumentElement.SelectSingleNode("tns:PropertyGroup", MSBuildProject.XmlNamespaceManager) as XmlElement;
				if (xmlElement3 != null)
				{
					this.doc.DocumentElement.InsertBefore(xmlElement, xmlElement3);
				}
			}
			if (xmlElement.ParentNode == null)
			{
				XmlElement xmlElement4 = this.doc.DocumentElement.SelectSingleNode("tns:ItemGroup", MSBuildProject.XmlNamespaceManager) as XmlElement;
				if (xmlElement4 != null)
				{
					this.doc.DocumentElement.InsertBefore(xmlElement, xmlElement4);
				}
				else
				{
					this.doc.DocumentElement.AppendChild(xmlElement);
				}
			}
			return this.GetGroup(xmlElement);
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00046F6C File Offset: 0x0004516C
		public IEnumerable<MSBuildItem> GetAllItems()
		{
			foreach (object obj in this.doc.DocumentElement.SelectNodes("tns:ItemGroup/*", MSBuildProject.XmlNamespaceManager))
			{
				XmlElement elem = (XmlElement)obj;
				yield return this.GetItem(elem);
			}
			yield break;
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00047178 File Offset: 0x00045378
		public IEnumerable<MSBuildItem> GetAllItems(params string[] names)
		{
			string name = string.Join("|tns:ItemGroup/tns:", names);
			foreach (object obj in this.doc.DocumentElement.SelectNodes("tns:ItemGroup/tns:" + name, MSBuildProject.XmlNamespaceManager))
			{
				XmlElement elem = (XmlElement)obj;
				yield return this.GetItem(elem);
			}
			yield break;
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001178 RID: 4472 RVA: 0x0004735C File Offset: 0x0004555C
		public IEnumerable<MSBuildPropertyGroup> PropertyGroups
		{
			get
			{
				foreach (object obj in this.doc.DocumentElement.SelectNodes("tns:PropertyGroup", MSBuildProject.XmlNamespaceManager))
				{
					XmlElement elem = (XmlElement)obj;
					yield return this.GetGroup(elem);
				}
				yield break;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001179 RID: 4473 RVA: 0x0004753C File Offset: 0x0004573C
		public IEnumerable<MSBuildItemGroup> ItemGroups
		{
			get
			{
				foreach (object obj in this.doc.DocumentElement.SelectNodes("tns:ItemGroup", MSBuildProject.XmlNamespaceManager))
				{
					XmlElement elem = (XmlElement)obj;
					yield return this.GetItemGroup(elem);
				}
				yield break;
			}
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x0004755C File Offset: 0x0004575C
		public MSBuildItemGroup AddNewItemGroup()
		{
			XmlElement xmlElement = this.doc.CreateElement(null, "ItemGroup", "http://schemas.microsoft.com/developer/msbuild/2003");
			this.doc.DocumentElement.AppendChild(xmlElement);
			return this.GetItemGroup(xmlElement);
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x0004759C File Offset: 0x0004579C
		public MSBuildItem AddNewItem(string name, string include)
		{
			MSBuildItemGroup msbuildItemGroup = this.FindBestGroupForItem(name);
			return msbuildItemGroup.AddNewItem(name, include);
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x000475BC File Offset: 0x000457BC
		private MSBuildItemGroup FindBestGroupForItem(string itemName)
		{
			MSBuildItemGroup msbuildItemGroup;
			if (this.bestGroups == null)
			{
				this.bestGroups = new Dictionary<string, MSBuildItemGroup>();
			}
			else if (this.bestGroups.TryGetValue(itemName, out msbuildItemGroup))
			{
				return msbuildItemGroup;
			}
			foreach (MSBuildItemGroup msbuildItemGroup2 in this.ItemGroups)
			{
				foreach (MSBuildItem msbuildItem in msbuildItemGroup2.Items)
				{
					if (msbuildItem.Name == itemName)
					{
						this.bestGroups[itemName] = msbuildItemGroup2;
						return msbuildItemGroup2;
					}
				}
			}
			msbuildItemGroup = this.AddNewItemGroup();
			this.bestGroups[itemName] = msbuildItemGroup;
			return msbuildItemGroup;
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x000476A0 File Offset: 0x000458A0
		public string GetProjectExtensions(string section)
		{
			XmlElement xmlElement = this.doc.DocumentElement.SelectSingleNode("tns:ProjectExtensions/tns:" + section, MSBuildProject.XmlNamespaceManager) as XmlElement;
			if (xmlElement != null)
			{
				return xmlElement.InnerXml;
			}
			return string.Empty;
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x000476E4 File Offset: 0x000458E4
		public void SetProjectExtensions(string section, string value)
		{
			XmlElement xmlElement = this.doc.DocumentElement["ProjectExtensions", "http://schemas.microsoft.com/developer/msbuild/2003"];
			if (xmlElement == null)
			{
				xmlElement = this.doc.CreateElement(null, "ProjectExtensions", "http://schemas.microsoft.com/developer/msbuild/2003");
				this.doc.DocumentElement.AppendChild(xmlElement);
			}
			XmlElement xmlElement2 = xmlElement[section];
			if (xmlElement2 == null)
			{
				xmlElement2 = this.doc.CreateElement(null, section, "http://schemas.microsoft.com/developer/msbuild/2003");
				xmlElement.AppendChild(xmlElement2);
			}
			xmlElement2.InnerXml = value;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x00047768 File Offset: 0x00045968
		public void RemoveProjectExtensions(string section)
		{
			XmlElement xmlElement = this.doc.DocumentElement.SelectSingleNode("tns:ProjectExtensions/tns:" + section, MSBuildProject.XmlNamespaceManager) as XmlElement;
			if (xmlElement != null)
			{
				XmlElement xmlElement2 = (XmlElement)xmlElement.ParentNode;
				xmlElement2.RemoveChild(xmlElement);
				if (!xmlElement2.HasChildNodes)
				{
					xmlElement2.ParentNode.RemoveChild(xmlElement2);
				}
			}
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x000477C8 File Offset: 0x000459C8
		public void RemoveItem(MSBuildItem item)
		{
			this.elemCache.Remove(item.Element);
			XmlElement xmlElement = (XmlElement)item.Element.ParentNode;
			item.Element.ParentNode.RemoveChild(item.Element);
			if (xmlElement.ChildNodes.Count == 0)
			{
				this.elemCache.Remove(xmlElement);
				xmlElement.ParentNode.RemoveChild(xmlElement);
				this.bestGroups = null;
			}
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x00047840 File Offset: 0x00045A40
		internal MSBuildItem GetItem(XmlElement elem)
		{
			MSBuildObject msbuildObject;
			if (this.elemCache.TryGetValue(elem, out msbuildObject))
			{
				return (MSBuildItem)msbuildObject;
			}
			MSBuildItem msbuildItem = new MSBuildItem(elem);
			this.elemCache[elem] = msbuildItem;
			return msbuildItem;
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x0004787C File Offset: 0x00045A7C
		private MSBuildPropertyGroup GetGroup(XmlElement elem)
		{
			MSBuildObject msbuildObject;
			if (this.elemCache.TryGetValue(elem, out msbuildObject))
			{
				return (MSBuildPropertyGroup)msbuildObject;
			}
			MSBuildPropertyGroup msbuildPropertyGroup = new MSBuildPropertyGroup(this, elem);
			this.elemCache[elem] = msbuildPropertyGroup;
			return msbuildPropertyGroup;
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x000478B8 File Offset: 0x00045AB8
		private MSBuildItemGroup GetItemGroup(XmlElement elem)
		{
			MSBuildObject msbuildObject;
			if (this.elemCache.TryGetValue(elem, out msbuildObject))
			{
				return (MSBuildItemGroup)msbuildObject;
			}
			MSBuildItemGroup msbuildItemGroup = new MSBuildItemGroup(this, elem);
			this.elemCache[elem] = msbuildItemGroup;
			return msbuildItemGroup;
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x000478F2 File Offset: 0x00045AF2
		public void RemoveGroup(MSBuildPropertyGroup grp)
		{
			this.elemCache.Remove(grp.Element);
			grp.Element.ParentNode.RemoveChild(grp.Element);
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001185 RID: 4485 RVA: 0x00047ADC File Offset: 0x00045CDC
		public IEnumerable<MSBuildTarget> Targets
		{
			get
			{
				foreach (object obj in this.doc.DocumentElement.SelectNodes("tns:Target", MSBuildProject.XmlNamespaceManager))
				{
					XmlElement elem = (XmlElement)obj;
					yield return new MSBuildTarget(elem);
				}
				yield break;
			}
		}

		// Token: 0x0400050B RID: 1291
		public const string Schema = "http://schemas.microsoft.com/developer/msbuild/2003";

		// Token: 0x0400050C RID: 1292
		private XmlDocument doc;

		// Token: 0x0400050D RID: 1293
		private string file;

		// Token: 0x0400050E RID: 1294
		private Dictionary<XmlElement, MSBuildObject> elemCache = new Dictionary<XmlElement, MSBuildObject>();

		// Token: 0x0400050F RID: 1295
		private Dictionary<string, MSBuildItemGroup> bestGroups;

		// Token: 0x04000510 RID: 1296
		private static XmlNamespaceManager manager;

		// Token: 0x04000511 RID: 1297
		private bool endsWithEmptyLine;

		// Token: 0x04000512 RID: 1298
		private string newLine = Environment.NewLine;

		// Token: 0x04000513 RID: 1299
		private ByteOrderMark bom;

		// Token: 0x020001C9 RID: 457
		private class ProjectWriter : StringWriter
		{
			// Token: 0x06001186 RID: 4486 RVA: 0x00047AF9 File Offset: 0x00045CF9
			public ProjectWriter(ByteOrderMark bom)
			{
				this.encoding = ((bom != null) ? Encoding.GetEncoding(bom.Name) : null);
				this.ByteOrderMark = bom;
			}

			// Token: 0x170003BC RID: 956
			// (get) Token: 0x06001187 RID: 4487 RVA: 0x00047B1F File Offset: 0x00045D1F
			// (set) Token: 0x06001188 RID: 4488 RVA: 0x00047B27 File Offset: 0x00045D27
			public ByteOrderMark ByteOrderMark { get; private set; }

			// Token: 0x170003BD RID: 957
			// (get) Token: 0x06001189 RID: 4489 RVA: 0x00047B30 File Offset: 0x00045D30
			public override Encoding Encoding
			{
				get
				{
					return this.encoding ?? Encoding.UTF8;
				}
			}

			// Token: 0x04000514 RID: 1300
			private Encoding encoding;
		}
	}
}
