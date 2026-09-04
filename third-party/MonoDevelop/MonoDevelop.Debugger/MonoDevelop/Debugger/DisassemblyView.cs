using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GLib;
using Gdk;
using Gtk;
using Mono.Debugging.Client;
using Mono.TextEditor;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Ide.Gui.Dialogs;

namespace MonoDevelop.Debugger
{
	public class DisassemblyView : AbstractViewContent, IClipboardHandler
	{
		private const int FillMarginLines = 50;

		private ScrolledWindow sw;

		private TextEditor editor;

		private int firstLine;

		private int lastLine;

		private Dictionary<string, int> addressLines = new Dictionary<string, int>();

		private bool autoRefill;

		private CurrentDebugLineTextMarker currentDebugLineMarker;

		private bool dragging;

		private FilePath currentFile;

		private AsmLineMarker asmMarker = new AsmLineMarker();

		private List<AssemblyLine> cachedLines = new List<AssemblyLine>();

		private string cachedLinesAddrSpace;

		private OverlayMessageWindow messageOverlayWindow;

		public override string TabPageLabel => GettextCatalog.GetString("Disassembly");

		public override Widget Control => sw;

		public override bool IsFile => false;

		public override bool IsReadOnly => true;

		bool IClipboardHandler.EnableCut => false;

		bool IClipboardHandler.EnableCopy => !editor.SelectionRange.IsEmpty;

		bool IClipboardHandler.EnablePaste => false;

		bool IClipboardHandler.EnableDelete => false;

		bool IClipboardHandler.EnableSelectAll => true;

		public DisassemblyView()
		{
			ContentName = GettextCatalog.GetString("Disassembly");
			sw = new ScrolledWindow();
			editor = new TextEditor();
			editor.Document.ReadOnly = true;
			editor.Options = new CommonTextEditorOptions
			{
				ShowLineNumberMargin = false
			};
			sw.Add(editor);
			sw.HscrollbarPolicy = PolicyType.Automatic;
			sw.VscrollbarPolicy = PolicyType.Automatic;
			sw.ShowAll();
			sw.Vadjustment.ValueChanged += OnScrollEditor;
			sw.VScrollbar.ButtonPressEvent += OnPress;
			sw.VScrollbar.ButtonReleaseEvent += OnRelease;
			sw.VScrollbar.Events |= EventMask.ButtonPressMask | EventMask.ButtonReleaseMask;
			sw.ShadowType = ShadowType.In;
			sw.Sensitive = false;
			currentDebugLineMarker = new CurrentDebugLineTextMarker(editor);
			DebuggingService.StoppedEvent += OnStop;
		}

		private void ShowLoadSourceFile(StackFrame sf)
		{
			if (messageOverlayWindow != null)
			{
				messageOverlayWindow.Destroy();
				messageOverlayWindow = null;
			}
			messageOverlayWindow = new OverlayMessageWindow();
			HBox hbox = new HBox();
			hbox.Spacing = 8;
			Label label = new Label($"{Path.GetFileName(sf.SourceLocation.FileName)} not found. Find source file at alternative location.");
			hbox.TooltipText = sf.SourceLocation.FileName;
			HslColor hslColor = editor.ColorStyle.NotificationText.Foreground;
			label.ModifyFg(StateType.Normal, hslColor);
			label.Layout.GetPixelSize(out var w, out var _);
			hbox.PackStart(label, expand: true, fill: true, 0u);
			Button openButton = new Button(Gtk.Stock.Open);
			openButton.WidthRequest = 60;
			hbox.PackEnd(openButton, expand: false, fill: false, 0u);
			HBox hBox = new HBox();
			hBox.PackStart(hbox, expand: true, fill: true, 8u);
			messageOverlayWindow.Child = hBox;
			messageOverlayWindow.ShowOverlay(editor);
			messageOverlayWindow.SizeFunc = () => openButton.SizeRequest().Width + w + hbox.Spacing * 5 + 16;
			openButton.Clicked += delegate
			{
				OpenFileDialog openFileDialog = new OpenFileDialog(GettextCatalog.GetString("File to Open"), FileChooserAction.Open)
				{
					TransientFor = IdeApp.Workbench.RootWindow,
					ShowEncodingSelector = true,
					ShowViewerSelector = true
				};
				if (!openFileDialog.Run())
				{
					return;
				}
				FilePath selectedFile = openFileDialog.SelectedFile;
				try
				{
					if (File.Exists(selectedFile))
					{
						if (SourceCodeLookup.CheckFileMd5(selectedFile, sf.SourceLocation.FileHash))
						{
							SourceCodeLookup.AddLoadedFile(selectedFile, sf.SourceLocation.FileName);
							sf.UpdateSourceFile(selectedFile);
							if (IdeApp.Workbench.OpenDocument(selectedFile, null, sf.SourceLocation.Line, 1, OpenDocumentOptions.Debugger) != null)
							{
								WorkbenchWindow.CloseWindow(force: false);
							}
						}
						else
						{
							MessageService.ShowWarning("File checksum doesn't match.");
						}
					}
					else
					{
						MessageService.ShowWarning("File not found.");
					}
				}
				catch (Exception)
				{
					MessageService.ShowWarning("Error opening file");
				}
			};
		}

