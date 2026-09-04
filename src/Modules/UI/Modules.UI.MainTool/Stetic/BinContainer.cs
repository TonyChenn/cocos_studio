using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000014 RID: 20
	internal class BinContainer
	{
		// Token: 0x0600006F RID: 111 RVA: 0x00004110 File Offset: 0x00002310
		public static BinContainer Attach(Bin bin)
		{
			BinContainer binContainer = new BinContainer();
			bin.SizeRequested += binContainer.OnSizeRequested;
			bin.SizeAllocated += binContainer.OnSizeAllocated;
			bin.Added += binContainer.OnAdded;
			return binContainer;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004164 File Offset: 0x00002364
		private void OnSizeRequested(object sender, SizeRequestedArgs args)
		{
			if (this.child != null)
			{
				args.Requisition = this.child.SizeRequest();
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004194 File Offset: 0x00002394
		private void OnSizeAllocated(object sender, SizeAllocatedArgs args)
		{
			if (this.child != null)
			{
				this.child.Allocation = args.Allocation;
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000041C3 File Offset: 0x000023C3
		private void OnAdded(object sender, AddedArgs args)
		{
			this.child = args.Widget;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000041D2 File Offset: 0x000023D2
		public void SetUiManager(UIManager uim)
		{
			this.uimanager = uim;
			this.child.Realized += this.OnRealized;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000041F4 File Offset: 0x000023F4
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

		// Token: 0x04000031 RID: 49
		private Widget child;

		// Token: 0x04000032 RID: 50
		private UIManager uimanager;
	}
}
