using System;
using System.Collections;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x02000105 RID: 261
	public class ItemConfiguration : IExtendedDataItem
	{
		// Token: 0x06000963 RID: 2403 RVA: 0x000258E4 File Offset: 0x00023AE4
		public ItemConfiguration()
		{
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x000258F7 File Offset: 0x00023AF7
		public ItemConfiguration(string id)
		{
			ItemConfiguration.ParseConfigurationId(id, out this.name, out this.platform);
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0002591C File Offset: 0x00023B1C
		public ItemConfiguration(string name, string platform)
		{
			this.name = name;
			this.platform = platform;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00025940 File Offset: 0x00023B40
		public static void ParseConfigurationId(string id, out string name, out string platform)
		{
			if (string.IsNullOrEmpty(id))
			{
				name = "";
				platform = "";
				return;
			}
			int num = id.IndexOf('|');
			if (num < 0)
			{
				name = id;
				platform = string.Empty;
				return;
			}
			name = id.Substring(0, num);
			platform = id.Substring(num + 1);
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x00025991 File Offset: 0x00023B91
		// (set) Token: 0x06000968 RID: 2408 RVA: 0x00025999 File Offset: 0x00023B99
		public string Name
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

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x000259A2 File Offset: 0x00023BA2
		// (set) Token: 0x0600096A RID: 2410 RVA: 0x000259CE File Offset: 0x00023BCE
		public string Id
		{
			get
			{
				if (string.IsNullOrEmpty(this.platform))
				{
					return this.name;
				}
				return this.name + "|" + this.platform;
			}
			set
			{
				ItemConfiguration.ParseConfigurationId(value, out this.name, out this.platform);
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x000259E2 File Offset: 0x00023BE2
		// (set) Token: 0x0600096C RID: 2412 RVA: 0x000259F3 File Offset: 0x00023BF3
		public string Platform
		{
			get
			{
				return this.platform ?? string.Empty;
			}
			set
			{
				this.platform = value;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x000259FC File Offset: 0x00023BFC
		public CustomCommandCollection CustomCommands
		{
			get
			{
				return this.customCommands;
			}
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00025A04 File Offset: 0x00023C04
		public object Clone()
		{
			ItemConfiguration itemConfiguration = (ItemConfiguration)Activator.CreateInstance(base.GetType());
			itemConfiguration.CopyFrom(this);
			itemConfiguration.name = this.name;
			itemConfiguration.platform = this.platform;
			return itemConfiguration;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x00025A44 File Offset: 0x00023C44
		public virtual void CopyFrom(ItemConfiguration configuration)
		{
			if (configuration.properties != null)
			{
				this.properties = new Hashtable();
				using (IDictionaryEnumerator enumerator = configuration.properties.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						if (dictionaryEntry.Value is ICloneable)
						{
							this.properties[dictionaryEntry.Key] = ((ICloneable)dictionaryEntry.Value).Clone();
						}
						else
						{
							this.properties[dictionaryEntry.Key] = dictionaryEntry.Value;
						}
					}
					goto IL_A0;
				}
			}
			this.properties = null;
			IL_A0:
			this.customCommands = configuration.customCommands.Clone();
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00025B14 File Offset: 0x00023D14
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x00025B1C File Offset: 0x00023D1C
		public IDictionary ExtendedProperties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new Hashtable();
				}
				return this.properties;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x00025B37 File Offset: 0x00023D37
		public virtual ConfigurationSelector Selector
		{
			get
			{
				return new SolutionConfigurationSelector(this.Id);
			}
		}

		// Token: 0x040002F6 RID: 758
		[ItemProperty]
		private string name;

		// Token: 0x040002F7 RID: 759
		private string platform;

		// Token: 0x040002F8 RID: 760
		[ItemProperty("Command", Scope = "*")]
		[ItemProperty("CustomCommands")]
		private CustomCommandCollection customCommands = new CustomCommandCollection();

		// Token: 0x040002F9 RID: 761
		private Hashtable properties;
	}
}
