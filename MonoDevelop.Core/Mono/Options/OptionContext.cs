using System;

namespace Mono.Options
{
	// Token: 0x020000F0 RID: 240
	public class OptionContext
	{
		// Token: 0x06000878 RID: 2168 RVA: 0x000218E0 File Offset: 0x0001FAE0
		public OptionContext(OptionSet set)
		{
			this.set = set;
			this.c = new OptionValueCollection(this);
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x000218FB File Offset: 0x0001FAFB
		// (set) Token: 0x0600087A RID: 2170 RVA: 0x00021903 File Offset: 0x0001FB03
		public Option Option
		{
			get
			{
				return this.option;
			}
			set
			{
				this.option = value;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x0002190C File Offset: 0x0001FB0C
		// (set) Token: 0x0600087C RID: 2172 RVA: 0x00021914 File Offset: 0x0001FB14
		public string OptionName
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x0002191D File Offset: 0x0001FB1D
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x00021925 File Offset: 0x0001FB25
		public int OptionIndex
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x0002192E File Offset: 0x0001FB2E
		public OptionSet OptionSet
		{
			get
			{
				return this.set;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x00021936 File Offset: 0x0001FB36
		public OptionValueCollection OptionValues
		{
			get
			{
				return this.c;
			}
		}

		// Token: 0x040002AB RID: 683
		private Option option;

		// Token: 0x040002AC RID: 684
		private string name;

		// Token: 0x040002AD RID: 685
		private int index;

		// Token: 0x040002AE RID: 686
		private OptionSet set;

		// Token: 0x040002AF RID: 687
		private OptionValueCollection c;
	}
}
