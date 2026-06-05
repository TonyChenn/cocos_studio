using System;
using System.Collections;
using MonoDevelop.Components.Commands;

namespace CocoStudio.Core.ExtensionModel
{
	// Token: 0x02000006 RID: 6
	public class MenuArrayInfo : IEnumerable
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00003038 File Offset: 0x00001238
		internal MenuArrayInfo(CommandArrayInfo info)
		{
			this.cmdArrayInfo = info;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000304A File Offset: 0x0000124A
		internal MenuArrayInfo(CommandInfo cmdInfo)
		{
			this.cmdArrayInfo = new CommandArrayInfo(cmdInfo);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00003061 File Offset: 0x00001261
		public void Clear()
		{
			this.cmdArrayInfo.Clear();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003070 File Offset: 0x00001270
		public MenuInfo FindCommandInfo(object dataItem)
		{
			return new MenuInfo(this.cmdArrayInfo.FindCommandInfo(dataItem));
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003093 File Offset: 0x00001293
		public void Insert(int index, MenuInfo info, object dataItem)
		{
			this.cmdArrayInfo.Insert(index, info.cmdInfo, dataItem);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000030AA File Offset: 0x000012AA
		public void Add(MenuInfo info, object dataItem)
		{
			this.cmdArrayInfo.Add(info.cmdInfo, dataItem);
		}

		// Token: 0x17000002 RID: 2
		public MenuInfo this[int n]
		{
			get
			{
				return new MenuInfo(this.cmdArrayInfo[n]);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000030E4 File Offset: 0x000012E4
		public int Count
		{
			get
			{
				return this.cmdArrayInfo.Count;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00003101 File Offset: 0x00001301
		public void AddSeparator()
		{
			this.cmdArrayInfo.AddSeparator();
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00003110 File Offset: 0x00001310
		public MenuInfo DefaultCommandInfo
		{
			get
			{
				return new MenuInfo(this.cmdArrayInfo.DefaultCommandInfo);
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003134 File Offset: 0x00001334
		public IEnumerator GetEnumerator()
		{
			return this.cmdArrayInfo.GetEnumerator();
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00003154 File Offset: 0x00001354
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00003171 File Offset: 0x00001371
		public bool Bypass
		{
			get
			{
				return this.cmdArrayInfo.Bypass;
			}
			set
			{
				this.cmdArrayInfo.Bypass = value;
			}
		}

		// Token: 0x0400002F RID: 47
		private CommandArrayInfo cmdArrayInfo;
	}
}
