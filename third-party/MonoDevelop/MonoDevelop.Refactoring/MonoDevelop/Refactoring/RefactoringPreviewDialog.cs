using System;
using System.Collections.Generic;
using System.IO;
using Gdk;
using Gtk;
using Mono.TextEditor;
using Mono.TextEditor.Utils;
using Mono.Unix;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Fonts;
using MonoDevelop.Ide.Gui;
using Pango;
using Stetic;
using Xwt.Drawing;

namespace MonoDevelop.Refactoring
{
	public class RefactoringPreviewDialog : Dialog
	{
		private class CellRendererDiff : CellRendererText
		{
			private Pango.Layout layout;

			private bool diffMode;

			private int width;

			private int height;

			private int lineHeight;

			private string[] lines;

			private bool isDisposed;

			private void DisposeLayout()
			{
				if (layout != null)
				{
					layout.Dispose();
					layout = null;
				}
			}

			protected override void OnDestroyed()
			{
				isDisposed = true;
				DisposeLayout();
				base.OnDestroyed();
			}

			public void Reset()
			{
			}

			public void InitCell(Widget container, bool diffMode, string text, string path)
			{
				if (isDisposed)
				{
					return;
				}
				this.diffMode = diffMode;
				if (diffMode)
				{
					if (text.Length > 0)
					{
						lines = text.Split('\n');
						int num = -1;
						int num2 = -1;
						for (int i = 0; i < lines.Length; i++)
						{
							if (lines[i].Length > num)
							{
								num = lines[i].Length;
								num2 = i;
							}
						}
						DisposeLayout();
						CreateLayout(container, lines[num2]);
						layout.GetPixelSize(out width, out lineHeight);
						height = lineHeight * lines.Length;
					}
					else
					{
						width = (height = 0);
					}
				}
				else
				{
					DisposeLayout();
					CreateLayout(container, text);
					layout.GetPixelSize(out width, out height);
				}
			}

			private void CreateLayout(Widget container, string text)
			{
				layout = new Pango.Layout(container.PangoContext);
				layout.SingleParagraphMode = false;
				if (diffMode)
				{
					layout.FontDescription = FontService.MonospaceFont;
					layout.SetText(text);
				}
				else
				{
					layout.SetMarkup(text);
				}
			}

			protected override void Render(Drawable window, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
			{
				if (isDisposed)
				{
					return;
				}
				try
				{
					if (diffMode)
					{
						window.GetSize(out var _, out var num2);
						int num3 = cell_area.Y;
						int num4 = cell_area.Height - 1;
						if (num3 < 0)
						{
							num4 += num3 + 1;
							num3 = -1;
						}
						if (num4 > num2 + 2)
						{
							num4 = num2 + 2;
						}
						window.DrawRectangle(widget.Style.BaseGC(StateType.Normal), filled: true, cell_area.X, num3, cell_area.Width - 1, num4);
						Gdk.GC gC = widget.Style.TextGC(StateType.Normal);
						Gdk.GC gC2 = new Gdk.GC(window);
						gC2.Copy(gC);
						gC2.RgbFgColor = new Gdk.Color(byte.MaxValue, 0, 0);
						Gdk.GC gC3 = new Gdk.GC(window);
						gC3.Copy(gC);
						gC3.RgbFgColor = new Gdk.Color(0, 0, byte.MaxValue);
						Gdk.GC gC4 = new Gdk.GC(window);
						gC4.Copy(gC);
						gC4.RgbFgColor = new Gdk.Color(165, 42, 42);
						int num5 = cell_area.Y + 2;
						int num6 = 0;
						while (num6 < lines.Length)
						{
							if (num5 + lineHeight >= 0)
							{
								if (num5 > num2)
								{
									break;
								}
								string text = lines[num6];
								if (text.Length != 0)
								{
									Gdk.GC gc;
									switch (text[0])
									{
									case '-':
										gc = gC2;
										break;
									case '+':
										gc = gC3;
										break;
									case '@':
										gc = gC4;
										break;
									default:
										gc = gC;
										break;
									}
									layout.SetText(text);
									window.DrawLayout(gc, cell_area.X + 2, num5, layout);
								}
							}
							num6++;
							num5 += lineHeight;
						}
						window.DrawRectangle(widget.Style.DarkGC(StateType.Prelight), filled: false, cell_area.X, num3, cell_area.Width - 1, num4);
						gC2.Dispose();
						gC3.Dispose();
						gC4.Dispose();
					}
					else
					{
						int y = cell_area.Y + (cell_area.Height - height) / 2;
						window.DrawLayout(widget.Style.TextGC(GetState(flags)), cell_area.X, y, layout);
					}
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
				}
			}

			public override void GetSize(Widget widget, ref Gdk.Rectangle cell_area, out int x_offset, out int y_offset, out int c_width, out int c_height)
			{
				x_offset = (y_offset = 0);
				c_width = width;
				c_height = height;
				if (diffMode)
				{
					c_width += 4;
					c_height += 4;
				}
			}

			private StateType GetState(CellRendererState flags)
			{
				if ((flags & CellRendererState.Selected) != 0)
				{
					return StateType.Selected;
				}
				return StateType.Normal;
			}
		}

