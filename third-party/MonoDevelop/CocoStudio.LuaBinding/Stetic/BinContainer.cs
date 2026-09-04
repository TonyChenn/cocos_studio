using System;
using Gtk;

namespace Stetic
{
	internal class BinContainer
	{
		private Widget child;

		private UIManager uimanager;

		public static Stetic.BinContainer Attach(Bin bin)
		{
			Stetic.BinContainer binContainer = new Stetic.BinContainer();
			bin.SizeRequested += binContainer.OnSizeRequested;
			bin.SizeAllocated += binContainer.OnSizeAllocated;
			bin.Added += binContainer.OnAdded;
			return binContainer;
		}

		private void OnSizeRequested(object sender, SizeRequestedArgs args)
		{
			if (child != null)
			{
				args.Requisition = child.SizeRequest();
			}
		}

		private void OnSizeAllocated(object sender, SizeAllocatedArgs args)
		{
			if (child != null)
			{
				child.Allocation = args.Allocation;
			}
		}

		private void OnAdded(object sender, AddedArgs args)
		{
			child = args.Widget;
		}

		public void SetUiManager(UIManager uim)
		{
			uimanager = uim;
			child.Realized += OnRealized;
		}

		private void OnRealized(object sender, EventArgs args)
		{
			if (uimanager != null)
			{
				Widget toplevel = child.Toplevel;
				if (toplevel != null && typeof(Window).IsInstanceOfType(toplevel))
				{
					((Window)toplevel).AddAccelGroup(uimanager.AccelGroup);
					uimanager = null;
				}
			}
		}
	}
}
