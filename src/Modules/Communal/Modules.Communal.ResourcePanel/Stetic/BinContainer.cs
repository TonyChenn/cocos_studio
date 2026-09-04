using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000A RID: 10
	internal class BinContainer
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00002EFC File Offset: 0x000010FC
		public static BinContainer Attach(Bin bin)
		{
			BinContainer binContainer = new BinContainer();
			bin.SizeRequested += binContainer.OnSizeRequested;
			bin.SizeAllocated += binContainer.OnSizeAllocated;
			bin.Added += binContainer.OnAdded;
			return binContainer;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002F46 File Offset: 0x00001146
		private void OnSizeRequested(object sender, SizeRequestedArgs args)
		{
			if (this.child != null)
			{
				args.Requisition = this.child.SizeRequest();
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002F61 File Offset: 0x00001161
		private void OnSizeAllocated(object sender, SizeAllocatedArgs args)
		{
			if (this.child != null)
			{
				this.child.Allocation = args.Allocation;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002F7C File Offset: 0x0000117C
		private void OnAdded(object sender, AddedArgs args)
		{
			this.child = args.Widget;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002F8A File Offset: 0x0000118A
		public void SetUiManager(UIManager uim)
		{
			this.uimanager = uim;
			this.child.Realized += this.OnRealized;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002FAC File Offset: 0x000011AC
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

		// Token: 0x04000027 RID: 39
		private Widget child;

		// Token: 0x04000028 RID: 40
		private UIManager uimanager;
	}
}
