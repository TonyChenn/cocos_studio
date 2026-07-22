using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000005 RID: 5
	internal class BinContainer
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000021A4 File Offset: 0x000003A4
		public static BinContainer Attach(Bin bin)
		{
			BinContainer binContainer = new BinContainer();
			bin.SizeRequested += binContainer.OnSizeRequested;
			bin.SizeAllocated += binContainer.OnSizeAllocated;
			bin.Added += binContainer.OnAdded;
			return binContainer;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021EE File Offset: 0x000003EE
		private void OnSizeRequested(object sender, SizeRequestedArgs args)
		{
			if (this.child != null)
			{
				args.Requisition = this.child.SizeRequest();
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002209 File Offset: 0x00000409
		private void OnSizeAllocated(object sender, SizeAllocatedArgs args)
		{
			if (this.child != null)
			{
				this.child.Allocation = args.Allocation;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002224 File Offset: 0x00000424
		private void OnAdded(object sender, AddedArgs args)
		{
			this.child = args.Widget;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002232 File Offset: 0x00000432
		public void SetUiManager(UIManager uim)
		{
			this.uimanager = uim;
			this.child.Realized += this.OnRealized;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002254 File Offset: 0x00000454
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

		// Token: 0x04000003 RID: 3
		private Widget child;

		// Token: 0x04000004 RID: 4
		private UIManager uimanager;
	}
}
