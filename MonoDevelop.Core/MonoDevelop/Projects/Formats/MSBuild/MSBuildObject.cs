using System;
using System.Xml;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001CA RID: 458
	public class MSBuildObject
	{
		// Token: 0x0600118A RID: 4490 RVA: 0x00047B41 File Offset: 0x00045D41
		public MSBuildObject(XmlElement elem)
		{
			this.elem = elem;
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x0600118B RID: 4491 RVA: 0x00047B50 File Offset: 0x00045D50
		public XmlElement Element
		{
			get
			{
				return this.elem;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x0600118C RID: 4492 RVA: 0x00047B58 File Offset: 0x00045D58
		// (set) Token: 0x0600118D RID: 4493 RVA: 0x00047B6A File Offset: 0x00045D6A
		public XmlElement EvaluatedElement
		{
			get
			{
				return this.evaluatedElem ?? this.elem;
			}
			protected set
			{
				this.evaluatedElem = value;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x0600118E RID: 4494 RVA: 0x00047B73 File Offset: 0x00045D73
		public bool IsEvaluated
		{
			get
			{
				return this.evaluatedElem != null;
			}
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00047B84 File Offset: 0x00045D84
		protected XmlElement AddChildElement(string name)
		{
			XmlElement xmlElement = this.elem.OwnerDocument.CreateElement(null, name, "http://schemas.microsoft.com/developer/msbuild/2003");
			this.elem.AppendChild(xmlElement);
			return xmlElement;
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06001190 RID: 4496 RVA: 0x00047BB7 File Offset: 0x00045DB7
		// (set) Token: 0x06001191 RID: 4497 RVA: 0x00047BC9 File Offset: 0x00045DC9
		public string Label
		{
			get
			{
				return this.EvaluatedElement.GetAttribute("Label");
			}
			set
			{
				this.Element.SetAttribute("Label", value);
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06001192 RID: 4498 RVA: 0x00047BDC File Offset: 0x00045DDC
		// (set) Token: 0x06001193 RID: 4499 RVA: 0x00047BEE File Offset: 0x00045DEE
		public string Condition
		{
			get
			{
				return this.Element.GetAttribute("Condition");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Element.RemoveAttribute("Condition");
					return;
				}
				this.Element.SetAttribute("Condition", value);
			}
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00047C1A File Offset: 0x00045E1A
		internal virtual void Evaluate(MSBuildEvaluationContext context)
		{
		}

		// Token: 0x04000516 RID: 1302
		private XmlElement elem;

		// Token: 0x04000517 RID: 1303
		private XmlElement evaluatedElem;
	}
}
