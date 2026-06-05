using System;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects
{
	// Token: 0x02000123 RID: 291
	[DataItem(FallbackType = typeof(UnknownProjectParameters))]
	public class ProjectParameters : ILoadController
	{
		// Token: 0x06000ACA RID: 2762 RVA: 0x00028B78 File Offset: 0x00026D78
		public ProjectParameters()
		{
			ProjectExtensionUtil.LoadControl(this);
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x00028B86 File Offset: 0x00026D86
		public virtual ProjectParameters Clone()
		{
			return (ProjectParameters)base.MemberwiseClone();
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x00028B93 File Offset: 0x00026D93
		void ILoadController.BeginLoad()
		{
			this.OnBeginLoad();
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x00028B9B File Offset: 0x00026D9B
		void ILoadController.EndLoad()
		{
			this.OnEndLoad();
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x00028BA3 File Offset: 0x00026DA3
		// (set) Token: 0x06000ACF RID: 2767 RVA: 0x00028BAB File Offset: 0x00026DAB
		public DotNetProject ParentProject
		{
			get
			{
				return this.parentProject;
			}
			internal set
			{
				this.parentProject = value;
			}
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00028BB4 File Offset: 0x00026DB4
		protected virtual void OnBeginLoad()
		{
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00028BB6 File Offset: 0x00026DB6
		protected virtual void OnEndLoad()
		{
		}

		// Token: 0x04000343 RID: 835
		private DotNetProject parentProject;
	}
}
