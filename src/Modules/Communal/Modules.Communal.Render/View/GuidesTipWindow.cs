using System;
using Cairo;
using Gdk;
using Gtk;
using MonoDevelop.Components;

namespace Modules.Communal.Render.View
{
	// Token: 0x02000035 RID: 53
	internal class GuidesTipWindow : PopoverWindow
	{
		// Token: 0x06000266 RID: 614 RVA: 0x0000D484 File Offset: 0x0000B684
		public GuidesTipWindow()
		{
			base.Theme.SetFlatColor(new Cairo.Color(0.0784313725490196, 0.0784313725490196, 0.0784313725490196));
			base.Theme.BorderColor = new Cairo.Color(0.3215686274509804, 0.3215686274509804, 0.3215686274509804);
			base.ShowArrow = true;
			this.Initialize();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000D504 File Offset: 0x0000B704
		private void Initialize()
		{
			Alignment alignment = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.label = new Label();
			alignment.Add(this.label);
			base.ContentBox.Add(alignment);
			alignment.ShowAll();
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000D558 File Offset: 0x0000B758
		// (set) Token: 0x06000269 RID: 617 RVA: 0x0000D575 File Offset: 0x0000B775
		public string Text
		{
			get
			{
				return this.label.Text;
			}
			set
			{
				this.label.Text = value;
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000D588 File Offset: 0x0000B788
		public void MoveWindow(Widget parent, Gdk.Window window, double screenX, double screenY)
		{
			int num;
			int num2;
			window.GetOrigin(out num, out num2);
			Gdk.Point point = new Gdk.Point(num + (int)screenX, num2 + (int)screenY);
			base.Move(point.X + 10, point.Y + 10);
		}

		// Token: 0x040000AC RID: 172
		private Label label;
	}
}
