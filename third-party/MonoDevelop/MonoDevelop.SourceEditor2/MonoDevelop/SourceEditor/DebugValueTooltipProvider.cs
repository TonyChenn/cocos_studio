using System;
using System.Collections.Generic;
using Cairo;
using Gdk;
using Gtk;
using Mono.Debugging.Client;
using Mono.TextEditor;
using MonoDevelop.Components;
using MonoDevelop.Debugger;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.SourceEditor
{
	public class DebugValueTooltipProvider : TooltipProvider, IDisposable
	{
		private Dictionary<string, ObjectValue> cachedValues = new Dictionary<string, ObjectValue>();

		private DebugValueWindow tooltip;

		public DebugValueTooltipProvider()
		{
			DebuggingService.CurrentFrameChanged += CurrentFrameChanged;
			DebuggingService.DebugSessionStarted += DebugSessionStarted;
		}

		private void DebugSessionStarted(object sender, EventArgs e)
		{
			DebuggingService.DebuggerSession.TargetExited += TargetProcessExited;
		}

		private void CurrentFrameChanged(object sender, EventArgs e)
		{
			cachedValues.Clear();
			if (tooltip != null)
			{
				tooltip.Hide();
			}
		}

		private void TargetProcessExited(object sender, EventArgs e)
		{
			if (tooltip != null)
			{
				tooltip.Destroy();
				tooltip = null;
			}
		}

		public override TooltipItem GetItem(TextEditor editor, int offset)
		{
			if (offset >= editor.Document.TextLength)
			{
				return null;
			}
			if (!DebuggingService.IsDebugging || DebuggingService.IsRunning)
			{
				return null;
			}
			StackFrame currentFrame = DebuggingService.CurrentFrame;
			if (currentFrame == null)
			{
				return null;
			}
			ExtensibleTextEditor extensibleTextEditor = (ExtensibleTextEditor)editor;
			string text = null;
			int startOffset;
			if (extensibleTextEditor.IsSomethingSelected && offset >= extensibleTextEditor.SelectionRange.Offset && offset <= extensibleTextEditor.SelectionRange.EndOffset)
			{
				startOffset = extensibleTextEditor.SelectionRange.Offset;
				text = extensibleTextEditor.SelectedText;
			}
			else
			{
				Document activeDocument = IdeApp.Workbench.ActiveDocument;
				if (activeDocument == null || activeDocument.ParsedDocument == null)
				{
					return null;
				}
				IDebuggerExpressionResolver content = activeDocument.GetContent<IDebuggerExpressionResolver>();
				TextEditorData textEditorData = editor.GetTextEditorData();
				if (content != null)
				{
					text = content.ResolveExpression(textEditorData, activeDocument, offset, out startOffset);
				}
				else
				{
					int num = textEditorData.FindCurrentWordEnd(offset);
					startOffset = textEditorData.FindCurrentWordStart(offset);
					text = textEditorData.GetTextAt(startOffset, num - startOffset);
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			if (!cachedValues.TryGetValue(text, out var value))
			{
				EvaluationOptions evaluationOptions = DebuggingService.DebuggerSession.EvaluationOptions.Clone();
				evaluationOptions.AllowMethodEvaluation = true;
				evaluationOptions.AllowTargetInvoke = true;
				value = currentFrame.GetExpressionValue(text, evaluationOptions);
				cachedValues[text] = value;
			}
			if (value == null || value.IsUnknown || value.IsNotSupported)
			{
				return null;
			}
			value.Name = text;
			return new TooltipItem(value, startOffset, text.Length);
		}

		public override Gtk.Window ShowTooltipWindow(TextEditor editor, int offset, ModifierType modifierState, int mouseX, int mouseY, TooltipItem item)
		{
			DocumentLocation loc = editor.OffsetToLocation(item.ItemSegment.Offset);
			Cairo.Point point = editor.LocationToPoint(loc);
			int num = (int)editor.LineHeight;
			int i;
			for (i = point.Y; i + num < mouseY; i += num)
			{
			}
			Gdk.Rectangle caret = new Gdk.Rectangle(mouseX, i, 1, num);
			tooltip = new DebugValueWindow(editor, offset, DebuggingService.CurrentFrame, (ObjectValue)item.Item, null);
			tooltip.ShowPopup(editor, caret, PopupPosition.TopLeft);
			return tooltip;
		}

		public override bool IsInteractive(TextEditor editor, Gtk.Window tipWindow)
		{
			return DebuggingService.IsDebugging;
		}

		public void Dispose()
		{
			DebuggingService.CurrentFrameChanged -= CurrentFrameChanged;
			DebuggingService.DebugSessionStarted -= DebugSessionStarted;
		}
	}
}
