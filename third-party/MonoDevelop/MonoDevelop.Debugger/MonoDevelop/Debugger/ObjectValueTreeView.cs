using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cairo;
using GLib;
using Gdk;
using Gtk;
using Mono.Debugging.Client;
using Mono.TextEditor;
using MonoDevelop.Components;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui;
using Pango;
using Xwt;
using Xwt.Drawing;

namespace MonoDevelop.Debugger
{
	[ToolboxItem(true)]
	public class ObjectValueTreeView : Gtk.TreeView, ICompletionWidget
	{
		private enum LocalCommands
		{
			AddWatch
		}

		private class CellRendererTextWithIcon : CellRendererText
		{
			private IconId icon;

			[Property("icon")]
			public string Icon
			{
				get
				{
					return icon;
				}
				set
				{
					icon = value;
				}
			}

			private Xwt.Drawing.Image img => ImageService.GetIcon(icon, Gtk.IconSize.Menu);

			public override void GetSize(Gtk.Widget widget, ref Gdk.Rectangle cell_area, out int x_offset, out int y_offset, out int width, out int height)
			{
				base.GetSize(widget, ref cell_area, out x_offset, out y_offset, out width, out height);
				if (!icon.IsNull)
				{
					width += (int)((double)(base.Xpad * 2) + img.Width);
				}
			}

			protected override void Render(Drawable window, Gtk.Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
			{
				base.Render(window, widget, background_area, cell_area, expose_area, flags);
				if (icon.IsNull)
				{
					return;
				}
				using (Cairo.Context s = Gdk.CairoHelper.Create(window))
				{
					using (Pango.Layout layout = new Pango.Layout(widget.PangoContext))
					{
						layout.FontDescription = base.FontDesc.Copy();
						layout.FontDescription.Family = base.Family;
						layout.SetText(base.Text);
						layout.GetPixelSize(out var width, out var _);
						long num = cell_area.X + width + 3 * base.Xpad;
						int num2 = cell_area.Y + cell_area.Height / 2 - (int)(img.Height / 2.0);
						s.DrawImage(widget, img, num, num2);
					}
				}
			}
		}

		private class CellRendererTextUrl : CellRendererText
		{
			[Property("texturl")]
			public string TextUrl
			{
				get
				{
					return base.Text;
				}
				set
				{
					if (value != null && Uri.TryCreate(value.Trim('"', '{', '}'), UriKind.Absolute, out var result) && (result.Scheme == "http" || result.Scheme == "https"))
					{
						base.Underline = Underline.Single;
						base.Foreground = "#197CEF";
					}
					else
					{
						base.Underline = Underline.None;
					}
					base.Text = value;
				}
			}
		}

		private class CellRendererColorPreview : CellRenderer
		{
			public Xwt.Drawing.Color Color { get; set; }

			protected override void Render(Drawable window, Gtk.Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
			{
				Xwt.Drawing.Color color = Color.WithIncreasedLight(-0.15);
				using (Cairo.Context context = Gdk.CairoHelper.Create(window))
				{
					double xc = (double)cell_area.X + Math.Round((double)cell_area.Width / 2.0);
					double yc = (double)cell_area.Y + Math.Round((double)cell_area.Height / 2.0);
					context.LineWidth = 1.0;
					context.Arc(xc, yc, 5.5, 0.0, Math.PI * 2.0);
					context.SetSourceRGBA(Color.Red, Color.Green, Color.Blue, 1.0);
					context.FillPreserve();
					context.SetSourceRGBA(color.Red, color.Green, color.Blue, 1.0);
					context.Stroke();
				}
			}

			public override void GetSize(Gtk.Widget widget, ref Gdk.Rectangle cell_area, out int x_offset, out int y_offset, out int width, out int height)
			{
				x_offset = (y_offset = 0);
				height = (width = 16);
			}
		}

		private class CellRendererRoundedButton : CellRendererText
		{
			private const int TopBottomPadding = 1;

			protected override void Render(Drawable window, Gtk.Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
			{
				if (string.IsNullOrEmpty(base.Text))
				{
					return;
				}
				using (Cairo.Context context = Gdk.CairoHelper.Create(window))
				{
					using (Pango.Layout layout = new Pango.Layout(widget.PangoContext))
					{
						layout.SetText(base.Text);
						layout.FontDescription = base.FontDesc;
						layout.FontDescription.Family = base.Family;
						layout.GetPixelSize(out var width, out var height);
						int xpad = (int)base.Xpad;
						context.RoundedRectangle((double)(cell_area.X + xpad) + 0.5, (double)(cell_area.Y + 1) + 0.5, width + (cell_area.Height - 2) - 1, cell_area.Height - 2 - 1, (cell_area.Height - 2) / 2);
						context.LineWidth = 1.0;
						context.SetSourceRGB(233.0 / 255.0, 242.0 / 255.0, 84.0 / 85.0);
						context.FillPreserve();
						context.SetSourceRGB(82.0 / 255.0, 148.0 / 255.0, 47.0 / 51.0);
						context.Stroke();
						int num = (cell_area.Height - height) / 2;
						if (((ObjectValueTreeView)widget).CompactView && !Platform.IsWindows)
						{
							num++;
						}
						window.DrawLayoutWithColors(widget.Style.TextGC(StateType.Normal), cell_area.X + (cell_area.Height - 2 + 1) / 2 + xpad, cell_area.Y + num, layout, new Gdk.Color(82, 148, 235), new Gdk.Color(233, 242, 252));
					}
				}
			}

			public override void GetSize(Gtk.Widget widget, ref Gdk.Rectangle cell_area, out int x_offset, out int y_offset, out int width, out int height)
			{
				base.GetSize(widget, ref cell_area, out x_offset, out y_offset, out width, out height);
				x_offset = (y_offset = 0);
				if (string.IsNullOrEmpty(base.Text))
				{
					width = 0;
					height = 0;
					return;
				}
				using (Pango.Layout layout = new Pango.Layout(widget.PangoContext))
				{
					layout.SetText(base.Text);
					layout.FontDescription = base.FontDesc;
					layout.FontDescription.Family = base.Family;
					layout.GetPixelSize(out var width2, out var _);
					width = width2 + (height - 2) + (int)(2 * base.Xpad);
				}
			}
		}

		private enum PreviewButtonIcons
		{
			None,
			Hidden,
			RowHover,
			Hover,
			Active,
			Selected
		}

		private const string errorColor = "red";

		private const string modifiedColor = "blue";

		private const string disabledColor = "gray";

		private const int NameColumn = 0;

		private const int ValueColumn = 1;

		private const int TypeColumn = 2;

		public const int ObjectColumn = 3;

		private const int NameEditableColumn = 4;

		private const int ValueEditableColumn = 5;

		private const int IconColumn = 6;

		private const int NameColorColumn = 7;

		private const int ValueColorColumn = 8;

		private const int ValueButtonVisibleColumn = 9;

		private const int PinIconColumn = 10;

		private const int LiveUpdateIconColumn = 11;

		private const int ViewerButtonVisibleColumn = 12;

		private const int PreviewIconColumn = 13;

		private const int EvaluateStatusIconColumn = 14;

		private const int EvaluateStatusIconVisibleColumn = 15;

		private const int ValueButtonTextColumn = 16;

		private readonly Dictionary<ObjectValue, TreeRowReference> nodes = new Dictionary<ObjectValue, TreeRowReference>();

		private readonly Dictionary<string, ObjectValue> cachedValues = new Dictionary<string, ObjectValue>();

		private readonly Dictionary<ObjectValue, Task> expandTasks = new Dictionary<ObjectValue, Task>();

		private readonly List<ObjectValue> enumerableLoading = new List<ObjectValue>();

		private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		private readonly Dictionary<string, string> oldValues = new Dictionary<string, string>();

		private readonly List<ObjectValue> values = new List<ObjectValue>();

		private readonly List<string> valueNames = new List<string>();

		private readonly Xwt.Drawing.Image noLiveIcon;

		private readonly Xwt.Drawing.Image liveIcon;

		private readonly TreeViewState state;

		private readonly Gtk.TreeStore store;

		private readonly string createMsg;

		private bool restoringState;

		private bool compact;

		private StackFrame frame;

		private bool disposed;

		private bool columnsAdjusted;

		private bool columnSizesUpdating;

		private bool allowStoreColumnSizes;

		private double expColWidth;

		private double valueColWidth;

		private double typeColWidth;

		private readonly CellRendererTextWithIcon crtExp;

		private readonly CellRendererText crtValue;

		private readonly CellRendererText crtType;

		private readonly CellRendererRoundedButton crpButton;

		private readonly CellRendererImage crpPin;

		private readonly CellRendererImage crpLiveUpdate;

		private readonly CellRendererImage crpViewer;

		private Entry editEntry;

		private Mono.Debugging.Client.CompletionData currentCompletionData;

		private readonly TreeViewColumn expCol;

		private readonly TreeViewColumn valueCol;

		private readonly TreeViewColumn typeCol;

		private readonly TreeViewColumn pinCol;

		private static readonly CommandEntrySet menuSet;

		private Dictionary<TreeIter, bool> evalSpinnersIcons = new Dictionary<TreeIter, bool>();

		private bool allowAdding;

		private bool allowEditing;

		private bool allowExpanding = true;

		private bool editing;

