using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Build.BuildEngine;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001D4 RID: 468
	public class MSBuildEvaluationContext : IExpressionContext
	{
		// Token: 0x060011DB RID: 4571 RVA: 0x00048D98 File Offset: 0x00046F98
		internal void InitEvaluation(MSBuildProject project)
		{
			this.project = project;
			this.SetPropertyValue("MSBuildThisFile", Path.GetFileName(project.FileName));
			this.SetPropertyValue("MSBuildThisFileName", Path.GetFileNameWithoutExtension(project.FileName));
			this.SetPropertyValue("MSBuildThisFileDirectory", Path.GetDirectoryName(project.FileName) + Path.DirectorySeparatorChar);
			this.SetPropertyValue("MSBuildThisFileExtension", Path.GetExtension(project.FileName));
			this.SetPropertyValue("MSBuildThisFileFullPath", Path.GetFullPath(project.FileName));
			this.SetPropertyValue("VisualStudioReferenceAssemblyVersion", project.ToolsVersion + ".0.0");
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00048E44 File Offset: 0x00047044
		public string GetPropertyValue(string name)
		{
			string result;
			if (this.properties.TryGetValue(name, out result))
			{
				return result;
			}
			return Environment.GetEnvironmentVariable(name);
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x00048E69 File Offset: 0x00047069
		public void SetPropertyValue(string name, string value)
		{
			this.properties[name] = value;
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x00048E78 File Offset: 0x00047078
		public void ClearPropertyValue(string name)
		{
			this.properties.Remove(name);
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00048E87 File Offset: 0x00047087
		public bool Evaluate(XmlElement source, out XmlElement result)
		{
			this.allResolved = true;
			result = (XmlElement)this.EvaluateNode(source);
			return this.allResolved;
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00048EA4 File Offset: 0x000470A4
		private XmlNode EvaluateNode(XmlNode source)
		{
			XmlElement xmlElement = source as XmlElement;
			if (xmlElement != null)
			{
				XmlElement xmlElement2 = source.OwnerDocument.CreateElement(xmlElement.Prefix, xmlElement.LocalName, xmlElement.NamespaceURI);
				foreach (object obj in xmlElement.Attributes)
				{
					XmlAttribute source2 = (XmlAttribute)obj;
					xmlElement2.Attributes.Append((XmlAttribute)this.EvaluateNode(source2));
				}
				foreach (object obj2 in xmlElement.ChildNodes)
				{
					XmlNode source3 = (XmlNode)obj2;
					xmlElement2.AppendChild(this.EvaluateNode(source3));
				}
				return xmlElement2;
			}
			XmlAttribute xmlAttribute = source as XmlAttribute;
			if (xmlAttribute != null)
			{
				bool flag = this.allResolved;
				XmlAttribute xmlAttribute2 = source.OwnerDocument.CreateAttribute(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI);
				xmlAttribute2.Value = this.Evaluate(xmlAttribute.Value);
				if (xmlAttribute.Name == "Condition")
				{
					this.allResolved = flag;
				}
				return xmlAttribute2;
			}
			XmlText xmlText = source as XmlText;
			if (xmlText != null)
			{
				return source.OwnerDocument.CreateTextNode(this.Evaluate(xmlText.InnerText));
			}
			return source.Clone();
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x0004902C File Offset: 0x0004722C
		public bool Evaluate(string str, out string result)
		{
			this.allResolved = true;
			result = this.Evaluate(str);
			return this.allResolved;
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x00049044 File Offset: 0x00047244
		private string Evaluate(string str)
		{
			int num = str.IndexOf("$(");
			if (num == -1)
			{
				return str;
			}
			int num2 = 0;
			StringBuilder stringBuilder = new StringBuilder();
			for (;;)
			{
				stringBuilder.Append(str, num2, num - num2);
				num += 2;
				int num3 = str.IndexOf(")", num);
				if (num3 == -1)
				{
					break;
				}
				string name = str.Substring(num, num3 - num);
				string propertyValue = this.GetPropertyValue(name);
				if (propertyValue == null)
				{
					goto Block_3;
				}
				stringBuilder.Append(propertyValue);
				num2 = num3 + 1;
				num = str.IndexOf("$(", num2);
				if (num == -1)
				{
					goto Block_4;
				}
			}
			this.allResolved = false;
			return "";
			Block_3:
			this.allResolved = false;
			return "";
			Block_4:
			stringBuilder.Append(str, num2, str.Length - num2);
			return stringBuilder.ToString();
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x000490F5 File Offset: 0x000472F5
		public string EvaluateString(string value)
		{
			if (value.StartsWith("$(") && value.EndsWith(")"))
			{
				return this.GetPropertyValue(value.Substring(2, value.Length - 3)) ?? value;
			}
			return value;
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x0004912D File Offset: 0x0004732D
		public string FullFileName
		{
			get
			{
				return this.project.FileName;
			}
		}

		// Token: 0x0400051F RID: 1311
		private Dictionary<string, string> properties = new Dictionary<string, string>();

		// Token: 0x04000520 RID: 1312
		private bool allResolved;

		// Token: 0x04000521 RID: 1313
		private MSBuildProject project;
	}
}
