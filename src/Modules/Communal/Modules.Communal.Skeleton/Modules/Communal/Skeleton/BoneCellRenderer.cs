using System;
using CocoStudio.Model.ViewModel;
using Gdk;
using GLib;
using Gtk;
using Pango;

namespace Modules.Communal.Skeleton
{
	public class BoneCellRenderer : CellRendererText
	{
		[Property("bone")]
		public BoneObject Bone
		{
			get
			{
				return this.bone;
			}
			set
			{
				this.bone = value;
			}
		}

		protected override void Render(Drawable window, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
		{
			if (this.bone != null)
			{
				StateType stateType = this.GetStateType(widget, flags);
				string name = this.bone.Name;
				this.layout = new Pango.Layout(widget.PangoContext);
				this.layout.SetMarkup(name);
				window.DrawLayout(widget.Style.TextGC(stateType), cell_area.X, cell_area.Y, this.layout);
			}
			base.Render(window, widget, background_area, cell_area, expose_area, flags);
		}

		private StateType GetStateType(Widget widget, CellRendererState flags)
		{
			StateType result = StateType.Normal;
			if ((flags & CellRendererState.Prelit) != (CellRendererState)0)
			{
				result = StateType.Prelight;
			}
			if ((flags & CellRendererState.Focused) != (CellRendererState)0)
			{
				result = StateType.Normal;
			}
			if ((flags & CellRendererState.Insensitive) != (CellRendererState)0)
			{
				result = StateType.Insensitive;
			}
			if ((flags & CellRendererState.Selected) != (CellRendererState)0)
			{
				result = (widget.HasFocus ? StateType.Selected : StateType.Active);
			}
			return result;
		}

		private BoneObject bone;

		private Pango.Layout layout;
	}
}
