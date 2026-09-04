using Gtk;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide.TextEditing;
using Xwt;
using Xwt.Drawing;

namespace MonoDevelop.Debugger
{
	internal class ExceptionCaughtMiniButton : TopLevelWidgetExtension
	{
		private readonly ExceptionCaughtMessage dlg;

		public ExceptionCaughtMiniButton(ExceptionCaughtMessage dlg, FilePath file, int line)
		{
			this.dlg = dlg;
			base.OffsetX = 6;
			base.File = file;
			base.Line = line;
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
			EventBox eventBox = new EventBox();
			eventBox.VisibleWindow = false;
			Xwt.Drawing.Image image = Xwt.Drawing.Image.FromResource("lightning-16.png");
			eventBox.Add(new Xwt.ImageView(image).ToGtkWidget());
			eventBox.ButtonPressEvent += delegate
			{
				dlg.ShowButton();
			};
			PopoverWidget popoverWidget = new PopoverWidget();
			popoverWidget.Theme.Padding = 2;
			popoverWidget.ShowArrow = true;
			popoverWidget.EnableAnimation = true;
			popoverWidget.PopupPosition = PopupPosition.Left;
			popoverWidget.ContentBox.Add(eventBox);
			popoverWidget.ShowAll();
			return popoverWidget;
		}
	}
}
