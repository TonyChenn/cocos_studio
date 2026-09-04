using System;
using Gdk;
using Gtk;
using Mono.TextEditor;
using MonoDevelop.Core;
using Stetic;

namespace MonoDevelop.SourceEditor
{
	public class GotoLineNumberWidget : Bin
	{
		private readonly TextEditor textEditor;

		private readonly Widget frame;

		private double vSave;

		private double hSave;

		private DocumentLocation caretSave;

		private bool cleanExit;

		internal static readonly Color warningColor = new Color(210, 210, 32);

		internal static readonly Color errorColor = new Color(byte.MaxValue, 102, 102);

		private HBox hbox1;

		private Entry entryLineNumber;

		private Button buttonGoToLine;

		private EventBox eventbox2;

		private Gtk.Image image2;

		private Button closeButton;

		private EventBox eventbox1;

		private Gtk.Image image1;

		private int TargetLine
		{
			get
			{
				int num;
				try
				{
					string s = entryLineNumber.Text.Split(',', ':')[0];
					num = int.Parse(s);
				}
				catch (OverflowException)
				{
					num = (entryLineNumber.Text.Trim().StartsWith("-", StringComparison.Ordinal) ? int.MinValue : int.MaxValue);
				}
				if (!entryLineNumber.Text.Trim().StartsWith("-", StringComparison.Ordinal) && !entryLineNumber.Text.Trim().StartsWith("+", StringComparison.Ordinal))
				{
					return num;
				}
				return caretSave.Line + num;
			}
		}

		private int TargetColumn
		{
			get
			{
				int num;
				try
				{
					string[] array = entryLineNumber.Text.Split(',', ':');
					if (array.Length != 2)
					{
						return 0;
					}
					string s = array[1];
					num = int.Parse(s);
				}
				catch (OverflowException)
				{
					num = (entryLineNumber.Text.Trim().StartsWith("-", StringComparison.Ordinal) ? int.MinValue : int.MaxValue);
				}
				if (!entryLineNumber.Text.Trim().StartsWith("-", StringComparison.Ordinal) && !entryLineNumber.Text.Trim().StartsWith("+", StringComparison.Ordinal))
				{
					return num;
				}
				return caretSave.Column + num;
			}
		}

		private void HandleViewTextEditorhandleSizeAllocated(object o, SizeAllocatedArgs args)
		{
			int num = textEditor.Allocation.Width - base.Allocation.Width - 8;
			TextEditor.EditorContainerChild editorContainerChild = (TextEditor.EditorContainerChild)textEditor[frame];
			if (num != editorContainerChild.X)
			{
				entryLineNumber.WidthRequest = textEditor.Allocation.Width / 4;
				editorContainerChild.X = num;
				textEditor.QueueResize();
			}
		}

		private void CloseWidget()
		{
			Destroy();
		}

		public GotoLineNumberWidget(TextEditor textEditor, Widget frame)
		{
			this.textEditor = textEditor;
			this.frame = frame;
			Build();
			StoreWidgetState();
			textEditor.Parent.SizeAllocated += HandleViewTextEditorhandleSizeAllocated;
			if (Platform.IsMac)
			{
				EventBox[] array = new EventBox[2] { eventbox1, eventbox2 };
				foreach (EventBox eventBox in array)
				{
					eventBox.VisibleWindow = true;
					eventBox.ModifyBg(StateType.Normal, new Color(230, 230, 230));
				}
			}
			closeButton.Clicked += delegate
			{
				RestoreWidgetState();
				CloseWidget();
			};
			buttonGoToLine.Clicked += delegate
			{
				cleanExit = true;
				GotoLine();
				CloseWidget();
			};
			Widget[] children = base.Children;
			foreach (Widget widget in children)
			{
				widget.KeyPressEvent += delegate(object sender, KeyPressEventArgs args)
				{
					if (args.Event.Key == Gdk.Key.Escape)
					{
						RestoreWidgetState();
						CloseWidget();
					}
				};
			}
			Widget oldWidget = null;
			base.FocusChildSet += delegate(object sender, FocusChildSetArgs args)
			{
				if (args.Widget != null && oldWidget == null)
				{
					StoreWidgetState();
				}
				oldWidget = args.Widget;
			};
			entryLineNumber.Changed += delegate
			{
				PreviewLine();
			};
			entryLineNumber.Activated += delegate
			{
				cleanExit = true;
				GotoLine();
				CloseWidget();
			};
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();
			textEditor.Parent.SizeAllocated -= HandleViewTextEditorhandleSizeAllocated;
		}

