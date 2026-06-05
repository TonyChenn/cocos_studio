using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Projects.Formates;
using Mono.Addins;

namespace CocoStudio.Core
{
	// Token: 0x02000007 RID: 7
	public class CompositeProcesserManager
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002314 File Offset: 0x00000514
		// (set) Token: 0x06000012 RID: 18 RVA: 0x0000231B File Offset: 0x0000051B
		public static CompositeProcesserManager Instance { get; private set; } = new CompositeProcesserManager();

		// Token: 0x06000014 RID: 20 RVA: 0x0000232F File Offset: 0x0000052F
		private CompositeProcesserManager()
		{
			this.Initialize();
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000233D File Offset: 0x0000053D
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002345 File Offset: 0x00000545
		public IEnumerable<string> AfterTypes
		{
			get
			{
				return this.afterTypes;
			}
			set
			{
				this.afterTypes = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000234E File Offset: 0x0000054E
		// (set) Token: 0x06000018 RID: 24 RVA: 0x0000235B File Offset: 0x0000055B
		public List<string> CompositeFilterTypes
		{
			get
			{
				return this.compositeFilterTypes.ToList<string>();
			}
			set
			{
				this.compositeFilterTypes = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002364 File Offset: 0x00000564
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002371 File Offset: 0x00000571
		public List<string> PretreatmentTypes
		{
			get
			{
				return this.pretreatmentTypes.ToList<string>();
			}
			set
			{
				this.pretreatmentTypes = value;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000237C File Offset: 0x0000057C
		private void Initialize()
		{
			try
			{
				HashSet<string> hashSet = new HashSet<string>();
				HashSet<string> hashSet2 = new HashSet<string>();
				HashSet<string> hashSet3 = new HashSet<string>();
				ICompositeResourceProcesser[] extensionObjects = AddinManager.GetExtensionObjects<ICompositeResourceProcesser>();
				foreach (ICompositeResourceProcesser compositeResourceProcesser in extensionObjects)
				{
					List<string> list = compositeResourceProcesser.GetPretreatmentTypes();
					if (list != null)
					{
						foreach (string item in list)
						{
							hashSet.Add(item);
						}
					}
					List<string> filterTypes = compositeResourceProcesser.GetFilterTypes();
					if (filterTypes != null)
					{
						foreach (string item2 in filterTypes)
						{
							hashSet2.Add(item2);
						}
					}
					List<string> list2 = compositeResourceProcesser.GetAfterTypes();
					if (list2 != null)
					{
						foreach (string item3 in list2)
						{
							hashSet3.Add(item3);
						}
					}
				}
				this.pretreatmentTypes = hashSet;
				this.compositeFilterTypes = hashSet2;
				this.afterTypes = hashSet3;
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Debug("Load resource panel addins failed", exception);
			}
		}

		// Token: 0x04000005 RID: 5
		private IEnumerable<string> pretreatmentTypes;

		// Token: 0x04000006 RID: 6
		private IEnumerable<string> compositeFilterTypes;

		// Token: 0x04000007 RID: 7
		private IEnumerable<string> afterTypes;
	}
}
