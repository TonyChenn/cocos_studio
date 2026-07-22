using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000007 RID: 7
	internal class BinContainer
	{
		// Token: 0x06000011 RID: 17 RVA: 0x000022BC File Offset: 0x000004BC
		public static BinContainer Attach(Bin bin)
		{
			BinContainer binContainer = new BinContainer();
			bin.SizeRequested += binContainer.OnSizeRequested;
			bin.SizeAllocated += binContainer.OnSizeAllocated;
			bin.Added += binContainer.OnAdded;
			return binContainer;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002306 File Offset: 0x00000506
		private void OnSizeRequested(object sender, SizeRequestedArgs args)
		{
			if (this.child != null)
			{
				args.Requisition = this.child.SizeRequest();
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002321 File Offset: 0x00000521
		private void OnSizeAllocated(object sender, SizeAllocatedArgs args)
		{
			if (this.child != null)
			{
				this.child.Allocation = args.Allocation;
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000233C File Offset: 0x0000053C
		private void OnAdded(object sender, AddedArgs args)
		{
			this.child = args.Widget;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000234A File Offset: 0x0000054A
		public void SetUiManager(UIManager uim)
		{
			this.uimanager = uim;
			this.child.Realized += this.OnRealized;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000236C File Offset: 0x0000056C
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

		// Token: 0x0400000A RID: 10
		private Widget child;

		// Token: 0x0400000B RID: 11
		private UIManager uimanager;
	}
}
