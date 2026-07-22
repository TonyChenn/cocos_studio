using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000C RID: 12
	internal class BinContainer
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002B08 File Offset: 0x00000D08
		public static BinContainer Attach(Bin bin)
		{
			BinContainer binContainer = new BinContainer();
			bin.SizeRequested += binContainer.OnSizeRequested;
			bin.SizeAllocated += binContainer.OnSizeAllocated;
			bin.Added += binContainer.OnAdded;
			return binContainer;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002B5C File Offset: 0x00000D5C
		private void OnSizeRequested(object sender, SizeRequestedArgs args)
		{
			if (this.child != null)
			{
				args.Requisition = this.child.SizeRequest();
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002B8C File Offset: 0x00000D8C
		private void OnSizeAllocated(object sender, SizeAllocatedArgs args)
		{
			if (this.child != null)
			{
				this.child.Allocation = args.Allocation;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002BBB File Offset: 0x00000DBB
		private void OnAdded(object sender, AddedArgs args)
		{
			this.child = args.Widget;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002BCA File Offset: 0x00000DCA
		public void SetUiManager(UIManager uim)
		{
			this.uimanager = uim;
			this.child.Realized += this.OnRealized;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002BEC File Offset: 0x00000DEC
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

		// Token: 0x04000024 RID: 36
		private Widget child;

		// Token: 0x04000025 RID: 37
		private UIManager uimanager;
	}
}
