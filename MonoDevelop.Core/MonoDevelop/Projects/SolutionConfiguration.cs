using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MonoDevelop.Projects
{
	// Token: 0x02000106 RID: 262
	public class SolutionConfiguration : ItemConfiguration
	{
		// Token: 0x06000973 RID: 2419 RVA: 0x00025B44 File Offset: 0x00023D44
		public SolutionConfiguration()
		{
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00025B57 File Offset: 0x00023D57
		public SolutionConfiguration(string id) : base(id)
		{
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00025B6B File Offset: 0x00023D6B
		public SolutionConfiguration(string name, string platform) : base(name, platform)
		{
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x00025B80 File Offset: 0x00023D80
		public override ConfigurationSelector Selector
		{
			get
			{
				return new SolutionConfigurationSelector(base.Id);
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x00025B8D File Offset: 0x00023D8D
		// (set) Token: 0x06000978 RID: 2424 RVA: 0x00025B95 File Offset: 0x00023D95
		public Solution ParentSolution
		{
			get
			{
				return this.parentSolution;
			}
			internal set
			{
				this.parentSolution = value;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x00025B9E File Offset: 0x00023D9E
		public ReadOnlyCollection<SolutionConfigurationEntry> Configurations
		{
			get
			{
				return this.configurations.AsReadOnly();
			}
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00025BAC File Offset: 0x00023DAC
		public bool BuildEnabledForItem(SolutionEntityItem item)
		{
			foreach (SolutionConfigurationEntry solutionConfigurationEntry in this.configurations)
			{
				if (solutionConfigurationEntry.Item == item)
				{
					return solutionConfigurationEntry.Build && item.Configurations[solutionConfigurationEntry.ItemConfiguration] != null;
				}
			}
			return false;
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00025C2C File Offset: 0x00023E2C
		public string GetMappedConfiguration(SolutionEntityItem item)
		{
			foreach (SolutionConfigurationEntry solutionConfigurationEntry in this.configurations)
			{
				if (solutionConfigurationEntry.Item == item)
				{
					return solutionConfigurationEntry.ItemConfiguration;
				}
			}
			return null;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00025C90 File Offset: 0x00023E90
		public SolutionConfigurationEntry GetEntryForItem(SolutionEntityItem item)
		{
			foreach (SolutionConfigurationEntry solutionConfigurationEntry in this.configurations)
			{
				if (solutionConfigurationEntry.Item == item)
				{
					return solutionConfigurationEntry;
				}
			}
			return null;
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x00025CEC File Offset: 0x00023EEC
		public SolutionConfigurationEntry AddItem(SolutionEntityItem item)
		{
			string text = this.FindMatchingConfiguration(item);
			return this.AddItem(item, text != null, text);
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x00025D10 File Offset: 0x00023F10
		private string FindMatchingConfiguration(SolutionEntityItem item)
		{
			if (item.Configurations.Count == 0)
			{
				return null;
			}
			if (item.Configurations[base.Id] != null)
			{
				return base.Id;
			}
			foreach (SolutionItemConfiguration solutionItemConfiguration in item.Configurations)
			{
				if (solutionItemConfiguration.Name == base.Name && solutionItemConfiguration.Platform == "")
				{
					return solutionItemConfiguration.Id;
				}
			}
			if (this.ParentSolution != null && this.ParentSolution.StartupItem != null)
			{
				SolutionEntityItem startupItem = this.ParentSolution.StartupItem;
				SolutionItemConfiguration configuration = startupItem.GetConfiguration(this.Selector);
				if (configuration != null)
				{
					SolutionItemConfiguration solutionItemConfiguration2 = configuration.FindBestMatch(item.Configurations);
					if (solutionItemConfiguration2 != null)
					{
						return solutionItemConfiguration2.Id;
					}
				}
			}
			if (base.Platform.Length > 0)
			{
				foreach (SolutionItemConfiguration solutionItemConfiguration3 in item.Configurations)
				{
					if (solutionItemConfiguration3.Platform == base.Platform)
					{
						return solutionItemConfiguration3.Id;
					}
				}
			}
			foreach (SolutionItemConfiguration solutionItemConfiguration4 in item.Configurations)
			{
				if (solutionItemConfiguration4.Name == base.Name)
				{
					return solutionItemConfiguration4.Id;
				}
			}
			return item.Configurations[0].Id;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00025ED8 File Offset: 0x000240D8
		public SolutionConfigurationEntry AddItem(SolutionEntityItem item, bool build, string itemConfiguration)
		{
			if (itemConfiguration == null)
			{
				itemConfiguration = base.Name;
			}
			SolutionConfigurationEntry solutionConfigurationEntry = new SolutionConfigurationEntry(this, item);
			solutionConfigurationEntry.Build = build;
			solutionConfigurationEntry.ItemConfiguration = itemConfiguration;
			this.configurations.Add(solutionConfigurationEntry);
			if (this.parentSolution != null)
			{
				this.parentSolution.UpdateDefaultConfigurations();
			}
			return solutionConfigurationEntry;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00025F28 File Offset: 0x00024128
		public void RemoveItem(SolutionEntityItem item)
		{
			for (int i = 0; i < this.configurations.Count; i++)
			{
				if (this.configurations[i].Item == item)
				{
					this.configurations.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00025F84 File Offset: 0x00024184
		internal void ReplaceItem(SolutionEntityItem oldItem, SolutionEntityItem newItem)
		{
			foreach (SolutionConfigurationEntry solutionConfigurationEntry in from ce in this.configurations
			where ce.Item == oldItem
			select ce)
			{
				solutionConfigurationEntry.Item = newItem;
			}
			if (this.parentSolution != null)
			{
				this.parentSolution.UpdateDefaultConfigurations();
			}
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x00026004 File Offset: 0x00024204
		public override void CopyFrom(ItemConfiguration configuration)
		{
			base.CopyFrom(configuration);
			SolutionConfiguration solutionConfiguration = (SolutionConfiguration)configuration;
			if (this.parentSolution == null)
			{
				this.parentSolution = solutionConfiguration.parentSolution;
			}
			this.configurations.Clear();
			foreach (SolutionConfigurationEntry other in solutionConfiguration.configurations)
			{
				this.configurations.Add(new SolutionConfigurationEntry(this, other));
			}
			if (this.parentSolution != null)
			{
				this.parentSolution.UpdateDefaultConfigurations();
			}
		}

		// Token: 0x040002FA RID: 762
		private Solution parentSolution;

		// Token: 0x040002FB RID: 763
		private List<SolutionConfigurationEntry> configurations = new List<SolutionConfigurationEntry>();
	}
}
