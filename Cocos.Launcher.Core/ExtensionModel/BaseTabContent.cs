using System;
using CocoStudio.Basic;
using Gtk;

namespace Cocos.Launcher.Core.ExtensionModel
{
	// Token: 0x02000007 RID: 7
	public abstract class BaseTabContent : ITabContent
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000239A File Offset: 0x0000059A
		public virtual int Order
		{
			get
			{
				return int.MaxValue;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000023A1 File Offset: 0x000005A1
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000023A9 File Offset: 0x000005A9
		public virtual Widget Content { get; protected set; }

		// Token: 0x06000023 RID: 35 RVA: 0x000023B4 File Offset: 0x000005B4
		public void Initialize(ITabHead tabHead)
		{
			try
			{
				this.OnInitialize(tabHead);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("TabContent Initialize failed.", exception);
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000023F0 File Offset: 0x000005F0
		protected virtual void OnInitialize(ITabHead tabHead)
		{
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000023F4 File Offset: 0x000005F4
		public void Activated(SwitchTabInfo switchTabInfo)
		{
			try
			{
				this.OnActivated(switchTabInfo);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("TabContent activated failed.", exception);
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002430 File Offset: 0x00000630
		protected virtual void OnActivated(SwitchTabInfo switchTabInfo)
		{
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002434 File Offset: 0x00000634
		public void Deactivated()
		{
			try
			{
				this.OnDeactivated();
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("TabContent Deactivated failed.", exception);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000246C File Offset: 0x0000066C
		protected virtual void OnDeactivated()
		{
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002470 File Offset: 0x00000670
		public void Search(string url)
		{
			try
			{
				this.OnSearch(url);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("TabContent Search failed.", exception);
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000024AC File Offset: 0x000006AC
		protected virtual void OnSearch(string url)
		{
		}
	}
}
