using System;
using Mono.Addins;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x020001A3 RID: 419
	public class ProjectLanguageCondition : ConditionType
	{
		// Token: 0x06000FEF RID: 4079 RVA: 0x0003B005 File Offset: 0x00039205
		public ProjectLanguageCondition(object obj)
		{
			this.TargetProject = obj;
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x0003B014 File Offset: 0x00039214
		// (set) Token: 0x06000FF1 RID: 4081 RVA: 0x0003B01C File Offset: 0x0003921C
		public object TargetProject
		{
			get
			{
				return this.target;
			}
			set
			{
				if (this.target != value)
				{
					this.target = value;
					DotNetProject dotNetProject = this.target as DotNetProject;
					if (dotNetProject != null)
					{
						this.language = dotNetProject.LanguageName;
					}
					else
					{
						this.language = null;
					}
					base.NotifyChanged();
				}
			}
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0003B063 File Offset: 0x00039263
		public override bool Evaluate(NodeElement conditionNode)
		{
			return !string.IsNullOrEmpty(this.language) && this.language == conditionNode.GetAttribute("value");
		}

		// Token: 0x040004A5 RID: 1189
		private string language;

		// Token: 0x040004A6 RID: 1190
		private object target;
	}
}
