using System;

namespace CocoStudio.Projects
{
	// Token: 0x0200003A RID: 58
	public abstract class BaseCocosFileSerializer : IGameFileSerializer, IComparable
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000154 RID: 340 RVA: 0x0000606C File Offset: 0x0000426C
		public string ID
		{
			get
			{
				return this.OnGetID();
			}
		}

		// Token: 0x06000155 RID: 341
		protected abstract string OnGetID();

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00006074 File Offset: 0x00004274
		public string Label
		{
			get
			{
				return this.OnGetLabel();
			}
		}

		// Token: 0x06000157 RID: 343
		protected abstract string OnGetLabel();

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000158 RID: 344 RVA: 0x0000607C File Offset: 0x0000427C
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00006084 File Offset: 0x00004284
		public bool IsDefault { get; internal set; }

		// Token: 0x0600015A RID: 346 RVA: 0x0000608D File Offset: 0x0000428D
		public string Serialize(PublishInfo info, GameFile projFile)
		{
			return this.OnSerialize(info, projFile);
		}

		// Token: 0x0600015B RID: 347
		protected abstract string OnSerialize(PublishInfo info, GameFile projFile);

		// Token: 0x0600015C RID: 348 RVA: 0x00006097 File Offset: 0x00004297
		public virtual void ContextInitialize(PublishInfo publishInfo)
		{
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00006099 File Offset: 0x00004299
		public virtual void ContextFinalize(PublishInfo publishInfo)
		{
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600015E RID: 350 RVA: 0x0000609B File Offset: 0x0000429B
		// (set) Token: 0x0600015F RID: 351 RVA: 0x000060A3 File Offset: 0x000042A3
		public virtual string Description { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000160 RID: 352 RVA: 0x000060AC File Offset: 0x000042AC
		public string SolutionLink
		{
			get
			{
				return "http://www.cocos2d-x.org/download";
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000161 RID: 353 RVA: 0x000060B3 File Offset: 0x000042B3
		// (set) Token: 0x06000162 RID: 354 RVA: 0x000060BB File Offset: 0x000042BB
		private protected virtual int DisplayIndex { protected get; private set; }

		// Token: 0x06000163 RID: 355 RVA: 0x000060C4 File Offset: 0x000042C4
		public int CompareTo(object other)
		{
			return this.DisplayIndex.CompareTo(((BaseCocosFileSerializer)other).DisplayIndex);
		}

		// Token: 0x04000053 RID: 83
		private const string defaultSolutionLink = "http://www.cocos2d-x.org/download";
	}
}
