using System.ComponentModel;
using Gdk;
using Gtk;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	public interface IToolboxConsumer
	{
		TargetEntry[] DragTargets { get; }

		ToolboxItemFilterAttribute[] ToolboxFilterAttributes { get; }

		string DefaultItemDomain { get; }

		void ConsumeItem(ItemToolboxNode item);

		void DragItem(ItemToolboxNode item, Widget source, DragContext ctx);

		bool CustomFilterSupports(ItemToolboxNode item);
	}
}