		public override void Load(string fileName)
		{
		}

		public void Update()
		{
			autoRefill = false;
			editor.Document.RemoveMarker(currentDebugLineMarker);
			if (DebuggingService.CurrentFrame == null)
			{
				if (messageOverlayWindow != null)
				{
					messageOverlayWindow.Destroy();
					messageOverlayWindow = null;
				}
				sw.Sensitive = false;
				return;
			}
			sw.Sensitive = true;
			StackFrame currentFrame = DebuggingService.CurrentFrame;
			if (!string.IsNullOrWhiteSpace(currentFrame.SourceLocation.FileName) && currentFrame.SourceLocation.Line != -1 && currentFrame.SourceLocation.FileHash != null)
			{
				ShowLoadSourceFile(currentFrame);
			}
			else if (messageOverlayWindow != null)
			{
				messageOverlayWindow.Destroy();
				messageOverlayWindow = null;
			}
			if (!string.IsNullOrEmpty(currentFrame.SourceLocation.FileName) && File.Exists(currentFrame.SourceLocation.FileName))
			{
				FillWithSource();
			}
			else
			{
				Fill();
			}
		}

		public void FillWithSource()
		{
			cachedLines.Clear();
			StackFrame currentFrame = DebuggingService.CurrentFrame;
			if (currentFile != (FilePath)currentFrame.SourceLocation.FileName)
			{
				AssemblyLine[] array = DebuggingService.DebuggerSession.DisassembleFile(currentFrame.SourceLocation.FileName);
				if (array == null)
				{
					Fill();
					return;
				}
				currentFile = currentFrame.SourceLocation.FileName;
				addressLines.Clear();
				editor.Document.Text = string.Empty;
				StreamReader streamReader = new StreamReader(currentFrame.SourceLocation.FileName);
				int num = 1;
				int num2 = 0;
				int item = 1;
				StringBuilder stringBuilder = new StringBuilder();
				List<int> list = new List<int>();
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					InsertSourceLine(stringBuilder, item++, text);
					while (num2 < array.Length && array[num2].SourceLine == num)
					{
						list.Add(item);
						InsertAssemblerLine(stringBuilder, item++, array[num2++]);
					}
					num++;
				}
				editor.Document.Text = stringBuilder.ToString();
				foreach (int item2 in list)
				{
					editor.Document.AddMarker(item2, asmMarker);
				}
			}
			if (addressLines.TryGetValue(GetAddrId(currentFrame.Address, currentFrame.AddressSpace), out var _))
			{
				UpdateCurrentLineMarker(moveCaret: true);
			}
		}

		private string GetAddrId(long addr, string addrSpace)
		{
			return addrSpace + " " + addr;
		}

		private void InsertSourceLine(StringBuilder sb, int line, string text)
		{
			sb.Append(text).Append('\n');
		}

		private void InsertAssemblerLine(StringBuilder sb, int line, AssemblyLine asm)
		{
			sb.AppendFormat("{0:x8}   {1}\n", asm.Address, asm.Code);
			addressLines[GetAddrId(asm.Address, asm.AddressSpace)] = line;
		}

		public void Fill()
		{
			currentFile = null;
			StackFrame currentFrame = DebuggingService.CurrentFrame;
			if (cachedLines.Count > 0 && cachedLinesAddrSpace == currentFrame.AddressSpace && currentFrame.Address >= cachedLines[0].Address && currentFrame.Address <= cachedLines[cachedLines.Count - 1].Address)
			{
				autoRefill = true;
				UpdateCurrentLineMarker(moveCaret: true);
				return;
			}
			cachedLinesAddrSpace = currentFrame.AddressSpace;
			cachedLines.Clear();
			addressLines.Clear();
			firstLine = -150;
			lastLine = 150;
			editor.Document.MimeType = "text/plain";
			editor.Document.Text = string.Empty;
			InsertLines(0, firstLine, lastLine, out firstLine, out lastLine);
			autoRefill = true;
			UpdateCurrentLineMarker(moveCaret: true);
		}

		private void UpdateCurrentLineMarker(bool moveCaret)
		{
			editor.Document.RemoveMarker(currentDebugLineMarker);
			StackFrame currentFrame = DebuggingService.CurrentFrame;
			if (!addressLines.TryGetValue(GetAddrId(currentFrame.Address, currentFrame.AddressSpace), out var value))
			{
				return;
			}
			editor.Document.AddMarker(value, currentDebugLineMarker);
			if (moveCaret)
			{
				editor.Caret.Line = value;
				GLib.Timeout.Add(100u, delegate
				{
					editor.CenterToCaret();
					return false;
				});
			}
			editor.QueueDraw();
		}

