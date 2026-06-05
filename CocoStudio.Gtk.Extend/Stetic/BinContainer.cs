using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000057 RID: 87
	internal class BinContainer
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x000086B4 File Offset: 0x000068B4
		public static BinContainer Attach(Bin bin)
		{
			BinContainer binContainer = new BinContainer();
			bin.SizeRequested += binContainer.OnSizeRequested;
			bin.SizeAllocated += binContainer.OnSizeAllocated;
			bin.Added += binContainer.OnAdded;
			return binContainer;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00008708 File Offset: 0x00006908
		private void OnSizeRequested(object sender, SizeRequestedArgs args)
		{
			if (this.child != null)
			{
				args.Requisition = this.child.SizeRequest();
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00008738 File Offset: 0x00006938
		private void OnSizeAllocated(object sender, SizeAllocatedArgs args)
		{
			if (this.child != null)
			{
				this.child.Allocation = args.Allocation;
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00008767 File Offset: 0x00006967
		private void OnAdded(object sender, AddedArgs args)
		{
			this.child = args.Widget;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00008776 File Offset: 0x00006976
		public void SetUiManager(UIManager uim)
		{
			this.uimanager = uim;
			this.child.Realized += this.OnRealized;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00008798 File Offset: 0x00006998
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

		// Token: 0x040002F8 RID: 760
		private Widget child;

		// Token: 0x040002F9 RID: 761
		private UIManager uimanager;
	}
}
