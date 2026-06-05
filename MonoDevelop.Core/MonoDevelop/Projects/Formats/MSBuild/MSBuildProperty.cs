using System;
using System.Xml;
using Microsoft.Build.BuildEngine;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001CC RID: 460
	public class MSBuildProperty : MSBuildObject
	{
		// Token: 0x0600119A RID: 4506 RVA: 0x00047C6F File Offset: 0x00045E6F
		public MSBuildProperty(XmlElement elem) : base(elem)
		{
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x0600119B RID: 4507 RVA: 0x00047C78 File Offset: 0x00045E78
		public string Name
		{
			get
			{
				return base.Element.Name;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x0600119C RID: 4508 RVA: 0x00047C85 File Offset: 0x00045E85
		// (set) Token: 0x0600119D RID: 4509 RVA: 0x00047C8D File Offset: 0x00045E8D
		internal bool Overwritten { get; set; }

		// Token: 0x0600119E RID: 4510 RVA: 0x00047C96 File Offset: 0x00045E96
		public string GetValue(bool isXml = false)
		{
			if (isXml)
			{
				return base.EvaluatedElement.InnerXml;
			}
			return base.EvaluatedElement.InnerText;
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x00047CB2 File Offset: 0x00045EB2
		public void SetValue(string value, bool isXml = false)
		{
			if (isXml)
			{
				base.Element.InnerXml = value;
				return;
			}
			base.Element.InnerText = value;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00047CD0 File Offset: 0x00045ED0
		internal override void Evaluate(MSBuildEvaluationContext context)
		{
			base.EvaluatedElement = null;
			if (!string.IsNullOrEmpty(base.Condition))
			{
				string condition;
				if (!context.Evaluate(base.Condition, out condition))
				{
					context.ClearPropertyValue(this.Name);
					return;
				}
				if (!ConditionParser.ParseAndEvaluate(condition, context))
				{
					return;
				}
			}
			XmlElement evaluatedElement;
			if (context.Evaluate(base.Element, out evaluatedElement))
			{
				base.EvaluatedElement = evaluatedElement;
				context.SetPropertyValue(this.Name, this.GetValue(false));
				return;
			}
			context.ClearPropertyValue(this.Name);
		}
	}
}
