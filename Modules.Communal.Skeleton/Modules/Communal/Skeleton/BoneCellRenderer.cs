using System;
using CocoStudio.Model.ViewModel;
using Gdk;
using GLib;
using Gtk;
using Pango;

namespace Modules.Communal.Skeleton
{
	// Token: 0x0200000E RID: 14
	public class BoneCellRenderer : CellRendererText
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000033BC File Offset: 0x000015BC
		// (set) Token: 0x06000051 RID: 81 RVA: 0x000033C4 File Offset: 0x000015C4
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

		// Token: 0x06000052 RID: 82 RVA: 0x000033D0 File Offset: 0x000015D0
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

		// Token: 0x06000053 RID: 83 RVA: 0x00003450 File Offset: 0x00001650
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

		// Token: 0x0400000E RID: 14
		private BoneObject bone;

		// Token: 0x0400000F RID: 15
		private Pango.Layout layout;
	}
}
