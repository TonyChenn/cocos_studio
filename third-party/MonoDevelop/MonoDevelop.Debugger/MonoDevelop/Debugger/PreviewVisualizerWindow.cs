using System;
using Cairo;
using Gdk;
using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Components;
using MonoDevelop.Debugger.PreviewVisualizers;
using MonoDevelop.Ide;
using Pango;

namespace MonoDevelop.Debugger
{
	public class PreviewVisualizerWindow : PopoverWindow
	{
		public PreviewVisualizerWindow(ObjectValue val, Widget invokingWidget)
			: base(Gtk.WindowType.Toplevel)
		{
			PreviewVisualizerWindow previewVisualizerWindow = this;
			base.TypeHint = WindowTypeHint.PopupMenu;
			base.Decorated = false;
			if (((Gtk.Window)invokingWidget.Toplevel).Modal)
			{
				base.Modal = true;
			}
			base.TransientFor = (Gtk.Window)invokingWidget.Toplevel;
			base.Theme.SetFlatColor(new Cairo.Color(245.0 / 256.0, 245.0 / 256.0, 245.0 / 256.0));
			base.Theme.Padding = 3;
			base.ShowArrow = true;
			VBox vBox = new VBox();
			Table table = new Table(1u, 3u, homogeneous: false)
			{
				ColumnSpacing = 5u
			};
			ImageButton imageButton = new ImageButton
			{
				InactiveImage = ImageService.GetIcon("md-popup-close", IconSize.Menu),
				Image = ImageService.GetIcon("md-popup-close-hover", IconSize.Menu)
			};
			imageButton.Clicked += delegate
			{
				Destroy();
			};
			HBox hBox = new HBox();
			VBox vBox2 = new VBox();
			hBox.PackStart(vBox2, expand: false, fill: false, 0u);
			vBox2.PackStart(imageButton, expand: false, fill: false, 0u);
			table.Attach(hBox, 0u, 1u, 0u, 1u);
			Label label = new Label();
			label.ModifyFg(StateType.Normal, new Gdk.Color(36, 36, 36));
			FontDescription fontDescription = label.Style.FontDescription.Copy();
			fontDescription.Weight = Weight.Bold;
			label.ModifyFont(fontDescription);
			label.Text = val.TypeName;
			VBox vBox3 = new VBox();
			vBox3.PackStart(label, expand: false, fill: false, 3u);
			table.Attach(vBox3, 1u, 2u, 0u, 1u);
			if (DebuggingService.HasValueVisualizers(val))
			{
				Button button = new Button();
				button.Label = "Open";
				button.Relief = ReliefStyle.Half;
				button.Clicked += delegate
				{
					PreviewWindowManager.DestroyWindow();
					DebuggingService.ShowValueVisualizer(val);
				};
				HBox hBox2 = new HBox();
				hBox2.PackEnd(button, expand: false, fill: false, 2u);
				table.Attach(hBox2, 2u, 3u, 0u, 1u);
			}
			else
			{
				table.Attach(new Label(), 2u, 3u, 0u, 1u, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Expand | AttachOptions.Fill, 10u, 0u);
			}
			vBox.PackStart(table);
			vBox.ShowAll();
			PreviewVisualizer previewVisualizer = DebuggingService.GetPreviewVisualizer(val);
			if (previewVisualizer == null)
			{
				previewVisualizer = new GenericPreviewVisualizer();
			}
			Control control = null;
			try
			{
				control = previewVisualizer.GetVisualizerWidget(val);
			}
			catch (Exception ex)
			{
				DebuggingService.DebuggerSession.LogWriter(isStderr: true, "Exception during preview widget creation: " + ex.Message);
			}
			if (control == null)
			{
				control = new GenericPreviewVisualizer().GetVisualizerWidget(val);
			}
			Gtk.Alignment alignment = new Gtk.Alignment(0f, 0f, 1f, 1f);
			alignment.SetPadding(3u, 5u, 5u, 5u);
			alignment.Show();
			alignment.Add(control);
			vBox.PackStart(alignment);
			base.ContentBox.Add(vBox);
		}
	}
}
