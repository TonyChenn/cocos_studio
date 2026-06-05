using System;
using System.Xml;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001D0 RID: 464
	public class MSBuildItem : MSBuildObject
	{
		// Token: 0x060011BF RID: 4543 RVA: 0x0004863C File Offset: 0x0004683C
		public MSBuildItem(XmlElement elem) : base(elem)
		{
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060011C0 RID: 4544 RVA: 0x00048645 File Offset: 0x00046845
		// (set) Token: 0x060011C1 RID: 4545 RVA: 0x00048657 File Offset: 0x00046857
		public string Include
		{
			get
			{
				return base.EvaluatedElement.GetAttribute("Include");
			}
			set
			{
				base.Element.SetAttribute("Include", value);
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060011C2 RID: 4546 RVA: 0x0004866A File Offset: 0x0004686A
		// (set) Token: 0x060011C3 RID: 4547 RVA: 0x0004867C File Offset: 0x0004687C
		public string UnevaluatedInclude
		{
			get
			{
				return base.Element.GetAttribute("Include");
			}
			set
			{
				base.Element.SetAttribute("Include", value);
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060011C4 RID: 4548 RVA: 0x0004868F File Offset: 0x0004688F
		public string Name
		{
			get
			{
				return base.Element.Name;
			}
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0004869C File Offset: 0x0004689C
		public bool HasMetadata(string name)
		{
			return base.EvaluatedElement[name, "http://schemas.microsoft.com/developer/msbuild/2003"] != null;
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x000486B5 File Offset: 0x000468B5
		public void SetMetadata(string name, bool value)
		{
			this.SetMetadata(name, value ? "True" : "False", false);
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x000486D0 File Offset: 0x000468D0
		public void SetMetadata(string name, string value, bool isXml = false)
		{
			if (this.GetMetadata(name, isXml) == value)
			{
				return;
			}
			XmlElement xmlElement = base.Element[name, "http://schemas.microsoft.com/developer/msbuild/2003"];
			if (xmlElement == null)
			{
				xmlElement = base.AddChildElement(name);
				base.Element.AppendChild(xmlElement);
			}
			if (isXml)
			{
				xmlElement.InnerXml = value;
				return;
			}
			xmlElement.InnerText = value;
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x0004872C File Offset: 0x0004692C
		public void UnsetMetadata(string name)
		{
			XmlElement xmlElement = base.Element[name, "http://schemas.microsoft.com/developer/msbuild/2003"];
			if (xmlElement != null)
			{
				base.Element.RemoveChild(xmlElement);
				if (!base.Element.HasChildNodes)
				{
					base.Element.IsEmpty = true;
				}
			}
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00048774 File Offset: 0x00046974
		public string GetMetadata(string name, bool isXml = false)
		{
			XmlElement xmlElement = base.EvaluatedElement[name, "http://schemas.microsoft.com/developer/msbuild/2003"];
			if (xmlElement == null)
			{
				return null;
			}
			if (!isXml)
			{
				return xmlElement.InnerText;
			}
			return xmlElement.InnerXml;
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x000487A8 File Offset: 0x000469A8
		public bool? GetBoolMetadata(string name)
		{
			string metadata = this.GetMetadata(name, false);
			if (string.Equals(metadata, "False", StringComparison.OrdinalIgnoreCase))
			{
				return new bool?(false);
			}
			if (string.Equals(metadata, "True", StringComparison.OrdinalIgnoreCase))
			{
				return new bool?(true);
			}
			return null;
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x000487F1 File Offset: 0x000469F1
		public bool GetMetadataIsFalse(string name)
		{
			return string.Compare(this.GetMetadata(name, false), "False", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0004880C File Offset: 0x00046A0C
		public void MergeFrom(MSBuildItem other)
		{
			foreach (object obj in base.Element.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode is XmlElement)
				{
					this.SetMetadata(xmlNode.LocalName, xmlNode.InnerXml, true);
				}
			}
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00048880 File Offset: 0x00046A80
		internal override void Evaluate(MSBuildEvaluationContext context)
		{
			XmlElement evaluatedElement;
			if (context.Evaluate(base.Element, out evaluatedElement))
			{
				base.EvaluatedElement = evaluatedElement;
				return;
			}
			base.EvaluatedElement = null;
		}
	}
}
