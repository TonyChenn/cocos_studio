using System;
using System.Collections.Generic;
using Gdk;
using Gtk;
using MonoDevelop.Components.Docking;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using Pango;

namespace MonoDevelop.DesignerSupport
{
	public class DocumentOutlinePad : AbstractPadContent
	{
		private class WrappedCentreLabel : Widget
		{
			private string text;

			private Pango.Layout layout;

			public string Text
			{
				get
				{
					return text;
				}
				set
				{
					text = value;
					UpdateLayout();
				}
			}

			public WrappedCentreLabel()
			{
				base.WidgetFlags |= WidgetFlags.NoWindow;
			}

			public WrappedCentreLabel(string text)
				: this()
			{
				Text = text;
			}

			private void CreateLayout()
			{
				if (layout != null)
				{
					layout.Dispose();
				}
				layout = new Pango.Layout(base.PangoContext);
				layout.Wrap = Pango.WrapMode.Word;
			}

			private void UpdateLayout()
			{
				if (layout == null)
				{
					CreateLayout();
				}
				layout.Alignment = Pango.Alignment.Center;
				layout.SetText(text);
			}

			protected override bool OnExposeEvent(EventExpose evnt)
			{
				if (evnt.Window != base.GdkWindow || layout == null)
				{
					return base.OnExposeEvent(evnt);
				}
				layout.Width = (int)((double)(base.Allocation.Width * 2 / 3) * Pango.Scale.PangoScale);
				Gtk.Style.PaintLayout(base.Style, base.GdkWindow, base.State, use_text: false, evnt.Area, this, null, base.Allocation.Width / 6 + base.Allocation.X, 12 + base.Allocation.Y, layout);
				return true;
			}

			protected override void OnStyleSet(Gtk.Style previous_style)
			{
				CreateLayout();
				UpdateLayout();
				base.OnStyleSet(previous_style);
			}

			public override void Dispose()
			{
				if (layout != null)
				{
					layout.Dispose();
					layout = null;
				}
				base.Dispose();
			}
		}

		private Gtk.Alignment box;

		private IOutlinedDocument currentOutlineDoc;

		private Document currentDoc;

		private DockItemToolbar toolbar;

		private Document CurrentDoc
		{
			get
			{
				return currentDoc;
			}
			set
			{
				if (value != currentDoc)
				{
					if (currentDoc != null)
					{
						currentDoc.ViewChanged -= ViewChangedHandler;
					}
					currentDoc = value;
					if (currentDoc != null)
					{
						currentDoc.ViewChanged += ViewChangedHandler;
					}
				}
			}
		}

		public override Widget Control => box;

		public DocumentOutlinePad()
		{
			box = new Gtk.Alignment(0f, 0f, 1f, 1f);
			box.BorderWidth = 0u;
			SetWidget(null);
			box.ShowAll();
		}

		public override void Initialize(IPadWindow window)
		{
			base.Initialize(window);
			IdeApp.Workbench.ActiveDocumentChanged += DocumentChangedHandler;
			CurrentDoc = IdeApp.Workbench.ActiveDocument;
			toolbar = window.GetToolbar(PositionType.Top);
			toolbar.Visible = false;
			Update();
		}

		public override void Dispose()
		{
			IdeApp.Workbench.ActiveDocumentChanged -= DocumentChangedHandler;
			CurrentDoc = null;
			ReleaseDoc();
			base.Dispose();
		}

		private void ViewChangedHandler(object sender, EventArgs args)
		{
			Update();
		}

		private void DocumentChangedHandler(object sender, EventArgs args)
		{
			CurrentDoc = IdeApp.Workbench.ActiveDocument;
			Update();
		}

		private void Update()
		{
			IOutlinedDocument outlinedDocument = null;
			if (CurrentDoc != null)
			{
				outlinedDocument = CurrentDoc.GetContent<IOutlinedDocument>();
			}
			if (currentOutlineDoc == outlinedDocument)
			{
				return;
			}
			ReleaseDoc();
			currentOutlineDoc = outlinedDocument;
			Widget widget = null;
			IEnumerable<Widget> toolbarWidgets = null;
			if (outlinedDocument != null)
			{
				widget = outlinedDocument.GetOutlineWidget();
				if (widget != null)
				{
					toolbarWidgets = outlinedDocument.GetToolbarWidgets();
				}
			}
			SetWidget(widget);
			SetToolbarWidgets(toolbarWidgets);
		}

		private void ReleaseDoc()
		{
			RemoveBoxChild();
			if (currentOutlineDoc != null)
			{
				currentOutlineDoc.ReleaseOutlineWidget();
			}
			currentOutlineDoc = null;
		}

		private void SetWidget(Widget widget)
		{
			if (widget == null)
			{
				widget = new WrappedCentreLabel(GettextCatalog.GetString("An outline is not available for the current document."));
			}
			RemoveBoxChild();
			box.Add(widget);
			widget.Show();
			box.Show();
		}

		private void SetToolbarWidgets(IEnumerable<Widget> toolbarWidgets)
		{
			Widget[] children = toolbar.Children;
			foreach (Widget widget in children)
			{
				toolbar.Remove(widget);
			}
			bool visible = false;
			if (toolbarWidgets != null)
			{
				foreach (Widget toolbarWidget in toolbarWidgets)
				{
					toolbarWidget.Show();
					toolbar.Add(toolbarWidget);
					visible = true;
				}
			}
			toolbar.Visible = visible;
		}

		private void RemoveBoxChild()
		{
			Widget child = box.Child;
			if (child != null)
			{
				box.Remove(child);
			}
		}
	}
}
