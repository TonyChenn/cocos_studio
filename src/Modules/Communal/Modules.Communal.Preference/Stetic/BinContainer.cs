using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000004 RID: 4
	internal class BinContainer
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002068 File Offset: 0x00000268
		public static BinContainer Attach(Bin bin)
		{
			BinContainer binContainer = new BinContainer();
			bin.SizeRequested += binContainer.OnSizeRequested;
			bin.SizeAllocated += binContainer.OnSizeAllocated;
			bin.Added += binContainer.OnAdded;
			return binContainer;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020B2 File Offset: 0x000002B2
		private void OnSizeRequested(object sender, SizeRequestedArgs args)
		{
			if (this.child != null)
			{
				args.Requisition = this.child.SizeRequest();
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020CD File Offset: 0x000002CD
		private void OnSizeAllocated(object sender, SizeAllocatedArgs args)
		{
			if (this.child != null)
			{
				this.child.Allocation = args.Allocation;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020E8 File Offset: 0x000002E8
		private void OnAdded(object sender, AddedArgs args)
		{
			this.child = args.Widget;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020F6 File Offset: 0x000002F6
		public void SetUiManager(UIManager uim)
		{
			this.uimanager = uim;
			this.child.Realized += this.OnRealized;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002118 File Offset: 0x00000318
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

		// Token: 0x0400000C RID: 12
		private Widget child;

		// Token: 0x0400000D RID: 13
		private UIManager uimanager;
	}
}
