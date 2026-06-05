using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Build.BuildEngine;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001CF RID: 463
	public class MSBuildPropertyGroup : MSBuildObject, MSBuildPropertySet
	{
		// Token: 0x060011B3 RID: 4531 RVA: 0x000481A0 File Offset: 0x000463A0
		public MSBuildPropertyGroup(MSBuildProject parent, XmlElement elem) : base(elem)
		{
			this.parent = parent;
			foreach (XmlElement xmlElement in base.Element.ChildNodes.OfType<XmlElement>())
			{
				MSBuildProperty msbuildProperty;
				if (this.properties.TryGetValue(xmlElement.Name, out msbuildProperty))
				{
					msbuildProperty.Overwritten = true;
				}
				MSBuildProperty msbuildProperty2 = new MSBuildProperty(xmlElement);
				this.propertyList.Add(msbuildProperty2);
				this.properties[xmlElement.Name] = msbuildProperty2;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x00048258 File Offset: 0x00046458
		public MSBuildProject Parent
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00048260 File Offset: 0x00046460
		public MSBuildProperty GetProperty(string name)
		{
			MSBuildProperty result;
			this.properties.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x060011B6 RID: 4534 RVA: 0x00048288 File Offset: 0x00046488
		public IEnumerable<MSBuildProperty> Properties
		{
			get
			{
				return from p in this.propertyList
				where !p.Overwritten
				select p;
			}
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x000482B4 File Offset: 0x000464B4
		public MSBuildProperty SetPropertyValue(string name, string value, bool preserveExistingCase, bool isXml = false)
		{
			MSBuildProperty msbuildProperty = this.GetProperty(name);
			if (msbuildProperty == null)
			{
				XmlElement elem = base.AddChildElement(name);
				msbuildProperty = new MSBuildProperty(elem);
				this.properties[name] = msbuildProperty;
				this.propertyList.Add(msbuildProperty);
				msbuildProperty.SetValue(value, isXml);
			}
			else if (!preserveExistingCase || !string.Equals(value, msbuildProperty.GetValue(isXml), StringComparison.OrdinalIgnoreCase))
			{
				msbuildProperty.SetValue(value, isXml);
			}
			return msbuildProperty;
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00048320 File Offset: 0x00046520
		public string GetPropertyValue(string name, bool isXml = false)
		{
			MSBuildProperty property = this.GetProperty(name);
			if (property == null)
			{
				return null;
			}
			return property.GetValue(isXml);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00048344 File Offset: 0x00046544
		public bool RemoveProperty(string name)
		{
			MSBuildProperty property = this.GetProperty(name);
			if (property != null)
			{
				this.properties.Remove(name);
				this.propertyList.Remove(property);
				base.Element.RemoveChild(property.Element);
				return true;
			}
			return false;
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x0004838C File Offset: 0x0004658C
		public void RemoveAllProperties()
		{
			List<XmlNode> list = new List<XmlNode>();
			foreach (object obj in base.Element.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode is XmlElement)
				{
					list.Add(xmlNode);
				}
			}
			foreach (XmlNode oldChild in list)
			{
				base.Element.RemoveChild(oldChild);
			}
			this.properties.Clear();
			this.propertyList.Clear();
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x00048458 File Offset: 0x00046658
		public void UnMerge(MSBuildPropertySet baseGrp, ISet<string> propsToExclude)
		{
			foreach (MSBuildProperty msbuildProperty in baseGrp.Properties)
			{
				if (propsToExclude == null || !propsToExclude.Contains(msbuildProperty.Name))
				{
					MSBuildProperty property = this.GetProperty(msbuildProperty.Name);
					if (property != null && msbuildProperty.GetValue(true).Equals(property.GetValue(true), StringComparison.OrdinalIgnoreCase))
					{
						this.RemoveProperty(msbuildProperty.Name);
					}
				}
			}
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x000484E4 File Offset: 0x000466E4
		internal override void Evaluate(MSBuildEvaluationContext context)
		{
			if (!string.IsNullOrEmpty(base.Condition))
			{
				string condition;
				if (!context.Evaluate(base.Condition, out condition))
				{
					foreach (MSBuildProperty msbuildProperty in this.Properties)
					{
						context.ClearPropertyValue(msbuildProperty.Name);
					}
					return;
				}
				if (!ConditionParser.ParseAndEvaluate(condition, context))
				{
					return;
				}
			}
			foreach (MSBuildProperty msbuildProperty2 in this.propertyList)
			{
				msbuildProperty2.Evaluate(context);
			}
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x000485A4 File Offset: 0x000467A4
		public override string ToString()
		{
			string text = "[MSBuildPropertyGroup:";
			foreach (MSBuildProperty msbuildProperty in this.Properties)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					" ",
					msbuildProperty.Name,
					"=",
					msbuildProperty.GetValue(true)
				});
			}
			return text + "]";
		}

		// Token: 0x0400051A RID: 1306
		private Dictionary<string, MSBuildProperty> properties = new Dictionary<string, MSBuildProperty>();

		// Token: 0x0400051B RID: 1307
		private List<MSBuildProperty> propertyList = new List<MSBuildProperty>();

		// Token: 0x0400051C RID: 1308
		private MSBuildProject parent;
	}
}