		private bool wasHandled;

		private CodeCompletionContext ctx;

		private Gdk.Key key;

		private char keyChar;

		private ModifierType modifierState;

		private uint keyValue;

		private TreeIter lastPinIter;

		private PreviewButtonIcons iconBeforeSelected;

		private PreviewButtonIcons currentIcon;

		private TreeIter currentHoverIter = TreeIter.Zero;

		private Gdk.Rectangle startPreviewCaret;

		private double startHAdj;

		private double startVAdj;

		public StackFrame Frame
		{
			get
			{
				return frame;
			}
			set
			{
				frame = value;
				Update();
			}
		}

		public bool AllowAdding
		{
			get
			{
				return allowAdding;
			}
			set
			{
				allowAdding = value;
				Refresh(resetScrollPosition: false);
			}
		}

		public bool AllowEditing
		{
			get
			{
				return allowEditing;
			}
			set
			{
				allowEditing = value;
				Refresh(resetScrollPosition: false);
			}
		}

		public bool AllowPinning
		{
			get
			{
				return pinCol.Visible;
			}
			set
			{
				pinCol.Visible = value;
			}
		}

		public bool RootPinAlwaysVisible { get; set; }

		public bool AllowExpanding
		{
			get
			{
				return allowExpanding;
			}
			set
			{
				allowExpanding = value;
			}
		}

		public bool AllowPopupMenu { get; set; }

		private static bool CanQueryDebugger
		{
			get
			{
				if (DebuggingService.IsConnected)
				{
					return DebuggingService.IsPaused;
				}
				return false;
			}
		}

		public PinnedWatch PinnedWatch { get; set; }

		public string PinnedWatchFile { get; set; }

		public int PinnedWatchLine { get; set; }

		public bool CompactView
		{
			get
			{
				return compact;
			}
			set
			{
				compact = value;
				FontDescription fontDescription;
				if (compact)
				{
					fontDescription = base.Style.FontDescription.Copy();
					fontDescription.Size = fontDescription.Size * 8 / 10;
					expCol.Sizing = TreeViewColumnSizing.Autosize;
					valueCol.Sizing = TreeViewColumnSizing.Autosize;
					valueCol.MaxWidth = 800;
					crpViewer.Image = ImageService.GetIcon(Gtk.Stock.Edit).WithSize(12.0, 12.0);
					ColumnsAutosize();
				}
				else
				{
					fontDescription = base.Style.FontDescription;
					expCol.Sizing = TreeViewColumnSizing.Fixed;
					valueCol.Sizing = TreeViewColumnSizing.Fixed;
					valueCol.MaxWidth = int.MaxValue;
				}
				typeCol.Visible = !compact;
				crtExp.FontDesc = fontDescription;
				crtValue.FontDesc = fontDescription;
				crtType.FontDesc = fontDescription;
				crpButton.FontDesc = fontDescription;
				ResetColumnSizes();
				AdjustColumnSizes();
			}
		}

		public IEnumerable<string> Expressions => valueNames;

		CodeCompletionContext ICompletionWidget.CurrentCodeCompletionContext => ((ICompletionWidget)this).CreateCodeCompletionContext(editEntry.Position);

		int ICompletionWidget.CaretOffset => editEntry.Position;

		int ICompletionWidget.TextLength => editEntry.Text.Length;

		int ICompletionWidget.SelectedLength => 0;

		Gtk.Style ICompletionWidget.GtkStyle => editEntry.Style;

		public event EventHandler StartEditing;

		public event EventHandler EndEditing;

		public event EventHandler PinStatusChanged;

		public event EventHandler CompletionContextChanged;

		static ObjectValueTreeView()
		{
			menuSet = new CommandEntrySet();
			menuSet.AddItem(DebugCommands.AddWatch);
			menuSet.AddSeparator();
			menuSet.AddItem(EditCommands.Copy);
			menuSet.AddItem(EditCommands.Rename);
			menuSet.AddItem(EditCommands.DeleteKey);
		}

		public ObjectValueTreeView()
		{
			store = new Gtk.TreeStore(typeof(string), typeof(string), typeof(string), typeof(ObjectValue), typeof(bool), typeof(bool), typeof(string), typeof(string), typeof(string), typeof(bool), typeof(string), typeof(Xwt.Drawing.Image), typeof(bool), typeof(string), typeof(Xwt.Drawing.Image), typeof(bool), typeof(string));
			base.Model = store;
			base.RulesHint = true;
			base.EnableSearch = false;
			AllowPopupMenu = true;
			base.Selection.Mode = Gtk.SelectionMode.Multiple;
			base.Selection.Changed += HandleSelectionChanged;
			ResetColumnSizes();
			FontDescription fontDescription = base.Style.FontDescription.Copy();
			fontDescription.Size = fontDescription.Size * 8 / 10;
			liveIcon = ImageService.GetIcon(Gtk.Stock.Execute, Gtk.IconSize.Menu);
			noLiveIcon = liveIcon.WithAlpha(0.5);
			expCol = new TreeViewColumn();
			expCol.Title = GettextCatalog.GetString("Name");
			CellRendererImage cell = new CellRendererImage();
			expCol.PackStart(cell, expand: false);
			expCol.AddAttribute(cell, "stock_id", 6);
			crtExp = new CellRendererTextWithIcon();
			expCol.PackStart(crtExp, expand: true);
			expCol.AddAttribute(crtExp, "text", 0);
			expCol.AddAttribute(crtExp, "editable", 4);
			expCol.AddAttribute(crtExp, "foreground", 7);
			expCol.AddAttribute(crtExp, "icon", 13);
			expCol.Resizable = true;
			expCol.Sizing = TreeViewColumnSizing.Fixed;
			expCol.MinWidth = 15;
			expCol.AddNotification("width", OnColumnWidthChanged);
			AppendColumn(expCol);
			valueCol = new TreeViewColumn();
			valueCol.Title = GettextCatalog.GetString("Value");
			CellRendererImage cell2 = new CellRendererImage();
			valueCol.PackStart(cell2, expand: false);
			valueCol.AddAttribute(cell2, "visible", 15);
			valueCol.AddAttribute(cell2, "image", 14);
			CellRendererColorPreview cellRendererColorPreview = new CellRendererColorPreview();
			valueCol.PackStart(cellRendererColorPreview, expand: false);
			valueCol.SetCellDataFunc(cellRendererColorPreview, delegate(TreeViewColumn tree_column, CellRenderer cellRenderer, TreeModel model, TreeIter iter)
			{
				ObjectValue objectValue = (ObjectValue)model.GetValue(iter, 3);
				Xwt.Drawing.Color? color = ((objectValue == null || objectValue.IsNull || !DebuggingService.HasGetConverter<Xwt.Drawing.Color>(objectValue)) ? ((Xwt.Drawing.Color?)null) : new Xwt.Drawing.Color?(DebuggingService.GetGetConverter<Xwt.Drawing.Color>(objectValue).GetValue(objectValue)));
				if (color.HasValue)
				{
					((CellRendererColorPreview)cellRenderer).Color = color.Value;
					cellRenderer.Visible = true;
				}
				else
				{
					cellRenderer.Visible = false;
				}
			});
			crpButton = new CellRendererRoundedButton();
			valueCol.PackStart(crpButton, expand: false);
			valueCol.AddAttribute(crpButton, "visible", 9);
			valueCol.AddAttribute(crpButton, "text", 16);
			crpViewer = new CellRendererImage();
			crpViewer.Image = ImageService.GetIcon(Gtk.Stock.Edit, Gtk.IconSize.Menu);
			valueCol.PackStart(crpViewer, expand: false);
			valueCol.AddAttribute(crpViewer, "visible", 12);
			crtValue = new CellRendererTextUrl();
			valueCol.PackStart(crtValue, expand: true);
			valueCol.AddAttribute(crtValue, "texturl", 1);
			valueCol.AddAttribute(crtValue, "editable", 5);
			valueCol.AddAttribute(crtValue, "foreground", 8);
			valueCol.Resizable = true;
			valueCol.MinWidth = 15;
			valueCol.AddNotification("width", OnColumnWidthChanged);
			valueCol.Sizing = TreeViewColumnSizing.Fixed;
			AppendColumn(valueCol);
			typeCol = new TreeViewColumn();
			typeCol.Title = GettextCatalog.GetString("Type");
			crtType = new CellRendererText();
			typeCol.PackStart(crtType, expand: true);
			typeCol.AddAttribute(crtType, "text", 2);
			typeCol.Resizable = true;
			typeCol.Sizing = TreeViewColumnSizing.Fixed;
			typeCol.MinWidth = 15;
			typeCol.AddNotification("width", OnColumnWidthChanged);
			AppendColumn(typeCol);
			pinCol = new TreeViewColumn();
			crpPin = new CellRendererImage();
			pinCol.PackStart(crpPin, expand: false);
			pinCol.AddAttribute(crpPin, "stock_id", 10);
			crpLiveUpdate = new CellRendererImage();
			pinCol.PackStart(crpLiveUpdate, expand: false);
			pinCol.AddAttribute(crpLiveUpdate, "image", 11);
			pinCol.Resizable = false;
			pinCol.Visible = false;
			pinCol.Expand = false;
			AppendColumn(pinCol);
			state = new TreeViewState(this, 0);
			crtExp.Edited += OnExpEdited;
			crtExp.EditingStarted += OnExpEditing;
			crtExp.EditingCanceled += OnEditingCancelled;
			crtValue.EditingStarted += OnValueEditing;
			crtValue.Edited += OnValueEdited;
			crtValue.EditingCanceled += OnEditingCancelled;
			this.EnableAutoTooltips();
			createMsg = GettextCatalog.GetString("Click here to add a new watch");
			CompletionWindowManager.WindowClosed += HandleCompletionWindowClosed;
			PreviewWindowManager.WindowClosed += HandlePreviewWindowClosed;
			base.ScrollAdjustmentsSet += HandleScrollAdjustmentsSet;
		}

