using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000A RID: 10
	internal class BinContainer
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00002D8C File Offset: 0x00000F8C
		public static BinContainer Attach(Bin bin)
		{
			BinContainer binContainer = new BinContainer();
			bin.SizeRequested += binContainer.OnSizeRequested;
			bin.SizeAllocated += binContainer.OnSizeAllocated;
			bin.Added += binContainer.OnAdded;
			return binContainer;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002DD6 File Offset: 0x00000FD6
		private void OnSizeRequested(object sender, SizeRequestedArgs args)
		{
			if (this.child != null)
			{
				args.Requisition = this.child.SizeRequest();
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002DF1 File Offset: 0x00000FF1
		private void OnSizeAllocated(object sender, SizeAllocatedArgs args)
		{
			if (this.child != null)
			{
				this.child.Allocation = args.Allocation;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002E0C File Offset: 0x0000100C
		private void OnAdded(object sender, AddedArgs args)
		{
			this.child = args.Widget;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002E1A File Offset: 0x0000101A
		public void SetUiManager(UIManager uim)
		{
			this.uimanager = uim;
			this.child.Realized += this.OnRealized;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002E3C File Offset: 0x0000103C
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

		// Token: 0x0400001D RID: 29
		private Widget child;

		// Token: 0x0400001E RID: 30
		private UIManager uimanager;
	}
}
