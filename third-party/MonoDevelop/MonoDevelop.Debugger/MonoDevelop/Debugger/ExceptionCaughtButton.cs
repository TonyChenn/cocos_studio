using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.TextEditing;
using Xwt;
using Xwt.Drawing;

namespace MonoDevelop.Debugger
{
	internal class ExceptionCaughtButton : TopLevelWidgetExtension
	{
		private readonly Xwt.Drawing.Image closeSelOverImage;

		private readonly Xwt.Drawing.Image closeSelImage;

		private readonly ExceptionCaughtMessage dlg;

		private readonly ExceptionInfo exception;

		private Gtk.Label messageLabel;

		public ExceptionCaughtButton(ExceptionInfo val, ExceptionCaughtMessage dlg, FilePath file, int line)
		{
			exception = val;
			this.dlg = dlg;
			base.OffsetX = 6;
			base.File = file;
			base.Line = line;
			closeSelImage = ImageService.GetIcon("md-popup-close", Gtk.IconSize.Menu);
			closeSelOverImage = ImageService.GetIcon("md-popup-close-hover", Gtk.IconSize.Menu);
		}

		protected override void OnLineChanged()
		{
			base.OnLineChanged();
			dlg.Line = base.Line;
		}

		protected override void OnLineDeleted()
		{
			base.OnLineDeleted();
			base.Line++;
		}

		public override Gtk.Widget CreateWidget()
		{
			Xwt.Drawing.Image image = Xwt.Drawing.Image.FromResource("lightning-16.png");
			Gtk.Widget child = new Xwt.ImageView(image).ToGtkWidget();
			Gtk.HBox hBox = new Gtk.HBox(homogeneous: false, 6);
			Gtk.VBox vBox = new Gtk.VBox();
			vBox.PackStart(child, expand: false, fill: false, 0u);
			hBox.PackStart(vBox, expand: false, fill: false, 0u);
			vBox = new Gtk.VBox(homogeneous: false, 6);
			vBox.PackStart(new Gtk.Label
			{
				Markup = GettextCatalog.GetString("<b>{0}</b> has been thrown", exception.Type),
				Xalign = 0f
			});
			messageLabel = new Gtk.Label
			{
				Xalign = 0f,
				NoShowAll = true
			};
			vBox.PackStart(messageLabel);
			LinkLabel linkLabel = new LinkLabel(GettextCatalog.GetString("Show Details"));
			Gtk.HBox hBox2 = new Gtk.HBox();
			linkLabel.NavigateToUrl += delegate
			{
				dlg.ShowDialog();
			};
			hBox2.PackStart(linkLabel.ToGtkWidget(), expand: false, fill: false, 0u);
			vBox.PackStart(hBox2, expand: false, fill: false, 0u);
			hBox.PackStart(vBox, expand: true, fill: true, 0u);
			vBox = new Gtk.VBox();
			ImageButton imageButton = new ImageButton();
			imageButton.InactiveImage = closeSelImage;
			imageButton.Image = closeSelOverImage;
			ImageButton imageButton2 = imageButton;
			imageButton2.Clicked += delegate
			{
				dlg.ShowMiniButton();
			};
			vBox.PackStart(imageButton2, expand: false, fill: false, 0u);
			hBox.PackStart(vBox, expand: false, fill: false, 0u);
			exception.Changed += delegate
			{
				Gtk.Application.Invoke(delegate
				{
					LoadData();
				});
			};
			LoadData();
			PopoverWidget popoverWidget = new PopoverWidget();
			popoverWidget.ShowArrow = true;
			popoverWidget.EnableAnimation = true;
			popoverWidget.PopupPosition = PopupPosition.Left;
			popoverWidget.ContentBox.Add(hBox);
			popoverWidget.ShowAll();
			return popoverWidget;
		}

		private void LoadData()
		{
			if (!string.IsNullOrEmpty(exception.Message))
			{
				messageLabel.Show();
				messageLabel.Text = exception.Message;
				if (messageLabel.SizeRequest().Width > 400)
				{
					messageLabel.WidthRequest = 400;
					messageLabel.Wrap = true;
				}
			}
			else
			{
				messageLabel.Hide();
			}
		}
	}
}
