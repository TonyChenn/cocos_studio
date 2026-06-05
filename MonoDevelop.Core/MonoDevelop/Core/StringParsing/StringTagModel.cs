using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.StringParsing
{
	// Token: 0x02000205 RID: 517
	public class StringTagModel : IStringTagModel
	{
		// Token: 0x060013A3 RID: 5027 RVA: 0x00050E7B File Offset: 0x0004F07B
		public void Add(object obj)
		{
			this.objects.Add(obj);
			this.initialized = false;
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x00050E90 File Offset: 0x0004F090
		public void Add(StringTagModel source)
		{
			this.objects.Add(source);
			this.initialized = false;
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x00050EA5 File Offset: 0x0004F0A5
		public void Remove(object obj)
		{
			this.objects.Remove(obj);
			this.initialized = false;
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00050EBC File Offset: 0x0004F0BC
		private void Initialize()
		{
			this.initialized = true;
			this.tags.Clear();
			foreach (IStringTagProvider stringTagProvider in StringParserService.GetProviders())
			{
				foreach (object obj in this.objects)
				{
					if (!(obj is StringTagModel))
					{
						foreach (StringTagDescription stringTagDescription in stringTagProvider.GetTags(obj.GetType()))
						{
							if (!this.tags.ContainsKey(stringTagDescription.Name))
							{
								stringTagDescription.SetSource(stringTagProvider, obj);
								this.tags.Add(stringTagDescription.Name, stringTagDescription);
							}
						}
					}
				}
				foreach (StringTagDescription stringTagDescription2 in stringTagProvider.GetTags(null))
				{
					if (!this.tags.ContainsKey(stringTagDescription2.Name))
					{
						stringTagDescription2.SetSource(stringTagProvider, null);
						this.tags.Add(stringTagDescription2.Name, stringTagDescription2);
					}
				}
			}
			foreach (object obj2 in this.objects)
			{
				StringTagModel stringTagModel = obj2 as StringTagModel;
				if (stringTagModel != null)
				{
					foreach (StringTagDescription stringTagDescription3 in stringTagModel.Tags.Values)
					{
						if (!this.tags.ContainsKey(stringTagDescription3.Name))
						{
							this.tags.Add(stringTagDescription3.Name, stringTagDescription3);
						}
					}
				}
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x060013A7 RID: 5031 RVA: 0x00051134 File Offset: 0x0004F334
		private Dictionary<string, StringTagDescription> Tags
		{
			get
			{
				if (!this.initialized)
				{
					this.Initialize();
				}
				return this.tags;
			}
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x0005114C File Offset: 0x0004F34C
		public object GetValue(string name)
		{
			StringTagDescription stringTagDescription;
			if (this.Tags.TryGetValue(name, out stringTagDescription))
			{
				return stringTagDescription.GetValue();
			}
			return null;
		}

		// Token: 0x040005CA RID: 1482
		private Dictionary<string, StringTagDescription> tags = new Dictionary<string, StringTagDescription>(StringComparer.InvariantCultureIgnoreCase);

		// Token: 0x040005CB RID: 1483
		private List<object> objects = new List<object>();

		// Token: 0x040005CC RID: 1484
		private bool initialized;
	}
}
