using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000F RID: 15
	internal class BinContainer
	{
		// Token: 0x0600007A RID: 122 RVA: 0x00004AE8 File Offset: 0x00002CE8
		public static BinContainer Attach(Bin bin)
		{
			BinContainer binContainer = new BinContainer();
			bin.SizeRequested += binContainer.OnSizeRequested;
			bin.SizeAllocated += binContainer.OnSizeAllocated;
			bin.Added += binContainer.OnAdded;
			return binContainer;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004B32 File Offset: 0x00002D32
		private void OnSizeRequested(object sender, SizeRequestedArgs args)
		{
			if (this.child != null)
			{
				args.Requisition = this.child.SizeRequest();
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004B4D File Offset: 0x00002D4D
		private void OnSizeAllocated(object sender, SizeAllocatedArgs args)
		{
			if (this.child != null)
			{
				this.child.Allocation = args.Allocation;
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004B68 File Offset: 0x00002D68
		private void OnAdded(object sender, AddedArgs args)
		{
			this.child = args.Widget;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004B76 File Offset: 0x00002D76
		public void SetUiManager(UIManager uim)
		{
			this.uimanager = uim;
			this.child.Realized += this.OnRealized;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004B98 File Offset: 0x00002D98
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

		// Token: 0x04000047 RID: 71
		private Widget child;

		// Token: 0x04000048 RID: 72
		private UIManager uimanager;
	}
}
