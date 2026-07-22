using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000D RID: 13
	internal class BinContainer
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00002CD8 File Offset: 0x00000ED8
		public static BinContainer Attach(Bin bin)
		{
			BinContainer binContainer = new BinContainer();
			bin.SizeRequested += binContainer.OnSizeRequested;
			bin.SizeAllocated += binContainer.OnSizeAllocated;
			bin.Added += binContainer.OnAdded;
			return binContainer;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002D22 File Offset: 0x00000F22
		private void OnSizeRequested(object sender, SizeRequestedArgs args)
		{
			if (this.child != null)
			{
				args.Requisition = this.child.SizeRequest();
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002D3D File Offset: 0x00000F3D
		private void OnSizeAllocated(object sender, SizeAllocatedArgs args)
		{
			if (this.child != null)
			{
				this.child.Allocation = args.Allocation;
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002D58 File Offset: 0x00000F58
		private void OnAdded(object sender, AddedArgs args)
		{
			this.child = args.Widget;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002D66 File Offset: 0x00000F66
		public void SetUiManager(UIManager uim)
		{
			this.uimanager = uim;
			this.child.Realized += this.OnRealized;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002D88 File Offset: 0x00000F88
		private void OnRealized(object sender, EventArgs args)
		{
			if (this.uimanager != null)
			{
				Widget toplevel = this.child.Toplevel;
				if (toplevel != null && typeof(Window).IsInstanceOfType(toplevel))
				{
					((Window)toplevel).AddAccelGroup(this.uimanager.AccelGroup);
					this.uimanager = null;
				}
			}
		}

		// Token: 0x04000017 RID: 23
		private Widget child;

		// Token: 0x04000018 RID: 24
		private UIManager uimanager;
	}
}
