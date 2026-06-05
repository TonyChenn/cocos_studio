using System;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	/// <summary>This should really be called DotNetCompilerParameters</summary>
	// Token: 0x02000124 RID: 292
	[DataItem(FallbackType = typeof(UnknownCompilationParameters))]
	public abstract class ConfigurationParameters : ProjectParameters
	{
		// Token: 0x06000AD2 RID: 2770 RVA: 0x00028C58 File Offset: 0x00026E58
		public virtual IEnumerable<string> GetDefineSymbols()
		{
			yield break;
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00028C75 File Offset: 0x00026E75
		[Obsolete]
		public virtual void AddDefineSymbol(string symbol)
		{
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00028C77 File Offset: 0x00026E77
		[Obsolete]
		public virtual void RemoveDefineSymbol(string symbol)
		{
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00028C90 File Offset: 0x00026E90
		[Obsolete]
		public virtual bool HasDefineSymbol(string symbol)
		{
			return this.GetDefineSymbols().Any((string s) => s == symbol);
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x00028CC1 File Offset: 0x00026EC1
		public new ConfigurationParameters Clone()
		{
			return (ConfigurationParameters)base.Clone();
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x00028CCE File Offset: 0x00026ECE
		// (set) Token: 0x06000AD8 RID: 2776 RVA: 0x00028CD6 File Offset: 0x00026ED6
		public DotNetProjectConfiguration ParentConfiguration
		{
			get
			{
				return this.configuration;
			}
			internal set
			{
				this.configuration = value;
				if (this.configuration != null)
				{
					base.ParentProject = this.configuration.ParentItem;
				}
			}
		}

		// Token: 0x04000344 RID: 836
		private DotNetProjectConfiguration configuration;
	}
}
