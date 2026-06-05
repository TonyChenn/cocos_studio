using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.StringParsing
{
	// Token: 0x02000206 RID: 518
	public class StringTagModelDescription
	{
		// Token: 0x060013AA RID: 5034 RVA: 0x00051194 File Offset: 0x0004F394
		public void Add(Type type)
		{
			this.types.Add(type);
			this.initialized = false;
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x000511A9 File Offset: 0x0004F3A9
		public void Add(StringTagModelDescription model)
		{
			this.models.Add(model);
			this.initialized = false;
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x000511C0 File Offset: 0x0004F3C0
		private void Initialize()
		{
			this.initialized = true;
			this.tags.Clear();
			foreach (Type type in this.types)
			{
				List<StringTagDescription> list = new List<StringTagDescription>();
				foreach (IStringTagProvider stringTagProvider in StringParserService.GetProviders())
				{
					foreach (StringTagDescription stringTagDescription in stringTagProvider.GetTags(type))
					{
						if (this.tagNames.Add(stringTagDescription.Name))
						{
							list.Add(stringTagDescription);
						}
					}
					foreach (StringTagDescription stringTagDescription2 in stringTagProvider.GetTags(null))
					{
						if (this.tagNames.Add(stringTagDescription2.Name))
						{
							list.Add(stringTagDescription2);
						}
					}
				}
				this.tags.Add(list);
			}
			foreach (StringTagModelDescription stringTagModelDescription in this.models)
			{
				foreach (StringTagDescription[] array in stringTagModelDescription.GetTagsGrouped())
				{
					List<StringTagDescription> list2 = new List<StringTagDescription>();
					foreach (StringTagDescription stringTagDescription3 in array)
					{
						if (this.tagNames.Add(stringTagDescription3.Name))
						{
							list2.Add(stringTagDescription3);
						}
					}
					this.tags.Add(list2);
				}
			}
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00051618 File Offset: 0x0004F818
		public IEnumerable<StringTagDescription> GetTags()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			foreach (List<StringTagDescription> list in this.tags)
			{
				foreach (StringTagDescription tag in list)
				{
					yield return tag;
				}
			}
			yield break;
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x000517DC File Offset: 0x0004F9DC
		public IEnumerable<StringTagDescription[]> GetTagsGrouped()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			foreach (List<StringTagDescription> list in this.tags)
			{
				yield return list.ToArray();
			}
			yield break;
		}

		// Token: 0x040005CD RID: 1485
		private List<List<StringTagDescription>> tags = new List<List<StringTagDescription>>();

		// Token: 0x040005CE RID: 1486
		private HashSet<string> tagNames = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

		// Token: 0x040005CF RID: 1487
		private List<Type> types = new List<Type>();

		// Token: 0x040005D0 RID: 1488
		private List<StringTagModelDescription> models = new List<StringTagModelDescription>();

		// Token: 0x040005D1 RID: 1489
		private bool initialized;
	}
}