		private const int pixbufColumn = 0;

		private const int textColumn = 1;

		private const int objColumn = 2;

		private const int statusVisibleColumn = 3;

		private TreeStore store = new TreeStore(typeof(Xwt.Drawing.Image), typeof(string), typeof(object), typeof(bool));

		private List<Change> changes;

		private Dictionary<string, TreeIter> fileDictionary = new Dictionary<string, TreeIter>();

		private VBox vbox2;

		private Label label1;

		private ScrolledWindow GtkScrolledWindow;

		private TreeView treeviewPreview;

		private Button buttonCancel;

		private Button buttonOk;

		public RefactoringPreviewDialog(List<Change> changes)
		{
			RefactoringPreviewDialog refactoringPreviewDialog = this;
			Build();
			this.changes = changes;
			treeviewPreview.Model = store;
			TreeViewColumn treeViewColumn = new TreeViewColumn();
			CellRendererImage cell = new CellRendererImage();
			treeViewColumn.PackStart(cell, expand: false);
			treeViewColumn.SetAttributes(cell, "image", 0);
			treeViewColumn.AddAttribute(cell, "visible", 3);
			CellRendererText cell2 = new CellRendererText();
			treeViewColumn.PackStart(cell2, expand: false);
			treeViewColumn.SetAttributes(cell2, "text", 1);
			treeViewColumn.AddAttribute(cell2, "visible", 3);
			CellRendererText cellRendererText = new CellRendererText();
			treeViewColumn.PackStart(cellRendererText, expand: false);
			treeViewColumn.SetCellDataFunc(cellRendererText, SetLocationTextData);
			CellRendererDiff cellRendererDiff = new CellRendererDiff();
			treeViewColumn.PackStart(cellRendererDiff, expand: true);
			treeViewColumn.SetCellDataFunc(cellRendererDiff, SetDiffCellData);
			treeviewPreview.AppendColumn(treeViewColumn);
			treeviewPreview.HeadersVisible = false;
			Button button = buttonCancel;
			EventHandler value = delegate
			{
				Destroy();
			};
			button.Clicked += value;
			buttonOk.Clicked += delegate
			{
				IProgressMonitor backgroundProgressMonitor = IdeApp.Workbench.ProgressMonitors.GetBackgroundProgressMonitor(refactoringPreviewDialog.Title, null);
				RefactoringService.AcceptChanges(backgroundProgressMonitor, changes);
				refactoringPreviewDialog.Destroy();
			};
			FillChanges();
		}

		private void SetLocationTextData(TreeViewColumn tree_column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			CellRendererText cellRendererText = (CellRendererText)cell;
			Change change = store.GetValue(iter, 2) as Change;
			cellRendererText.Visible = (bool)store.GetValue(iter, 3);
			if (!(change is TextReplaceChange textReplaceChange))
			{
				cellRendererText.Text = "";
				return;
			}
			TextDocument textDocument = new TextDocument();
			textDocument.Text = TextFileUtility.ReadAllText(textReplaceChange.FileName);
			DocumentLocation documentLocation = textDocument.OffsetToLocation(textReplaceChange.Offset);
			string text = string.Format(GettextCatalog.GetString("(Line:{0}, Column:{1})"), documentLocation.Line, documentLocation.Column);
			if (treeviewPreview.Selection.IterIsSelected(iter))
			{
				cellRendererText.Text = text;
				return;
			}
			Gdk.Color color = base.Style.Text(StateType.Insensitive);
			string text2 = $"#{color.Red / 256:X02}{color.Green / 256:X02}{color.Blue / 256:X02}";
			cellRendererText.Markup = "<span foreground=\"" + text2 + "\">" + text + "</span>";
		}

