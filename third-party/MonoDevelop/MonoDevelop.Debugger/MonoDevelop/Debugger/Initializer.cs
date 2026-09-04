using System;
using System.IO;
using Mono.Debugging.Client;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Debugger
{
	public class Initializer : CommandHandler
	{
		private Document disassemblyDoc;

		private DisassemblyView disassemblyView;

		protected override void Run()
		{
			DebuggingService.CallStackChanged += OnStackChanged;
			DebuggingService.CurrentFrameChanged += OnFrameChanged;
			DebuggingService.ExecutionLocationChanged += OnExecLocationChanged;
			DebuggingService.DisassemblyRequested += OnShowDisassembly;
			IdeApp.CommandService.RegisterGlobalHandler(new GlobalRunMethodHandler());
		}

		private void OnExecLocationChanged(object s, EventArgs a)
		{
		}

		private void OnStackChanged(object s, EventArgs a)
		{
			if (disassemblyDoc == null || IdeApp.Workbench.ActiveDocument != disassemblyDoc)
			{
				SetSourceCodeFrame();
			}
		}

		private void OnFrameChanged(object s, EventArgs a)
		{
			if (disassemblyDoc != null && DebuggingService.IsFeatureSupported(DebuggerFeatures.Disassembly))
			{
				disassemblyView.Update();
			}
			StackFrame currentFrame = DebuggingService.CurrentFrame;
			if (currentFrame == null)
			{
				return;
			}
			FilePath filePath = currentFrame.SourceLocation.FileName;
			int line = currentFrame.SourceLocation.Line;
			if (line != -1)
			{
				if (!filePath.IsNullOrEmpty && File.Exists(filePath) && IdeApp.Workbench.OpenDocument(filePath, null, line, 1, OpenDocumentOptions.Debugger) != null)
				{
					return;
				}
				if (currentFrame.SourceLocation.FileHash != null)
				{
					FilePath filePath2 = SourceCodeLookup.FindSourceFile(filePath, currentFrame.SourceLocation.FileHash);
					if (filePath2 != null)
					{
						currentFrame.UpdateSourceFile(filePath2);
						if (IdeApp.Workbench.OpenDocument(filePath2, null, line, 1, OpenDocumentOptions.Debugger) != null)
						{
							return;
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(currentFrame.AddressSpace) && DebuggingService.CurrentSessionSupportsFeature(DebuggerFeatures.Disassembly))
			{
				if (disassemblyDoc == null)
				{
					OnShowDisassembly(null, null);
				}
				else
				{
					disassemblyDoc.Select();
				}
			}
		}

		private void OnShowDisassembly(object s, EventArgs a)
		{
			if (disassemblyDoc == null)
			{
				disassemblyView = new DisassemblyView();
				disassemblyDoc = IdeApp.Workbench.OpenDocument(disassemblyView, bringToFront: true);
				disassemblyDoc.Closed += delegate
				{
					disassemblyDoc = null;
					disassemblyView = null;
				};
			}
			else
			{
				disassemblyDoc.Select();
			}
			disassemblyView.Update();
		}

		private static void SetSourceCodeFrame()
		{
			Backtrace currentCallStack = DebuggingService.CurrentCallStack;
			if (currentCallStack == null)
			{
				return;
			}
			for (int i = 0; i < currentCallStack.FrameCount; i++)
			{
				StackFrame frame = currentCallStack.GetFrame(i);
				if (!frame.IsExternalCode && frame.SourceLocation.Line != -1 && !string.IsNullOrEmpty(frame.SourceLocation.FileName) && File.Exists(frame.SourceLocation.FileName))
				{
					if (i != DebuggingService.CurrentFrameIndex)
					{
						DebuggingService.CurrentFrameIndex = i;
					}
					break;
				}
			}
		}
	}
}