		private void HandleSelectionChanged(object sender, EventArgs e)
		{
			if (!currentHoverIter.Equals(TreeIter.Zero) && store.IterIsValid(currentHoverIter))
			{
				if (base.Selection.IterIsSelected(currentHoverIter))
				{
					SetPreviewButtonIcon(PreviewButtonIcons.Selected, currentHoverIter);
				}
				else
				{
					SetPreviewButtonIcon(iconBeforeSelected, currentHoverIter);
				}
			}
			KeyValuePair<TreeIter, bool>[] array = evalSpinnersIcons.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<TreeIter, bool> keyValuePair = array[i];
				if (store.IterIsValid(keyValuePair.Key) && base.Selection.IterIsSelected(keyValuePair.Key))
				{
					if (!keyValuePair.Value)
					{
						store.LoadIcon(keyValuePair.Key, 14, "md-spinner-selected-16", Gtk.IconSize.Menu);
						evalSpinnersIcons[keyValuePair.Key] = true;
					}
				}
				else if (keyValuePair.Value)
				{
					store.LoadIcon(keyValuePair.Key, 14, "md-spinner-normal-16", Gtk.IconSize.Menu);
					evalSpinnersIcons[keyValuePair.Key] = false;
				}
			}
		}

		private void HandleScrollAdjustmentsSet(object o, ScrollAdjustmentsSetArgs args)
		{
			base.Hadjustment.ValueChanged += UpdatePreviewPosition;
			base.Vadjustment.ValueChanged += UpdatePreviewPosition;
		}

		private void UpdatePreviewPosition(object sender, EventArgs e)
		{
			UpdatePreviewPosition();
		}

		private void UpdatePreviewPosition()
		{
			if (!startPreviewCaret.IsEmpty)
			{
				Gdk.Rectangle value = new Gdk.Rectangle((int)((double)startPreviewCaret.Left + (startHAdj - base.Hadjustment.Value)), (int)((double)startPreviewCaret.Top + (startVAdj - base.Vadjustment.Value)), startPreviewCaret.Width, startPreviewCaret.Height);
				if (new Gdk.Rectangle(base.VisibleRect.X - (int)base.Hadjustment.Value, base.VisibleRect.Y - (int)base.Vadjustment.Value, base.VisibleRect.Width, base.VisibleRect.Height).Contains(new Gdk.Point(value.X + value.Width / 2, value.Y + value.Height / 2 - ((!CompactView) ? 30 : 0))))
				{
					PreviewWindowManager.RepositionWindow(value);
				}
				else
				{
					PreviewWindowManager.DestroyWindow();
				}
			}
		}

		private void HandlePreviewWindowClosed(object sender, EventArgs e)
		{
			SetPreviewButtonIcon(PreviewButtonIcons.Hidden);
		}

		private void HandleCompletionWindowClosed(object sender, EventArgs e)
		{
			currentCompletionData = null;
		}

		protected override void OnDestroyed()
		{
			CompletionWindowManager.WindowClosed -= HandleCompletionWindowClosed;
			PreviewWindowManager.WindowClosed -= HandlePreviewWindowClosed;
			PreviewWindowManager.DestroyWindow();
			crtExp.Edited -= OnExpEdited;
			crtExp.EditingStarted -= OnExpEditing;
			crtExp.EditingCanceled -= OnEditingCancelled;
			crtValue.EditingStarted -= OnValueEditing;
			crtValue.Edited -= OnValueEdited;
			crtValue.EditingCanceled -= OnEditingCancelled;
			typeCol.RemoveNotification("width", OnColumnWidthChanged);
			valueCol.RemoveNotification("width", OnColumnWidthChanged);
			expCol.RemoveNotification("width", OnColumnWidthChanged);
			base.Hadjustment.ValueChanged -= UpdatePreviewPosition;
			base.Vadjustment.ValueChanged -= UpdatePreviewPosition;
			values.Clear();
			valueNames.Clear();
			Frame = null;
			disposed = true;
			cancellationTokenSource.Cancel();
			base.OnDestroyed();
		}

		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);
			AdjustColumnSizes();
			UpdatePreviewPosition();
		}

		protected override void OnShown()
		{
			base.OnShown();
			AdjustColumnSizes();
		}

		protected override void OnRealized()
		{
			base.OnRealized();
			AdjustColumnSizes();
		}

		private void OnColumnWidthChanged(object o, NotifyArgs args)
		{
			if (!columnSizesUpdating && allowStoreColumnSizes)
			{
				StoreColumnSizes();
			}
		}

		private void AdjustColumnSizes()
		{
			if (!base.Visible || base.Allocation.Width <= 0 || columnSizesUpdating || compact)
			{
				return;
			}
			columnSizesUpdating = true;
			double num = base.Allocation.Width;
			int num2 = Math.Max((int)(num * expColWidth), 1);
			if (num2 != expCol.FixedWidth)
			{
				expCol.FixedWidth = num2;
			}
			if (typeCol.Visible)
			{
				int num3 = Math.Max((int)(num * typeColWidth), 1);
				if (num3 != typeCol.FixedWidth)
				{
					typeCol.FixedWidth = num3;
				}
			}
			int num4 = Math.Max((int)(num * valueColWidth), 1);
			if (num4 != valueCol.FixedWidth)
			{
				valueCol.FixedWidth = num4;
				Gtk.Application.Invoke(delegate
				{
					QueueResize();
				});
			}
			columnSizesUpdating = false;
			columnsAdjusted = true;
		}

		private void StoreColumnSizes()
		{
			if (base.IsRealized && base.Visible && columnsAdjusted && !compact)
			{
				double num = base.Allocation.Width;
				expColWidth = (double)expCol.Width / num;
				valueColWidth = (double)valueCol.Width / num;
				if (typeCol.Visible)
				{
					typeColWidth = (double)typeCol.Width / num;
				}
			}
		}

		private void ResetColumnSizes()
		{
			expColWidth = 0.3;
			valueColWidth = 0.5;
			typeColWidth = 0.2;
		}

		public void SaveState()
		{
			state.Save();
		}

		public void LoadState()
		{
			restoringState = true;
			state.Load();
			restoringState = false;
		}

		public void AddExpression(string exp)
		{
			valueNames.Add(exp);
			Refresh(resetScrollPosition: false);
		}

		public void AddExpressions(IEnumerable<string> exps)
		{
			valueNames.AddRange(exps);
			Refresh(resetScrollPosition: false);
		}

		public void RemoveExpression(string exp)
		{
			cachedValues.Remove(exp);
			valueNames.Remove(exp);
			Refresh(resetScrollPosition: true);
		}

		public void AddValue(ObjectValue value)
		{
			values.Add(value);
			Refresh(resetScrollPosition: false);
		}

		public void AddValues(IEnumerable<ObjectValue> newValues)
		{
			foreach (ObjectValue newValue in newValues)
			{
				values.Add(newValue);
			}
			Refresh(resetScrollPosition: false);
		}

		public void RemoveValue(ObjectValue value)
		{
			values.Remove(value);
			Refresh(resetScrollPosition: true);
		}

		public void ReplaceValue(ObjectValue old, ObjectValue @new)
		{
			int num = values.IndexOf(old);
			if (num != -1)
			{
				values[num] = @new;
				Refresh(resetScrollPosition: false);
			}
		}

		public void ClearAll()
		{
			values.Clear();
			valueNames.Clear();
			cachedValues.Clear();
			frame = null;
			Refresh(resetScrollPosition: true);
		}

		public void ClearValues()
		{
			values.Clear();
			Refresh(resetScrollPosition: true);
		}

		public void ClearExpressions()
		{
			valueNames.Clear();
			Update();
		}

		public void Update()
		{
			cachedValues.Clear();
			Refresh(resetScrollPosition: true);
		}

