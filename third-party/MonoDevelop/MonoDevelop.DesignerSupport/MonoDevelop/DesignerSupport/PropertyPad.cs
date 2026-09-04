using Gtk;
using MonoDevelop.Components.Commands;
using MonoDevelop.Components.Docking;
using MonoDevelop.Components.PropertyGrid;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.DesignerSupport
{
	public class PropertyPad : AbstractPadContent, ICommandDelegator
	{
		private PropertyGrid grid;

		private InvisibleFrame frame;

		private bool customWidget;

		private IPadWindow container;

		private DockToolbarProvider toolbarProvider = new DockToolbarProvider();

		internal object CommandRouteOrigin { get; set; }

		internal IPadWindow PadWindow => container;

		public override Widget Control => frame;

		internal PropertyGrid PropertyGrid
		{
			get
			{
				if (customWidget)
				{
					customWidget = false;
					frame.Remove(frame.Child);
					frame.Add(grid);
					toolbarProvider.Attach(container.GetToolbar(PositionType.Top));
				}
				return grid;
			}
		}

		public PropertyPad()
		{
			grid = new PropertyGrid();
			frame = new InvisibleFrame();
			frame.Add(grid);
			frame.ShowAll();
		}

		public override void Initialize(IPadWindow container)
		{
			base.Initialize(container);
			toolbarProvider.Attach(container.GetToolbar(PositionType.Top));
			grid.SetToolbarProvider(toolbarProvider);
			this.container = container;
			DesignerSupport.Service.SetPad(this);
		}

		public override void Dispose()
		{
			DesignerSupport.Service.SetPad(null);
		}

		object ICommandDelegator.GetDelegatedCommandTarget()
		{
			if (IdeApp.CommandService.CurrentCommand == IdeApp.CommandService.GetCommand(FileCommands.Save))
			{
				return CommandRouteOrigin;
			}
			return null;
		}

		public void BlankPad()
		{
			PropertyGrid.CurrentObject = null;
			CommandRouteOrigin = null;
		}

		internal void UseCustomWidget(Widget widget)
		{
			toolbarProvider.Attach(null);
			ClearToolbar();
			customWidget = true;
			frame.Remove(frame.Child);
			frame.Add(widget);
			widget.Show();
		}

		private void ClearToolbar()
		{
			if (container != null)
			{
				DockItemToolbar toolbar = container.GetToolbar(PositionType.Top);
				Widget[] children = toolbar.Children;
				foreach (Widget widget in children)
				{
					toolbar.Remove(widget);
				}
			}
		}
	}
}
