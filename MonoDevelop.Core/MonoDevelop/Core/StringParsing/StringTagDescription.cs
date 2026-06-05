using System;

namespace MonoDevelop.Core.StringParsing
{
	// Token: 0x02000203 RID: 515
	public class StringTagDescription
	{
		// Token: 0x0600139B RID: 5019 RVA: 0x00050DF4 File Offset: 0x0004EFF4
		public StringTagDescription(string name, string description)
		{
			this.name = name;
			this.description = description;
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x00050E11 File Offset: 0x0004F011
		public StringTagDescription(string name, string description, bool important)
		{
			this.name = name;
			this.description = description;
			this.important = important;
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00050E35 File Offset: 0x0004F035
		internal void SetSource(IStringTagProvider provider, object instance)
		{
			this.provider = provider;
			this.instance = instance;
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00050E45 File Offset: 0x0004F045
		internal object GetValue()
		{
			return this.provider.GetTagValue(this.instance, this.name.ToUpperInvariant());
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x0600139F RID: 5023 RVA: 0x00050E63 File Offset: 0x0004F063
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060013A0 RID: 5024 RVA: 0x00050E6B File Offset: 0x0004F06B
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x060013A1 RID: 5025 RVA: 0x00050E73 File Offset: 0x0004F073
		public bool Important
		{
			get
			{
				return this.important;
			}
		}

		// Token: 0x040005C5 RID: 1477
		private string name;

		// Token: 0x040005C6 RID: 1478
		private string description;

		// Token: 0x040005C7 RID: 1479
		private bool important = true;

		// Token: 0x040005C8 RID: 1480
		private IStringTagProvider provider;

		// Token: 0x040005C9 RID: 1481
		private object instance;
	}
}