		private void SetDiffCellData(TreeViewColumn tree_column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			try
			{
				CellRendererDiff cellRendererDiff = (CellRendererDiff)cell;
				Change change = store.GetValue(iter, 2) as Change;
				cellRendererDiff.Visible = !(bool)store.GetValue(iter, 3);
				if (change == null || !cellRendererDiff.Visible)
				{
					cellRendererDiff.InitCell(treeviewPreview, diffMode: false, "", "");
				}
				else if (change is TextReplaceChange textReplaceChange)
				{
					Document document = IdeApp.Workbench.GetDocument(textReplaceChange.FileName);
					TextDocument textDocument = new TextDocument();
					textDocument.FileName = textReplaceChange.FileName;
					if (document == null)
					{
						textDocument.Text = TextFileUtility.ReadAllText(textReplaceChange.FileName);
					}
					else
					{
						textDocument.Text = document.Editor.Document.Text;
					}
					TextDocument textDocument2 = new TextDocument();
					textDocument2.FileName = textReplaceChange.FileName;
					textDocument2.Text = textDocument.Text;
					textDocument2.Replace(textReplaceChange.Offset, textReplaceChange.RemovedChars, textReplaceChange.InsertedText);
					string diffString = Diff.GetDiffString(textDocument, textDocument2);
					cellRendererDiff.InitCell(treeviewPreview, diffMode: true, diffString, textReplaceChange.FileName);
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}

		private TreeIter GetFile(Change change)
		{
			if (!(change is TextReplaceChange textReplaceChange))
			{
				return TreeIter.Zero;
			}
			if (!fileDictionary.TryGetValue(textReplaceChange.FileName, out var value))
			{
				value = (fileDictionary[textReplaceChange.FileName] = store.AppendValues(DesktopService.GetIconForFile(textReplaceChange.FileName, IconSize.Menu), System.IO.Path.GetFileName(textReplaceChange.FileName), null, true));
			}
			return value;
		}

		private void FillChanges()
		{
			foreach (Change change in changes)
			{
				TreeIter file = GetFile(change);
				file = ((!file.Equals(TreeIter.Zero)) ? store.AppendValues(file, ImageService.GetIcon(MonoDevelop.Ide.Gui.Stock.ReplaceIcon, IconSize.Menu), change.Description, change, true) : store.AppendValues(ImageService.GetIcon(MonoDevelop.Ide.Gui.Stock.ReplaceIcon, IconSize.Menu), change.Description, change, true));
				if (change is TextReplaceChange textReplaceChange && textReplaceChange.Offset >= 0)
				{
					store.AppendValues(file, null, null, change, false);
				}
			}
			if (changes.Count < 4)
			{
				treeviewPreview.ExpandAll();
				return;
			}
			foreach (TreeIter value in fileDictionary.Values)
			{
				treeviewPreview.ExpandRow(store.GetPath(value), open_all: false);
			}
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			base.Name = "MonoDevelop.Refactoring.RefactoringPreviewDialog";
			base.Title = Catalog.GetString("Refactoring Preview");
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.BorderWidth = 6u;
			VBox vBox = base.VBox;
			vBox.Name = "dialog1_VBox";
			vBox.BorderWidth = 2u;
			vbox2 = new VBox();
			vbox2.Name = "vbox2";
			vbox2.Spacing = 6;
			vbox2.BorderWidth = 6u;
			label1 = new Label();
			label1.Name = "label1";
			label1.Xalign = 0f;
			label1.LabelProp = Catalog.GetString("List of changes for this refactoring:");
			vbox2.Add(label1);
			Box.BoxChild boxChild = (Box.BoxChild)vbox2[label1];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			GtkScrolledWindow = new ScrolledWindow();
			GtkScrolledWindow.Name = "GtkScrolledWindow";
			GtkScrolledWindow.ShadowType = ShadowType.In;
			treeviewPreview = new TreeView();
			treeviewPreview.CanFocus = true;
			treeviewPreview.Name = "treeviewPreview";
			GtkScrolledWindow.Add(treeviewPreview);
			vbox2.Add(GtkScrolledWindow);
			Box.BoxChild boxChild2 = (Box.BoxChild)vbox2[GtkScrolledWindow];
			boxChild2.Position = 1;
			vBox.Add(vbox2);
			Box.BoxChild boxChild3 = (Box.BoxChild)vBox[vbox2];
			boxChild3.Position = 0;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5u;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			buttonCancel = new Button();
			buttonCancel.CanDefault = true;
			buttonCancel.CanFocus = true;
			buttonCancel.Name = "buttonCancel";
			buttonCancel.UseStock = true;
			buttonCancel.UseUnderline = true;
			buttonCancel.Label = "gtk-cancel";
			AddActionWidget(buttonCancel, -6);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[buttonCancel];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			buttonOk = new Button();
			buttonOk.CanDefault = true;
			buttonOk.CanFocus = true;
			buttonOk.Name = "buttonOk";
			buttonOk.UseStock = true;
			buttonOk.UseUnderline = true;
			buttonOk.Label = "gtk-ok";
			AddActionWidget(buttonOk, -5);
			ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[buttonOk];
			buttonBoxChild2.Position = 1;
			buttonBoxChild2.Expand = false;
			buttonBoxChild2.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 400;
			base.DefaultHeight = 300;
			Hide();
		}
	}
}