		[ConnectBefore]
		private void OnPress(object s, EventArgs a)
		{
			dragging = true;
		}

		[ConnectBefore]
		private void OnRelease(object s, EventArgs a)
		{
			dragging = false;
			OnScrollEditor(null, null);
		}

		private void OnScrollEditor(object s, EventArgs args)
		{
			if (!autoRefill || dragging)
			{
				return;
			}
			DocumentLocation documentLocation = editor.PointToLocation(0.0, 0.0);
			DocumentLocation documentLocation2 = editor.PointToLocation(0.0, editor.Allocation.Height);
			if (firstLine != int.MinValue && documentLocation.Line < 50)
			{
				int num = (50 - documentLocation.Line) * 2;
				num = InsertLines(0, firstLine - num, firstLine - 1, out firstLine, out var _);
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (KeyValuePair<string, int> addressLine in addressLines)
				{
					dictionary[addressLine.Key] = addressLine.Value + num;
				}
				addressLines = dictionary;
				editor.Caret.Line += num;
				double num2 = (double)num * editor.LineHeight;
				sw.Vadjustment.Value += num2;
				UpdateCurrentLineMarker(moveCaret: false);
			}
			if (lastLine != int.MinValue && documentLocation2.Line >= editor.Document.LineCount - 50)
			{
				int num3 = (documentLocation2.Line - (editor.Document.LineCount - 50) + 1) * 2;
				InsertLines(editor.Document.TextLength, lastLine + 1, lastLine + num3, out var _, out lastLine);
			}
		}

		private int InsertLines(int offset, int start, int end, out int newStart, out int newEnd)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StackFrame currentFrame = DebuggingService.CurrentFrame;
			List<AssemblyLine> list = new List<AssemblyLine>(currentFrame.Disassemble(start, end - start + 1));
			int num = list.FindIndex((AssemblyLine al) => !al.IsOutOfRange);
			if (num == -1)
			{
				newStart = int.MinValue;
				newEnd = int.MinValue;
				return 0;
			}
			newStart = ((num == 0) ? start : int.MinValue);
			list.RemoveRange(0, num);
			int num2 = list.FindLastIndex((AssemblyLine al) => !al.IsOutOfRange);
			newEnd = ((num2 == list.Count - 1) ? end : int.MinValue);
			list.RemoveRange(num2 + 1, list.Count - num2 - 1);
			int num3 = 0;
			int num4 = editor.GetTextEditorData().OffsetToLineNumber(offset);
			foreach (AssemblyLine item in list)
			{
				if (!item.IsOutOfRange)
				{
					InsertAssemblerLine(stringBuilder, num4++, item);
					num3++;
				}
			}
			editor.Insert(offset, stringBuilder.ToString());
			editor.Document.CommitUpdateAll();
			if (offset == 0)
			{
				cachedLines.InsertRange(0, list);
			}
			else
			{
				cachedLines.AddRange(list);
			}
			return num3;
		}

		private void OnStop(object s, EventArgs args)
		{
			addressLines.Clear();
			currentFile = null;
			if (messageOverlayWindow != null)
			{
				messageOverlayWindow.Destroy();
				messageOverlayWindow = null;
			}
			sw.Sensitive = false;
			autoRefill = false;
			editor.Document.Text = string.Empty;
			cachedLines.Clear();
		}

		public override void Dispose()
		{
			base.Dispose();
			DebuggingService.StoppedEvent -= OnStop;
		}

		[CommandHandler(DebugCommands.StepOver)]
		protected void OnStepOver()
		{
			DebuggingService.DebuggerSession.NextInstruction();
		}

		[CommandHandler(DebugCommands.StepInto)]
		protected void OnStepInto()
		{
			DebuggingService.DebuggerSession.StepInstruction();
		}

		[CommandUpdateHandler(DebugCommands.StepInto)]
		[CommandUpdateHandler(DebugCommands.StepOver)]
		protected void OnUpdateStep(CommandInfo ci)
		{
			StackFrame currentFrame = DebuggingService.CurrentFrame;
			ci.Enabled = currentFrame != null && addressLines.ContainsKey(GetAddrId(currentFrame.Address, currentFrame.AddressSpace));
		}

		void IClipboardHandler.Cut()
		{
			throw new NotSupportedException();
		}

		void IClipboardHandler.Copy()
		{
			editor.RunAction(ClipboardActions.Copy);
		}

		void IClipboardHandler.Paste()
		{
			throw new NotSupportedException();
		}

		void IClipboardHandler.Delete()
		{
			throw new NotSupportedException();
		}

		void IClipboardHandler.SelectAll()
		{
			editor.RunAction(SelectionActions.SelectAll);
		}
	}
}