		private void StoreWidgetState()
		{
			vSave = textEditor.VAdjustment.Value;
			hSave = textEditor.HAdjustment.Value;
			caretSave = textEditor.Caret.Location;
		}

		private void RestoreWidgetState()
		{
			if (!cleanExit)
			{
				textEditor.VAdjustment.Value = vSave;
				textEditor.HAdjustment.Value = hSave;
				textEditor.Caret.Location = caretSave;
			}
		}

		private void GotoLine()
		{
			try
			{
				textEditor.Caret.Line = TargetLine;
				int targetColumn = TargetColumn;
				if (targetColumn > 0)
				{
					textEditor.Caret.Column = targetColumn;
				}
				textEditor.CenterToCaret();
			}
			catch (Exception)
			{
			}
		}

		private void PreviewLine()
		{
			if (string.IsNullOrEmpty(entryLineNumber.Text) || entryLineNumber.Text == "+" || entryLineNumber.Text == "-")
			{
				entryLineNumber.ModifyBase(StateType.Normal, base.Style.Base(StateType.Normal));
				RestoreWidgetState();
				return;
			}
			try
			{
				int num = TargetLine;
				if (num >= textEditor.Document.LineCount || num < 0)
				{
					num = Math.Max(1, Math.Min(textEditor.Document.LineCount, num));
				}
				else
				{
					entryLineNumber.ModifyBase(StateType.Normal, base.Style.Base(StateType.Normal));
				}
				textEditor.Caret.Line = num;
				textEditor.CenterToCaret();
			}
			catch (Exception)
			{
				entryLineNumber.ModifyBase(StateType.Normal, errorColor);
			}
		}

		public void Focus()
		{
			if (!entryLineNumber.IsFocus)
			{
				entryLineNumber.IsFocus = true;
			}
			PreviewLine();
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			Stetic.BinContainer.Attach(this);
			base.Name = "MonoDevelop.SourceEditor.GotoLineNumberWidget";
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			entryLineNumber = new Entry();
			entryLineNumber.CanFocus = true;
			entryLineNumber.Name = "entryLineNumber";
			entryLineNumber.IsEditable = true;
			entryLineNumber.InvisibleChar = '●';
			hbox1.Add(entryLineNumber);
			Box.BoxChild boxChild = (Box.BoxChild)hbox1[entryLineNumber];
			boxChild.Position = 0;
			boxChild.Expand = false;
			buttonGoToLine = new Button();
			buttonGoToLine.CanDefault = true;
			buttonGoToLine.CanFocus = true;
			buttonGoToLine.Name = "buttonGoToLine";
			buttonGoToLine.Relief = ReliefStyle.None;
			eventbox2 = new EventBox();
			eventbox2.Name = "eventbox2";
			eventbox2.AboveChild = true;
			eventbox2.VisibleWindow = false;
			image2 = new Gtk.Image();
			image2.Name = "image2";
			image2.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-jump-to", IconSize.Menu);
			eventbox2.Add(image2);
			buttonGoToLine.Add(eventbox2);
			buttonGoToLine.Label = null;
			hbox1.Add(buttonGoToLine);
			Box.BoxChild boxChild2 = (Box.BoxChild)hbox1[buttonGoToLine];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			closeButton = new Button();
			closeButton.CanFocus = true;
			closeButton.Name = "closeButton";
			closeButton.Relief = ReliefStyle.None;
			eventbox1 = new EventBox();
			eventbox1.Name = "eventbox1";
			eventbox1.AboveChild = true;
			eventbox1.VisibleWindow = false;
			image1 = new Gtk.Image();
			image1.Name = "image1";
			image1.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-close", IconSize.Menu);
			eventbox1.Add(image1);
			closeButton.Add(eventbox1);
			closeButton.Label = null;
			hbox1.Add(closeButton);
			Box.BoxChild boxChild3 = (Box.BoxChild)hbox1[closeButton];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			Add(hbox1);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			Show();
		}
	}
}