		private void Refresh(bool resetScrollPosition)
		{
			foreach (ObjectValue item in new List<ObjectValue>(nodes.Keys))
			{
				UnregisterValue(item);
			}
			nodes.Clear();
			if (base.IsRealized && resetScrollPosition)
			{
				ScrollToPoint(0, 0);
			}
			SaveState();
			CleanPinIcon();
			store.Clear();
			bool flag = AllowAdding;
			foreach (ObjectValue value in values)
			{
				AppendValue(TreeIter.Zero, null, value);
				if (value.HasChildren)
				{
					flag = true;
				}
			}
			if (valueNames.Count > 0)
			{
				ObjectValue[] array = GetValues(valueNames.ToArray());
				for (int i = 0; i < array.Length; i++)
				{
					AppendValue(TreeIter.Zero, valueNames[i], array[i]);
					if (array[i].HasChildren)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				base.ShowExpanders = true;
			}
			if (AllowAdding)
			{
				store.AppendValues(createMsg, "", "", null, true, true, null, "gray", "gray");
			}
			LoadState();
		}

		public void Refresh()
		{
			Refresh(resetScrollPosition: true);
		}

		private void HandleValueButton(TreeIter it)
		{
			ObjectValue objectValue = (ObjectValue)store.GetValue(it, 3);
			if (objectValue.Flags.HasFlag(ObjectValueFlags.IEnumerable))
			{
				if (objectValue.Name == "")
				{
					LoadIEnumerableChildren(it);
				}
				else
				{
					ExpandRow(store.GetPath(it), open_all: false);
				}
			}
			else
			{
				RefreshRow(it);
			}
		}

		private void LoadIEnumerableChildren(TreeIter iter)
		{
			ObjectValue value = (ObjectValue)store.GetValue(iter, 3);
			if (enumerableLoading.Contains(value))
			{
				return;
			}
			enumerableLoading.Add(value);
			store.SetValue(iter, 16, "");
			if (value.Name == "")
			{
				store.IterParent(out iter, iter);
				value = (ObjectValue)store.GetValue(iter, 3);
			}
			int numberOfChildren = store.IterNChildren(iter);
			Task.Factory.StartNew(delegate(object arg)
			{
				try
				{
					return ((ObjectValue)arg).GetRangeOfChildren(numberOfChildren - 1, 20);
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Failed to get ObjectValue children.", ex);
					return new ObjectValue[0];
				}
			}, value, cancellationTokenSource.Token).ContinueWith(delegate(Task<ObjectValue[]> t)
			{
				if (!disposed)
				{
					store.IterNthChild(out var iter2, iter, numberOfChildren - 1);
					ObjectValue[] result = t.Result;
					foreach (ObjectValue val in result)
					{
						SetValues(iter, iter2, null, val);
						RegisterValue(val, iter2);
						iter2 = store.InsertNodeAfter(iter2);
					}
					ScrollToCell(store.GetPath(iter2), expCol, use_align: true, 0f, 0f);
					if (t.Result.Length == 20)
					{
						SetValues(iter, iter2, null, ObjectValue.CreateNullObject(null, "", "", ObjectValueFlags.IEnumerable));
					}
					else
					{
						store.Remove(ref iter2);
					}
					if (compact)
					{
						ColumnsAutosize();
					}
					enumerableLoading.Remove(value);
				}
			}, cancellationTokenSource.Token, TaskContinuationOptions.NotOnCanceled, Xwt.Application.UITaskScheduler);
		}

		private void RefreshRow(TreeIter iter)
		{
			ObjectValue objectValue = (ObjectValue)store.GetValue(iter, 3);
			UnregisterValue(objectValue);
			RemoveChildren(iter);
			if (!store.IterParent(out var iter2, iter))
			{
				iter2 = TreeIter.Zero;
			}
			if (CanQueryDebugger && frame != null)
			{
				EvaluationOptions evaluationOptions = frame.DebuggerSession.Options.EvaluationOptions.Clone();
				evaluationOptions.AllowMethodEvaluation = true;
				evaluationOptions.AllowToStringCalls = true;
				evaluationOptions.AllowTargetInvoke = true;
				evaluationOptions.EllipsizeStrings = false;
				string name = objectValue.Name;
				objectValue.Refresh(evaluationOptions);
				if (store.IterDepth(iter) == 0)
				{
					objectValue.Name = name;
				}
			}
			SetValues(iter2, iter, objectValue.Name, objectValue);
			RegisterValue(objectValue, iter);
		}

		private void RemoveChildren(TreeIter iter)
		{
			TreeIter iter2;
			while (store.IterChildren(out iter2, iter))
			{
				ObjectValue objectValue = (ObjectValue)store.GetValue(iter2, 3);
				if (objectValue != null)
				{
					UnregisterValue(objectValue);
				}
				RemoveChildren(iter2);
				store.Remove(ref iter2);
			}
		}

		private void RegisterValue(ObjectValue val, TreeIter iter)
		{
			if (val.IsEvaluating)
			{
				nodes[val] = new TreeRowReference(store, store.GetPath(iter));
				val.ValueChanged += OnValueUpdated;
			}
		}

		private void UnregisterValue(ObjectValue val)
		{
			val.ValueChanged -= OnValueUpdated;
			nodes.Remove(val);
		}

		private void OnValueUpdated(object o, EventArgs a)
		{
			Gtk.Application.Invoke(delegate
			{
				if (!disposed)
				{
					ObjectValue objectValue = (ObjectValue)o;
					if (FindValue(objectValue, out var it))
					{
						if (store.IterDepth(it) == 0)
						{
							objectValue.Name = (string)store.GetValue(it, 0);
						}
						RemoveChildren(it);
						if (!store.IterParent(out var iter, it))
						{
							iter = TreeIter.Zero;
						}
						if (objectValue.IsEvaluatingGroup)
						{
							if (objectValue.ArrayCount == 0)
							{
								store.Remove(ref it);
							}
							else
							{
								SetValues(iter, it, null, objectValue.GetArrayItem(0));
								RegisterValue(objectValue, it);
								for (int i = 1; i < objectValue.ArrayCount; i++)
								{
									TreeIter treeIter = store.InsertNodeAfter(it);
									ObjectValue arrayItem = objectValue.GetArrayItem(i);
									SetValues(iter, treeIter, null, arrayItem);
									RegisterValue(arrayItem, treeIter);
								}
							}
						}
						else
						{
							SetValues(iter, it, objectValue.Name, objectValue);
						}
					}
					UnregisterValue(objectValue);
				}
			});
		}

		private bool FindValue(ObjectValue val, out TreeIter it)
		{
			if (!nodes.TryGetValue(val, out var value) || !value.Valid())
			{
				it = TreeIter.Zero;
				return false;
			}
			return store.GetIter(out it, value.Path);
		}

		public void ResetChangeTracking()
		{
			oldValues.Clear();
		}

		public void ChangeCheckpoint()
		{
			oldValues.Clear();
			if (store.GetIterFirst(out var iter))
			{
				ChangeCheckpoint(iter, "/");
			}
		}

		private void ChangeCheckpoint(TreeIter it, string path)
		{
			do
			{
				string text = (string)store.GetValue(it, 0);
				string value = (string)store.GetValue(it, 1);
				oldValues[path + text] = value;
				if (store.IterChildren(out var iter, it))
				{
					ChangeCheckpoint(iter, text + "/");
				}
			}
			while (store.IterNext(ref it));
		}

		private void AppendValue(TreeIter parent, string name, ObjectValue val)
		{
			TreeIter treeIter = ((!parent.Equals(TreeIter.Zero)) ? store.AppendNode(parent) : store.AppendNode());
			SetValues(parent, treeIter, name, val);
			RegisterValue(val, treeIter);
		}

		private void SetValues(TreeIter parent, TreeIter it, string name, ObjectValue val)
		{
			string value = null;
			string value2 = null;
			string text = null;
			string text2 = null;
			name = name ?? val.Name;
			bool flag = !parent.Equals(TreeIter.Zero);
			bool value3 = false;
			string text3 = (flag ? (GetIterPath(parent) + "/" + name) : ("/" + name));
			oldValues.TryGetValue(text3, out var value4);
			bool flag2;
			string text4;
			if (val.IsUnknown)
			{
				if (frame != null)
				{
					text4 = GettextCatalog.GetString("The name '{0}' does not exist in the current context.", val.Name);
					value = "gray";
					flag2 = false;
				}
				else
				{
					flag2 = !val.IsReadOnly;
					text4 = string.Empty;
				}
				text2 = MonoDevelop.Ide.Gui.Stock.Warning;
			}
			else if (val.IsError)
			{
				text2 = MonoDevelop.Ide.Gui.Stock.Warning;
				text4 = val.Value;
				int num = text4.IndexOf('\n');
				if (num != -1)
				{
					text4 = text4.Substring(0, num);
				}
				value2 = "red";
				flag2 = false;
			}
			else if (val.IsNotSupported)
			{
				text4 = "";
				value2 = "gray";
				if (val.CanRefresh)
				{
					text = GettextCatalog.GetString("Show Value");
				}
				flag2 = false;
			}
			else if (val.IsEvaluating)
			{
				text4 = GettextCatalog.GetString("Evaluating...");
				if (base.Selection.IterIsSelected(it))
				{
					evalSpinnersIcons[it] = true;
					text2 = "md-spinner-selected-16";
				}
				else
				{
					evalSpinnersIcons[it] = false;
					text2 = "md-spinner-normal-16";
				}
				value2 = "gray";
				if (val.IsEvaluatingGroup)
				{
					value = "gray";
					name = val.Name;
				}
				flag2 = false;
			}
			else if (val.Flags.HasFlag(ObjectValueFlags.IEnumerable))
			{
				text = ((!(val.Name == "")) ? GettextCatalog.GetString("Show Values") : GettextCatalog.GetString("Show More"));
				text4 = "";
				flag2 = false;
			}
			else
			{
				value3 = !val.IsNull && DebuggingService.HasValueVisualizers(val);
				flag2 = val.IsPrimitive && !val.IsReadOnly;
				text4 = ((val.IsNull || !DebuggingService.HasInlineVisualizer(val)) ? (val.DisplayValue ?? "(null)") : DebuggingService.GetInlineVisualizer(val).InlineVisualize(val));
				if (value4 != null && text4 != value4)
				{
					value = (value2 = "blue");
				}
			}
			text4 = text4.Replace("\r\n", " ").Replace("\n", " ");
			bool hasChildren = val.HasChildren;
			string icon = GetIcon(val.Flags);
			store.SetValue(it, 0, name);
			store.SetValue(it, 1, text4);
			store.SetValue(it, 2, val.TypeName);
			store.SetValue(it, 3, val);
			store.SetValue(it, 4, !flag && AllowAdding);
			store.SetValue(it, 5, flag2 && AllowEditing);
			store.SetValue(it, 6, icon);
			store.SetValue(it, 7, value);
			store.SetValue(it, 8, value2);
			if (text2 != "md-spinner-normal-16" && text2 != "md-spinner-selected-16")
			{
				evalSpinnersIcons.Remove(it);
			}
			store.SetValue(it, 15, text2 != null);
			store.LoadIcon(it, 14, text2, Gtk.IconSize.Menu);
			store.SetValue(it, 9, text != null);
			store.SetValue(it, 16, text);
			store.SetValue(it, 12, value3);
			if (ValidObjectForPreviewIcon(it))
			{
				store.SetValue(it, 13, "md-empty");
			}
			if (!flag && PinnedWatch != null)
			{
				store.SetValue(it, 10, "md-pin-down");
				if (PinnedWatch.LiveUpdate)
				{
					store.SetValue(it, 11, liveIcon);
				}
				else
				{
					store.SetValue(it, 11, noLiveIcon);
				}
			}
			if (RootPinAlwaysVisible && !flag && PinnedWatch == null && AllowPinning)
			{
				store.SetValue(it, 10, "md-pin-up");
			}
			if (hasChildren)
			{
				store.AppendValues(it, GettextCatalog.GetString("Loading..."), "", "", null, true);
				if (!base.ShowExpanders)
				{
					base.ShowExpanders = true;
				}
			}
		}

		public static string GetIcon(ObjectValueFlags flags)
		{
			if ((flags & ObjectValueFlags.Field) != ObjectValueFlags.None && (flags & ObjectValueFlags.ReadOnly) != ObjectValueFlags.None)
			{
				return "md-literal";
			}
			string text = (((flags & ObjectValueFlags.Global) != ObjectValueFlags.None) ? "static-" : string.Empty);
			string text2;
			switch (flags & ObjectValueFlags.OriginMask)
			{
			case ObjectValueFlags.Property:
				text2 = "property";
				break;
			case ObjectValueFlags.Type:
				text2 = "class";
				text = string.Empty;
				break;
			case ObjectValueFlags.Method:
				text2 = "method";
				break;
			case ObjectValueFlags.Literal:
				return "md-literal";
			case ObjectValueFlags.Namespace:
				return "md-name-space";
			case ObjectValueFlags.Group:
				return "md-open-resource-folder";
			case ObjectValueFlags.Field:
				text2 = "field";
				break;
			case ObjectValueFlags.Variable:
				return "md-variable";
			default:
				return "md-empty";
			}
			string text3;
			switch (flags & ObjectValueFlags.AccessMask)
			{
			case ObjectValueFlags.Private:
				text3 = "private-";
				break;
			case ObjectValueFlags.Internal:
				text3 = "internal-";
				break;
			case ObjectValueFlags.Protected:
			case ObjectValueFlags.InternalProtected:
				text3 = "protected-";
				break;
			default:
				text3 = string.Empty;
				break;
			}
			return "md-" + text3 + text + text2;
		}

		protected override bool OnTestExpandRow(TreeIter iter, TreePath path)
		{
			if (!restoringState)
			{
				if (!allowExpanding)
				{
					return true;
				}
				if (GetRowExpanded(path))
				{
					return true;
				}
				if (store.IterParent(out var iter2, iter) && !GetRowExpanded(store.GetPath(iter2)))
				{
					return true;
				}
			}
			return base.OnTestExpandRow(iter, path);
		}

		protected override void OnRowCollapsed(TreeIter iter, TreePath path)
		{
			base.OnRowCollapsed(iter, path);
			if (compact)
			{
				ColumnsAutosize();
			}
			ScrollToCell(path, expCol, use_align: true, 0f, 0f);
		}

		private static Task<ObjectValue[]> GetChildrenAsync(ObjectValue value, CancellationToken cancellationToken)
		{
			return Task.Factory.StartNew(delegate(object arg)
			{
				try
				{
					return ((ObjectValue)arg).GetAllChildren();
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Failed to get ObjectValue children.", ex);
					return new ObjectValue[0];
				}
			}, value, cancellationToken);
		}

		private void AddChildrenAsync(ObjectValue value, TreePathReference row)
		{
			if (expandTasks.TryGetValue(value, out var value2))
			{
				return;
			}
			value2 = GetChildrenAsync(value, cancellationTokenSource.Token).ContinueWith(delegate(Task<ObjectValue[]> t)
			{
				if (!disposed)
				{
					if (row.IsValid && store.GetIter(out var iter, row.Path) && store.IterChildren(out var iter2, iter))
					{
						ObjectValue[] result = t.Result;
						foreach (ObjectValue val in result)
						{
							SetValues(iter, iter2, null, val);
							RegisterValue(val, iter2);
							iter2 = store.InsertNodeAfter(iter2);
						}
						store.Remove(ref iter2);
						if (compact)
						{
							ColumnsAutosize();
						}
					}
					expandTasks.Remove(value);
					row.Dispose();
				}
			}, cancellationTokenSource.Token, TaskContinuationOptions.NotOnCanceled, Xwt.Application.UITaskScheduler);
			expandTasks.Add(value, value2);
		}

		protected override void OnRowExpanded(TreeIter iter, TreePath path)
		{
			if (store.IterChildren(out var iter2, iter))
			{
				ObjectValue objectValue = (ObjectValue)store.GetValue(iter2, 3);
				if (objectValue == null)
				{
					objectValue = (ObjectValue)store.GetValue(iter, 3);
					if (objectValue.HasFlag(ObjectValueFlags.IEnumerable))
					{
						LoadIEnumerableChildren(iter);
					}
					else
					{
						AddChildrenAsync(objectValue, new TreePathReference(store, store.GetPath(iter)));
					}
				}
			}
			base.OnRowExpanded(iter, path);
			ScrollToCell(path, expCol, use_align: true, 0f, 0f);
		}

		private string GetIterPath(TreeIter iter)
		{
			StringBuilder stringBuilder = new StringBuilder();
			do
			{
				string text = (string)store.GetValue(iter, 0);
				stringBuilder.Insert(0, "/" + text);
			}
			while (store.IterParent(out iter, iter));
			return stringBuilder.ToString();
		}

		private void OnExpEditing(object s, EditingStartedArgs args)
		{
			if (store.GetIterFromString(out var _, args.Path))
			{
				Entry entry = (Entry)args.Editable;
				if (entry.Text == createMsg)
				{
					entry.Text = string.Empty;
				}
				OnStartEditing(args);
			}
		}

		private void OnExpEdited(object s, EditedArgs args)
		{
			OnEndEditing();
			if (!store.GetIterFromString(out var iter, args.Path))
			{
				return;
			}
			if (store.GetValue(iter, 3) == null)
			{
				if (args.NewText.Length > 0)
				{
					valueNames.Add(args.NewText);
					Refresh(resetScrollPosition: false);
				}
				return;
			}
			string text = (string)store.GetValue(iter, 0);
			if (args.NewText == text)
			{
				return;
			}
			int num = valueNames.IndexOf(text);
			if (num != -1)
			{
				if (args.NewText.Length != 0)
				{
					valueNames[num] = args.NewText;
				}
				else
				{
					valueNames.RemoveAt(num);
				}
				cachedValues.Remove(text);
				Refresh(resetScrollPosition: true);
			}
		}

		private void OnValueEditing(object s, EditingStartedArgs args)
		{
			if (store.GetIterFromString(out var iter, args.Path))
			{
				Entry entry = (Entry)args.Editable;
				string text = ((store.GetValue(iter, 3) is ObjectValue objectValue) ? objectValue.Value : null);
				if (!string.IsNullOrEmpty(text))
				{
					entry.Text = text;
				}
				entry.GrabFocus();
				OnStartEditing(args);
			}
		}

		private void OnValueEdited(object s, EditedArgs args)
		{
			OnEndEditing();
			if (!store.GetIterFromString(out var iter, args.Path))
			{
				return;
			}
			ObjectValue objectValue = (ObjectValue)store.GetValue(iter, 3);
			if (objectValue == null)
			{
				return;
			}
			try
			{
				string newText = args.NewText;
				if (objectValue.Value != newText)
				{
					objectValue.Value = newText;
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Could not set value for object '" + objectValue.Name + "'", ex);
			}
			store.SetValue(iter, 1, objectValue.DisplayValue);
			string value = null;
			string iterPath = GetIterPath(iter);
			if (oldValues.TryGetValue(iterPath, out var value2) && value2 != objectValue.Value)
			{
				value = "blue";
			}
			store.SetValue(iter, 7, value);
			store.SetValue(iter, 8, value);
		}

		private void OnEditingCancelled(object s, EventArgs args)
		{
			OnEndEditing();
		}

		private void OnStartEditing(EditingStartedArgs args)
		{
			editing = true;
			editEntry = (Entry)args.Editable;
			editEntry.KeyPressEvent += OnEditKeyPress;
			editEntry.KeyReleaseEvent += OnEditKeyRelease;
			if (StartEditing != null)
			{
				StartEditing(this, EventArgs.Empty);
			}
		}

		private void OnEndEditing()
		{
			editing = false;
			editEntry.KeyPressEvent -= OnEditKeyPress;
			editEntry.KeyReleaseEvent -= OnEditKeyRelease;
			CompletionWindowManager.HideWindow();
			currentCompletionData = null;
			if (EndEditing != null)
			{
				EndEditing(this, EventArgs.Empty);
			}
		}

		private void OnEditKeyRelease(object sender, EventArgs e)
		{
			if (!wasHandled)
			{
				string text = ((ctx == null) ? editEntry.Text : editEntry.Text.Substring(Math.Max(0, Math.Min(ctx.TriggerOffset, editEntry.Text.Length))));
				CompletionWindowManager.UpdateWordSelection(text);
				CompletionWindowManager.PostProcessKeyEvent(key, keyChar, modifierState);
				PopupCompletion((Entry)sender);
			}
		}

		[ConnectBefore]
		private void OnEditKeyPress(object s, KeyPressEventArgs args)
		{
			wasHandled = false;
			key = args.Event.Key;
			keyChar = (char)args.Event.Key;
			modifierState = args.Event.State;
			keyValue = args.Event.KeyValue;
			if (currentCompletionData != null)
			{
				wasHandled = CompletionWindowManager.PreProcessKeyEvent(key, keyChar, modifierState);
				args.RetVal = wasHandled;
			}
		}

		private static bool IsCompletionChar(char c)
		{
			if (!char.IsLetterOrDigit(c) && !char.IsPunctuation(c) && !char.IsSymbol(c))
			{
				return char.IsWhiteSpace(c);
			}
			return true;
		}

		private void PopupCompletion(Entry entry)
		{
			Gtk.Application.Invoke(delegate
			{
				char c = (char)Keyval.ToUnicode(keyValue);
				if (currentCompletionData == null && IsCompletionChar(c))
				{
					string exp = entry.Text.Substring(0, entry.CursorPosition);
					currentCompletionData = GetCompletionData(exp);
					if (currentCompletionData != null)
					{
						DebugCompletionDataList list = new DebugCompletionDataList(currentCompletionData);
						ctx = ((ICompletionWidget)this).CreateCodeCompletionContext(entry.CursorPosition - currentCompletionData.ExpressionLength);
						CompletionWindowManager.ShowWindow(null, c, list, this, ctx);
					}
					else
					{
						currentCompletionData = null;
					}
				}
			});
		}

		private bool ValidObjectForPreviewIcon(TreeIter it)
		{
			if (!(base.Model.GetValue(it, 3) is ObjectValue objectValue))
			{
				return false;
			}
			if (objectValue.IsNull)
			{
				return false;
			}
			if (objectValue.IsPrimitive)
			{
				if (objectValue.TypeName != "string")
				{
					return false;
				}
				if (objectValue.Value.Length < DebuggingService.DebuggerSession.EvaluationOptions.EllipsizedLength + 3)
				{
					return false;
				}
			}
			if (string.IsNullOrEmpty(objectValue.TypeName))
			{
				return false;
			}
			return true;
		}

		private void SetPreviewButtonIcon(PreviewButtonIcons icon, TreeIter it = default(TreeIter))
		{
			if (PreviewWindowManager.IsVisible)
			{
				return;
			}
			if (!it.Equals(TreeIter.Zero) && !ValidObjectForPreviewIcon(it))
			{
				icon = PreviewButtonIcons.None;
			}
			if (!currentHoverIter.Equals(it) && !currentHoverIter.Equals(TreeIter.Zero) && store.IterIsValid(currentHoverIter) && ValidObjectForPreviewIcon(currentHoverIter) && (string)store.GetValue(currentHoverIter, 13) != "md-empty")
			{
				store.SetValue(currentHoverIter, 13, "md-empty");
			}
			if (!it.Equals(TreeIter.Zero) && store.IterIsValid(it))
			{
				switch (icon)
				{
				case PreviewButtonIcons.Selected:
					if ((currentIcon == PreviewButtonIcons.Active || currentIcon == PreviewButtonIcons.Hover || currentIcon == PreviewButtonIcons.RowHover) && it.Equals(TreeIter.Zero))
					{
						iconBeforeSelected = currentIcon;
					}
					break;
				case PreviewButtonIcons.RowHover:
				case PreviewButtonIcons.Hover:
				case PreviewButtonIcons.Active:
					iconBeforeSelected = icon;
					if (base.Selection.IterIsSelected(it))
					{
						icon = PreviewButtonIcons.Selected;
					}
					break;
				}
				switch (icon)
				{
				case PreviewButtonIcons.None:
					if (store.GetValue(it, 13) != null)
					{
						store.SetValue(it, 13, null);
					}
					break;
				case PreviewButtonIcons.Hidden:
					if ((string)store.GetValue(it, 13) != "md-empty")
					{
						store.SetValue(it, 13, "md-empty");
					}
					break;
				case PreviewButtonIcons.RowHover:
					if ((string)store.GetValue(it, 13) != "md-preview-normal")
					{
						store.SetValue(it, 13, "md-preview-normal");
					}
					break;
				case PreviewButtonIcons.Hover:
					if ((string)store.GetValue(it, 13) != "md-preview-hover")
					{
						store.SetValue(it, 13, "md-preview-hover");
					}
					break;
				case PreviewButtonIcons.Active:
					if ((string)store.GetValue(it, 13) != "md-preview-active")
					{
						store.SetValue(it, 13, "md-preview-active");
					}
					break;
				case PreviewButtonIcons.Selected:
					if ((string)store.GetValue(it, 13) != "md-preview-selected")
					{
						store.SetValue(it, 13, "md-preview-selected");
					}
					break;
				}
				currentIcon = icon;
				currentHoverIter = it;
			}
			else
			{
				currentIcon = PreviewButtonIcons.None;
				currentHoverIter = TreeIter.Zero;
			}
		}

		protected override bool OnMotionNotifyEvent(EventMotion evnt)
		{
			if (!editing && GetPathAtPos((int)evnt.X, (int)evnt.Y, out var path))
			{
				if (store.GetIter(out var iter, path))
				{
					if (GetCellAtPos((int)evnt.X, (int)evnt.Y, out path, out var col, out var cellRenderer) && cellRenderer == crtExp)
					{
						using (Pango.Layout layout = new Pango.Layout(base.PangoContext))
						{
							layout.FontDescription = crtExp.FontDesc.Copy();
							layout.FontDescription.Family = crtExp.Family;
							layout.SetText((string)store.GetValue(iter, 0));
							layout.GetPixelSize(out var width, out var _);
							long num = GetCellRendererArea(path, col, cellRenderer).X + width + cellRenderer.Xpad * 3;
							if ((double)num < evnt.X && (double)(num + 16) > evnt.X)
							{
								SetPreviewButtonIcon(PreviewButtonIcons.Hover, iter);
							}
							else
							{
								SetPreviewButtonIcon(PreviewButtonIcons.RowHover, iter);
							}
						}
					}
					else
					{
						SetPreviewButtonIcon(PreviewButtonIcons.RowHover, iter);
					}
					if (AllowPinning && (path.Depth > 1 || PinnedWatch == null) && !iter.Equals(lastPinIter))
					{
						store.SetValue(iter, 10, "md-pin-up");
						CleanPinIcon();
						if (path.Depth > 1 || !RootPinAlwaysVisible)
						{
							lastPinIter = iter;
						}
					}
				}
			}
			else
			{
				SetPreviewButtonIcon(PreviewButtonIcons.Hidden);
			}
			return base.OnMotionNotifyEvent(evnt);
		}

		private void CleanPinIcon()
		{
			if (!lastPinIter.Equals(TreeIter.Zero))
			{
				store.SetValue(lastPinIter, 10, null);
				lastPinIter = TreeIter.Zero;
			}
		}

		protected override bool OnLeaveNotifyEvent(EventCrossing evnt)
		{
			if (!editing)
			{
				CleanPinIcon();
			}
			SetPreviewButtonIcon(PreviewButtonIcons.Hidden);
			return base.OnLeaveNotifyEvent(evnt);
		}

		protected override bool OnKeyPressEvent(EventKey evnt)
		{
			if (editing)
			{
				return base.OnKeyPressEvent(evnt);
			}
			TreePath[] selectedRows = base.Selection.GetSelectedRows();
			bool flag = false;
			if (selectedRows == null || selectedRows.Length < 1)
			{
				return base.OnKeyPressEvent(evnt);
			}
			switch (evnt.Key)
			{
			case Gdk.Key.Left:
			case Gdk.Key.KP_Left:
			{
				TreePath[] array2 = selectedRows;
				foreach (TreePath treePath in array2)
				{
					TreePath path2 = treePath.Copy();
					if (GetRowExpanded(treePath))
					{
						CollapseRow(treePath);
						flag = true;
					}
					else if (treePath.Up())
					{
						base.Selection.UnselectPath(path2);
						base.Selection.SelectPath(treePath);
						flag = true;
					}
				}
				break;
			}
			case Gdk.Key.Right:
			case Gdk.Key.KP_Right:
			{
				TreePath[] array3 = selectedRows;
				foreach (TreePath treePath2 in array3)
				{
					if (!GetRowExpanded(treePath2))
					{
						ExpandRow(treePath2, open_all: false);
						flag = true;
						continue;
					}
					TreePath path2 = treePath2.Copy();
					treePath2.Down();
					if (path2.Compare(treePath2) != 0)
					{
						base.Selection.UnselectPath(path2);
						base.Selection.SelectPath(treePath2);
						flag = true;
					}
				}
				break;
			}
			case Gdk.Key.BackSpace:
			case Gdk.Key.KP_Delete:
			case Gdk.Key.Delete:
			{
				if (!AllowEditing || !AllowAdding)
				{
					return base.OnKeyPressEvent(evnt);
				}
				Array.Sort(selectedRows, new TreePathComparer(reversed: true));
				TreePath[] array = selectedRows;
				foreach (TreePath path in array)
				{
					if (base.Model.GetIter(out var iter, path))
					{
						ObjectValue objectValue = (ObjectValue)store.GetValue(iter, 3);
						string fullExpression = GetFullExpression(iter);
						if (objectValue != null && values.Contains(objectValue))
						{
							RemoveValue(objectValue);
							flag = true;
						}
						else if (!string.IsNullOrEmpty(fullExpression) && valueNames.Contains(fullExpression))
						{
							RemoveExpression(fullExpression);
							flag = true;
						}
					}
				}
				break;
			}
			}
			if (!flag)
			{
				return base.OnKeyPressEvent(evnt);
			}
			return true;
		}

		private Gdk.Rectangle GetCellRendererArea(TreePath path, TreeViewColumn col, CellRenderer cr)
		{
			Gdk.Rectangle cellArea = GetCellArea(path, col);
			col.CellGetPosition(cr, out var start_pos, out var width);
			return new Gdk.Rectangle(cellArea.X + start_pos, cellArea.Y, width, cellArea.Height);
		}

		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			allowStoreColumnSizes = true;
			bool flag = true;
			if (CanQueryDebugger && evnt.Button == 1 && GetCellAtPos((int)evnt.X, (int)evnt.Y, out var path, out var col, out var cellRenderer))
			{
				store.GetIter(out var iter, path);
				if (cellRenderer == crpViewer)
				{
					ObjectValue val = (ObjectValue)store.GetValue(iter, 3);
					DebuggingService.ShowValueVisualizer(val);
				}
				else if (cellRenderer == crtExp && !PreviewWindowManager.IsVisible && ValidObjectForPreviewIcon(iter))
				{
					ObjectValue val2 = (ObjectValue)store.GetValue(iter, 3);
					startPreviewCaret = GetCellRendererArea(path, col, cellRenderer);
					startHAdj = base.Hadjustment.Value;
					startVAdj = base.Vadjustment.Value;
					using (Pango.Layout layout = new Pango.Layout(base.PangoContext))
					{
						layout.FontDescription = crtExp.FontDesc.Copy();
						layout.FontDescription.Family = crtExp.Family;
						layout.SetText((string)store.GetValue(iter, 0));
						layout.GetPixelSize(out var width, out var _);
						startPreviewCaret.X += (int)(width + cellRenderer.Xpad * 3);
						startPreviewCaret.Width = 16;
						ConvertTreeToWidgetCoords(startPreviewCaret.X, startPreviewCaret.Y, out startPreviewCaret.X, out startPreviewCaret.Y);
						startPreviewCaret.X += (int)base.Hadjustment.Value;
						startPreviewCaret.Y += (int)base.Vadjustment.Value;
						if ((double)startPreviewCaret.X < evnt.X && (double)(startPreviewCaret.X + 16) > evnt.X)
						{
							if (CompactView)
							{
								SetPreviewButtonIcon(PreviewButtonIcons.Active, iter);
							}
							else
							{
								SetPreviewButtonIcon(PreviewButtonIcons.Selected, iter);
							}
							DebuggingService.ShowPreviewVisualizer(val2, this, startPreviewCaret);
							flag = false;
						}
					}
				}
				else if (cellRenderer == crtValue)
				{
					if ((Platform.IsMac && (evnt.State & ModifierType.Mod2Mask) > ModifierType.None) || (!Platform.IsMac && (evnt.State & ModifierType.ControlMask) > ModifierType.None))
					{
						string text = crtValue.Text.Trim('"', '{', '}');
						if (text != null && Uri.TryCreate(text, UriKind.Absolute, out var result) && (result.Scheme == "http" || result.Scheme == "https"))
						{
							DesktopService.ShowUrl(text);
						}
					}
				}
				else if (!editing)
				{
					TreeIter iter3;
					if (cellRenderer == crpButton)
					{
						HandleValueButton(iter);
					}
					else if (cellRenderer == crpPin)
					{
						if (PinnedWatch != null && !store.IterParent(out var _, iter))
						{
							RemovePinnedWatch(iter);
						}
						else
						{
							CreatePinnedWatch(iter);
						}
					}
					else if (cellRenderer == crpLiveUpdate && PinnedWatch != null && !store.IterParent(out iter3, iter))
					{
						DebuggingService.SetLiveUpdateMode(PinnedWatch, !PinnedWatch.LiveUpdate);
						if (PinnedWatch.LiveUpdate)
						{
							store.SetValue(iter, 11, liveIcon);
						}
						else
						{
							store.SetValue(iter, 11, noLiveIcon);
						}
					}
				}
			}
			if (flag)
			{
				PreviewWindowManager.DestroyWindow();
			}
			bool result2 = base.OnButtonPressEvent(evnt);
			if (evnt.TriggersContextMenu())
			{
				return true;
			}
			return result2;
		}

		protected override bool OnButtonReleaseEvent(EventButton evnt)
		{
			allowStoreColumnSizes = false;
			bool result = base.OnButtonReleaseEvent(evnt);
			if (evnt.IsContextMenuButton())
			{
				ShowPopup(evnt);
				return true;
			}
			return result;
		}

		protected override bool OnPopupMenu()
		{
			ShowPopup(null);
			return true;
		}

		private void ShowPopup(EventButton evt)
		{
			if (AllowPopupMenu)
			{
				IdeApp.CommandService.ShowContextMenu(this, evt, menuSet, this);
			}
		}

		[CommandUpdateHandler(EditCommands.SelectAll)]
		protected void UpdateSelectAll(CommandInfo cmd)
		{
			cmd.Enabled = store.GetIterFirst(out var _);
		}

		[CommandHandler(EditCommands.SelectAll)]
		protected new void OnSelectAll()
		{
			base.Selection.SelectAll();
		}

		[CommandHandler(EditCommands.Copy)]
		protected void OnCopy()
		{
			TreePath[] selectedRows = base.Selection.GetSelectedRows();
			if (selectedRows == null || selectedRows.Length == 0)
			{
				return;
			}
			if (selectedRows.Length == 1 && IdeApp.Workbench.RootWindow.Focus is Editable editable)
			{
				editable.CopyClipboard();
				return;
			}
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < selectedRows.Length; i++)
			{
				if (store.GetIter(out var iter, selectedRows[i]))
				{
					string text = (string)store.GetValue(iter, 1);
					string text2 = (string)store.GetValue(iter, 0);
					string item = (string)store.GetValue(iter, 2);
					num = Math.Max(num, text.Length);
					num2 = Math.Max(num2, text2.Length);
					list.Add(text);
					list2.Add(text2);
					list3.Add(item);
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < list.Count; j++)
			{
				if (j > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(list2[j]);
				if (list2[j].Length < num2)
				{
					stringBuilder.Append(new string(' ', num2 - list2[j].Length));
				}
				stringBuilder.Append('\t');
				stringBuilder.Append(list[j]);
				if (list[j].Length < num)
				{
					stringBuilder.Append(new string(' ', num - list[j].Length));
				}
				stringBuilder.Append('\t');
				stringBuilder.Append(list3[j]);
			}
			Gtk.Clipboard.Get(Gdk.Selection.Clipboard).Text = stringBuilder.ToString();
		}

		[CommandHandler(EditCommands.Delete)]
		[CommandHandler(EditCommands.DeleteKey)]
		protected void OnDelete()
		{
			TreePath[] selectedRows = base.Selection.GetSelectedRows();
			foreach (TreePath path in selectedRows)
			{
				if (store.GetIter(out var iter, path))
				{
					string item = (string)store.GetValue(iter, 0);
					cachedValues.Remove(item);
					valueNames.Remove(item);
				}
			}
			Refresh(resetScrollPosition: true);
		}

		[CommandUpdateHandler(EditCommands.DeleteKey)]
		[CommandUpdateHandler(EditCommands.Delete)]
		protected void OnUpdateDelete(CommandInfo cinfo)
		{
			if (editing)
			{
				cinfo.Bypass = true;
				return;
			}
			if (!AllowAdding)
			{
				cinfo.Visible = false;
				return;
			}
			TreePath[] selectedRows = base.Selection.GetSelectedRows();
			if (selectedRows.Length == 0)
			{
				cinfo.Enabled = false;
				return;
			}
			TreePath[] array = selectedRows;
			foreach (TreePath treePath in array)
			{
				if (treePath.Depth > 1)
				{
					cinfo.Enabled = false;
					break;
				}
			}
		}

		[CommandHandler(DebugCommands.AddWatch)]
		protected void OnAddWatch()
		{
			List<string> list = new List<string>();
			TreePath[] selectedRows = base.Selection.GetSelectedRows();
			foreach (TreePath path in selectedRows)
			{
				if (store.GetIter(out var iter, path))
				{
					string fullExpression = GetFullExpression(iter);
					if (!string.IsNullOrEmpty(fullExpression))
					{
						list.Add(fullExpression);
					}
				}
			}
			foreach (string item in list)
			{
				DebuggingService.AddWatch(item);
			}
		}

		[CommandUpdateHandler(DebugCommands.AddWatch)]
		protected void OnUpdateAddWatch(CommandInfo cinfo)
		{
			cinfo.Enabled = base.Selection.GetSelectedRows().Length > 0;
		}

		[CommandHandler(EditCommands.Rename)]
		protected void OnRename()
		{
			if (store.GetIter(out var iter, base.Selection.GetSelectedRows()[0]))
			{
				SetCursor(store.GetPath(iter), base.Columns[0], start_editing: true);
			}
		}

		[CommandUpdateHandler(EditCommands.Rename)]
		protected void OnUpdateRename(CommandInfo cinfo)
		{
			cinfo.Visible = AllowAdding;
			cinfo.Enabled = base.Selection.GetSelectedRows().Length == 1;
		}

		protected override void OnRowActivated(TreePath path, TreeViewColumn column)
		{
			base.OnRowActivated(path, column);
			if (!CanQueryDebugger)
			{
				return;
			}
			TreePath[] selectedRows = base.Selection.GetSelectedRows();
			if (store.GetIter(out var iter, selectedRows[0]))
			{
				ObjectValue objectValue = (ObjectValue)store.GetValue(iter, 3);
				if (objectValue != null && objectValue.Name == DebuggingService.DebuggerSession.EvaluationOptions.CurrentExceptionTag)
				{
					DebuggingService.ShowExceptionCaughtDialog();
				}
			}
		}

		private bool GetCellAtPos(int x, int y, out TreePath path, out TreeViewColumn col, out CellRenderer cellRenderer)
		{
			if (GetPathAtPos(x, y, out path, out col))
			{
				x -= GetCellArea(path, col).X;
				CellRenderer[] cellRenderers = col.CellRenderers;
				foreach (CellRenderer cellRenderer2 in cellRenderers)
				{
					col.CellGetPosition(cellRenderer2, out var start_pos, out var width);
					if (cellRenderer2.Visible && x >= start_pos && x < start_pos + width)
					{
						cellRenderer = cellRenderer2;
						return true;
					}
				}
			}
			cellRenderer = null;
			return false;
		}

		private string GetFullExpression(TreeIter it)
		{
			TreePath path = store.GetPath(it);
			string text = "";
			while (path.Depth != 1)
			{
				ObjectValue objectValue = (ObjectValue)store.GetValue(it, 3);
				if (objectValue == null)
				{
					return null;
				}
				text = objectValue.ChildSelector + text;
				if (!store.IterParent(out it, it))
				{
					break;
				}
				path = store.GetPath(it);
			}
			string text2 = (string)store.GetValue(it, 0);
			return text2 + text;
		}

		public void CreatePinnedWatch(TreeIter it)
		{
			string fullExpression = GetFullExpression(it);
			if (!string.IsNullOrEmpty(fullExpression))
			{
				PinnedWatch pinnedWatch = new PinnedWatch();
				if (PinnedWatch != null)
				{
					CollapseAll();
					pinnedWatch.File = PinnedWatch.File;
					pinnedWatch.Line = PinnedWatch.Line;
					pinnedWatch.OffsetX = PinnedWatch.OffsetX;
					pinnedWatch.OffsetY = PinnedWatch.OffsetY + SizeRequest().Height + 5;
				}
				else
				{
					pinnedWatch.File = PinnedWatchFile;
					pinnedWatch.Line = PinnedWatchLine;
					pinnedWatch.OffsetX = -1;
					pinnedWatch.OffsetY = -1;
				}
				pinnedWatch.Expression = fullExpression;
				DebuggingService.PinnedWatches.Add(pinnedWatch);
				if (PinStatusChanged != null)
				{
					PinStatusChanged(this, EventArgs.Empty);
				}
			}
		}

		public void RemovePinnedWatch(TreeIter it)
		{
			DebuggingService.PinnedWatches.Remove(PinnedWatch);
			if (PinStatusChanged != null)
			{
				PinStatusChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnCompletionContextChanged(EventArgs e)
		{
			CompletionContextChanged?.Invoke(this, e);
		}

		string ICompletionWidget.GetText(int startOffset, int endOffset)
		{
			string text = editEntry.Text;
			if (startOffset < 0 || endOffset < 0 || startOffset > endOffset || startOffset >= text.Length)
			{
				return "";
			}
			int length = Math.Min(endOffset - startOffset, text.Length - startOffset);
			return text.Substring(startOffset, length);
		}

		void ICompletionWidget.Replace(int offset, int count, string text)
		{
			if (count > 0)
			{
				editEntry.Text = editEntry.Text.Remove(offset, count);
			}
			if (!string.IsNullOrEmpty(text))
			{
				editEntry.Text = editEntry.Text.Insert(offset, text);
			}
		}

		char ICompletionWidget.GetChar(int offset)
		{
			string text = editEntry.Text;
			if (offset < text.Length)
			{
				return text[offset];
			}
			return '\0';
		}

		CodeCompletionContext ICompletionWidget.CreateCodeCompletionContext(int triggerOffset)
		{
			CodeCompletionContext codeCompletionContext = new CodeCompletionContext();
			codeCompletionContext.TriggerLine = 0;
			codeCompletionContext.TriggerOffset = triggerOffset;
			codeCompletionContext.TriggerLineOffset = codeCompletionContext.TriggerOffset;
			codeCompletionContext.TriggerTextHeight = editEntry.SizeRequest().Height;
			codeCompletionContext.TriggerWordLength = currentCompletionData.ExpressionLength;
			editEntry.GdkWindow.GetOrigin(out var x, out var y);
			editEntry.GetLayoutOffsets(out var x2, out var _);
			int index_ = editEntry.TextIndexToLayoutIndex(editEntry.Position);
			Pango.Rectangle rectangle = editEntry.Layout.IndexToPos(index_);
			x2 += Units.ToPixels(rectangle.X) + x;
			y += editEntry.Allocation.Height;
			codeCompletionContext.TriggerXCoord = x2;
			codeCompletionContext.TriggerYCoord = y;
			return codeCompletionContext;
		}

		string ICompletionWidget.GetCompletionText(CodeCompletionContext ctx)
		{
			return editEntry.Text.Substring(ctx.TriggerOffset, ctx.TriggerWordLength);
		}

		void ICompletionWidget.SetCompletionText(CodeCompletionContext ctx, string partial_word, string complete_word)
		{
			int position = editEntry.Position - partial_word.Length;
			editEntry.DeleteText(position, position + partial_word.Length);
			editEntry.InsertText(complete_word, ref position);
			editEntry.Position = position;
		}

		void ICompletionWidget.SetCompletionText(CodeCompletionContext ctx, string partial_word, string complete_word, int offset)
		{
			int position = editEntry.Position - partial_word.Length;
			editEntry.DeleteText(position, position + partial_word.Length);
			editEntry.InsertText(complete_word, ref position);
			editEntry.Position = position + offset;
		}

		private ObjectValue[] GetValues(string[] names)
		{
			ObjectValue[] array = new ObjectValue[names.Length];
			List<string> list = new List<string>();
			for (int i = 0; i < names.Length; i++)
			{
				if (cachedValues.TryGetValue(names[i], out var value))
				{
					array[i] = value;
				}
				else
				{
					list.Add(names[i]);
				}
			}
			ObjectValue[] array2;
			if (frame != null)
			{
				array2 = frame.GetExpressionValues(list.ToArray(), evaluateMethods: true);
			}
			else
			{
				array2 = new ObjectValue[list.Count];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = ObjectValue.CreateUnknown(list[j]);
				}
			}
			int num = 0;
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k] == null)
				{
					array[k] = array2[num++];
					cachedValues[names[k]] = array[k];
				}
			}
			return array;
		}

		private Mono.Debugging.Client.CompletionData GetCompletionData(string exp)
		{
			if (CanQueryDebugger && frame != null)
			{
				return frame.GetExpressionCompletionData(exp);
			}
			return null;
		}

		internal void SetCustomFont(FontDescription font)
		{
			CellRendererRoundedButton cellRendererRoundedButton = crpButton;
			CellRendererTextWithIcon cellRendererTextWithIcon = crtExp;
			CellRendererText cellRendererText = crtType;
			FontDescription fontDescription = (crtValue.FontDesc = font);
			FontDescription fontDescription3 = (cellRendererText.FontDesc = fontDescription);
			FontDescription fontDesc = (cellRendererTextWithIcon.FontDesc = fontDescription3);
			cellRendererRoundedButton.FontDesc = fontDesc;
		}
	}
}
