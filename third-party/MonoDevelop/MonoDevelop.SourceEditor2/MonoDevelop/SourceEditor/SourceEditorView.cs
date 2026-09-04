using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Cairo;
using GLib;
using Gdk;
using Gtk;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.Addins;
using Mono.Debugging.Client;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;
using Mono.TextEditor.Utils;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Debugger;
using MonoDevelop.DesignerSupport.Toolbox;
using MonoDevelop.Ide;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Ide.CodeFormatting;
using MonoDevelop.Ide.CodeTemplates;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Ide.Tasks;
using MonoDevelop.Ide.TextEditing;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;
using MonoDevelop.Projects.Policies;
using MonoDevelop.Projects.Text;
using MonoDevelop.SourceEditor.QuickTasks;

namespace MonoDevelop.SourceEditor
{
	public class SourceEditorView : AbstractViewContent, IExtensibleTextEditor, IEditableTextBuffer, ITextBuffer, IEditableTextFile, ITextFile, IUndoHandler, IBookmarkBuffer, IClipboardHandler, ICompletionWidget, ISplittable, IFoldable, IToolboxDynamicProvider, IEncodedTextContent, ICustomFilteringToolboxConsumer, IToolboxConsumer, IZoomable, ITextEditorResolver, ITextEditorDataProvider, ICodeTemplateHandler, ICodeTemplateContextProvider, ISupportsProjectReload, IPrintable
	{
		private struct PinnedWatchInfo
		{
			public PinnedWatch Watch;

			public DocumentLine Line;

			public PinnedWatchWidget Widget;
		}

		private readonly SourceEditorWidget widget;

		private bool isDisposed;

		private DateTime lastSaveTimeUtc;

		private string loadedMimeType;

		internal object MemoryProbe = Counters.SourceViewsInMemory.CreateMemoryProbe();

		private TextLineMarker currentDebugLineMarker;

		private TextLineMarker debugStackLineMarker;

		private int lastDebugLine = -1;

		private BreakpointStore breakpoints;

		private EventHandler currentFrameChanged;

		private EventHandler executionLocationChanged;

		private EventHandler<BreakpointEventArgs> breakpointAdded;

		private EventHandler<BreakpointEventArgs> breakpointRemoved;

		private EventHandler<BreakpointEventArgs> breakpointStatusChanged;

		private List<DocumentLine> breakpointSegments = new List<DocumentLine>();

		private DocumentLine debugStackSegment;

		private DocumentLine currentLineSegment;

		private List<PinnedWatchInfo> pinnedWatches = new List<PinnedWatchInfo>();

		private bool writeAllowed;

		private bool writeAccessChecked;

		private uint autoSaveTimer;

		private bool wasEdited;

		private uint removeMarkerTimeout;

		private Queue<MessageBubbleTextMarker> markersToRemove = new Queue<MessageBubbleTextMarker>();

		private Dictionary<TopLevelWidgetExtension, Widget> widgetExtensions = new Dictionary<TopLevelWidgetExtension, Widget>();

		private Dictionary<FileExtension, Tuple<TextLineMarker, DocumentLine>> markerExtensions = new Dictionary<FileExtension, Tuple<TextLineMarker, DocumentLine>>();

		private MessageBubbleCache messageBubbleCache;

		private List<MessageBubbleTextMarker> currentErrorMarkers = new List<MessageBubbleTextMarker>();

		private Document ownerDocument;

		private bool warnOverwrite;

		private bool inLoad;

		private Encoding encoding;

		private bool hadBom;

		private string oldReplaceText;

		private static List<TextToolboxNode> clipboardRing;

		private Widget customSource;

		private ItemToolboxNode dragItem;

		public TextDocument Document => widget.TextEditor.Document;

		public DateTime LastSaveTimeUtc
		{
			get
			{
				return lastSaveTimeUtc;
			}
			internal set
			{
				lastSaveTimeUtc = value;
			}
		}

		public ExtensibleTextEditor TextEditor => widget.TextEditor;

		internal SourceEditorWidget SourceEditorWidget => widget;

		public override Widget Control
		{
			get
			{
				if (widget == null)
				{
					return null;
				}
				return widget.Vbox;
			}
		}

		public int LineCount => Document.LineCount;

		public override Project Project
		{
			get
			{
				return base.Project;
			}
			set
			{
				if (value != base.Project)
				{
					((StyledSourceEditorOptions)SourceEditorWidget.TextEditor.Options).UpdateStyleParent(value, loadedMimeType);
				}
				base.Project = value;
			}
		}

		public override string TabPageLabel => GettextCatalog.GetString("Source");

		protected Document OwnerDocument => ownerDocument;

		public Encoding SourceEncoding => encoding;

		public ITextEditorExtension Extension { get; set; }

		public bool EnableUndo
		{
			get
			{
				if (widget == null)
				{
					return false;
				}
				if (Document.CanUndo)
				{
					return widget.EditorHasFocus;
				}
				return false;
			}
		}

		public bool EnableRedo
		{
			get
			{
				if (widget == null)
				{
					return false;
				}
				if (Document.CanRedo)
				{
					return widget.EditorHasFocus;
				}
				return false;
			}
		}

		public string SelectedText
		{
			get
			{
				if (!TextEditor.IsSomethingSelected)
				{
					return "";
				}
				return Document.GetTextAt(TextEditor.SelectionRange);
			}
			set
			{
				TextEditor.DeleteSelectedText();
				int offset = TextEditor.Caret.Offset;
				int length = TextEditor.Insert(offset, value);
				TextEditor.SelectionRange = new TextSegment(offset, length);
			}
		}

		public bool HasInputFocus => TextEditor.HasFocus;

		public int CursorPosition
		{
			get
			{
				return TextEditor.Caret.Offset;
			}
			set
			{
				TextEditor.Caret.Offset = value;
			}
		}

		public int SelectionStartPosition
		{
			get
			{
				if (!TextEditor.IsSomethingSelected)
				{
					return TextEditor.Caret.Offset;
				}
				return TextEditor.SelectionRange.Offset;
			}
		}

		public int SelectionEndPosition
		{
			get
			{
				if (!TextEditor.IsSomethingSelected)
				{
					return TextEditor.Caret.Offset;
				}
				return TextEditor.SelectionRange.EndOffset;
			}
		}

		public FilePath Name => ContentName ?? UntitledName;

		public string Text
		{
			get
			{
				return widget.TextEditor.Document.Text;
			}
			set
			{
				IsDirty = true;
				TextDocument document = widget.TextEditor.Document;
				document.Replace(0, document.TextLength, value);
			}
		}

		public int Length => widget.TextEditor.Document.TextLength;

		public bool WarnOverwrite
		{
			get
			{
				return warnOverwrite;
			}
			set
			{
				warnOverwrite = value;
			}
		}

		public bool EnableCut => !widget.SearchWidgetHasFocus;

		public bool EnableCopy => EnableCut;

		public bool EnablePaste => EnableCut;

		public bool EnableDelete => EnableCut;

		public bool EnableSelectAll => EnableCut;

		public CodeCompletionContext CurrentCodeCompletionContext => CreateCodeCompletionContext(TextEditor.Caret.Offset);

		public int TextLength => Document.TextLength;

		public int SelectedLength
		{
			get
			{
				if (TextEditor.IsSomethingSelected)
				{
					if (TextEditor.MainSelection.SelectionMode == Mono.TextEditor.SelectionMode.Block)
					{
						return Math.Abs(TextEditor.MainSelection.Anchor.Column - TextEditor.MainSelection.Lead.Column);
					}
					return TextEditor.SelectionRange.Length;
				}
				return 0;
			}
		}

		public int CaretOffset => TextEditor.Caret.Offset;

		public Style GtkStyle => widget.Vbox.Style.Copy();

		public bool EnableSplitHorizontally => !EnableUnsplit;

		public bool EnableSplitVertically => !EnableUnsplit;

		public bool EnableUnsplit => widget.IsSplitted;

		public bool CanPrint => true;

		ToolboxItemFilterAttribute[] IToolboxConsumer.ToolboxFilterAttributes => new ToolboxItemFilterAttribute[0];

		public TargetEntry[] DragTargets => ClipboardActions.CopyOperation.TargetEntries;

		string IToolboxConsumer.DefaultItemDomain => "Text";

		bool IZoomable.EnableZoomIn => TextEditor.Options.CanZoomIn;

		bool IZoomable.EnableZoomOut => TextEditor.Options.CanZoomOut;

		bool IZoomable.EnableZoomReset => TextEditor.Options.CanResetZoom;

		ProjectReloadCapability ISupportsProjectReload.ProjectReloadCapability => ProjectReloadCapability.Full;

		public event EventHandler CaretPositionSet;

		public event EventHandler<TextChangedEventArgs> TextChanged;

		public event EventHandler CompletionContextChanged;

		private static event EventHandler ClipbardRingUpdated;

		public event EventHandler ItemsChanged;

		private void InformAutoSave()
		{
			RemoveAutoSaveTimer();
			autoSaveTimer = GLib.Timeout.Add(500u, delegate
			{
				AutoSave.InformAutoSaveThread(Document);
				autoSaveTimer = 0u;
				return false;
			});
		}

		private void RemoveAutoSaveTimer()
		{
			if (autoSaveTimer != 0)
			{
				Source.Remove(autoSaveTimer);
				autoSaveTimer = 0u;
			}
		}

		private void RemoveMarkerQueue()
		{
			if (removeMarkerTimeout != 0)
			{
				Source.Remove(removeMarkerTimeout);
			}
		}

		private void ResetRemoveMarker()
		{
			RemoveMarkerQueue();
			removeMarkerTimeout = GLib.Timeout.Add(2000u, delegate
			{
				while (markersToRemove.Count > 0)
				{
					MessageBubbleTextMarker messageBubbleTextMarker = markersToRemove.Dequeue();
					currentErrorMarkers.Remove(messageBubbleTextMarker);
					widget.TextEditor.Document.RemoveMarker(messageBubbleTextMarker);
				}
				removeMarkerTimeout = 0u;
				return false;
			});
		}

		public SourceEditorView()
		{
			++Counters.LoadedEditors;
			currentFrameChanged = DispatchService.GuiDispatch<EventHandler>(OnCurrentFrameChanged);
			executionLocationChanged = DispatchService.GuiDispatch<EventHandler>(OnExecutionLocationChanged);
			breakpointAdded = DispatchService.GuiDispatch<EventHandler<BreakpointEventArgs>>(OnBreakpointAdded);
			breakpointRemoved = DispatchService.GuiDispatch<EventHandler<BreakpointEventArgs>>(OnBreakpointRemoved);
			breakpointStatusChanged = DispatchService.GuiDispatch<EventHandler<BreakpointEventArgs>>(OnBreakpointStatusChanged);
			widget = new SourceEditorWidget(this);
			widget.TextEditor.Document.SyntaxModeChanged += HandleSyntaxModeChanged;
			widget.TextEditor.Document.TextReplaced += HandleTextReplaced;
			widget.TextEditor.Document.LineChanged += HandleLineChanged;
			widget.TextEditor.Document.BeginUndo += HandleBeginUndo;
			widget.TextEditor.Document.EndUndo += HandleEndUndo;
			widget.TextEditor.Document.Undone += HandleUndone;
			widget.TextEditor.Document.Redone += HandleUndone;
			widget.TextEditor.Document.TextReplacing += OnTextReplacing;
			widget.TextEditor.Document.TextReplaced += OnTextReplaced;
			widget.TextEditor.Document.ReadOnlyCheckDelegate = CheckReadOnly;
			widget.TextEditor.Caret.PositionChanged += HandlePositionChanged;
			widget.TextEditor.IconMargin.ButtonPressed += OnIconButtonPress;
			debugStackLineMarker = new DebugStackLineTextMarker(widget.TextEditor);
			currentDebugLineMarker = new CurrentDebugLineTextMarker(widget.TextEditor);
			base.WorkbenchWindowChanged += HandleWorkbenchWindowChanged;
			EventHandler value = delegate
			{
				Document.FileName = ContentName;
				if (!string.IsNullOrEmpty(ContentName) && File.Exists(ContentName))
				{
					lastSaveTimeUtc = File.GetLastWriteTimeUtc(ContentName);
				}
			};
			base.ContentNameChanged += value;
			ClipbardRingUpdated += UpdateClipboardRing;
			TextEditorService.FileExtensionAdded += HandleFileExtensionAdded;
			TextEditorService.FileExtensionRemoved += HandleFileExtensionRemoved;
			breakpoints = DebuggingService.Breakpoints;
			DebuggingService.DebugSessionStarted += OnDebugSessionStarted;
			DebuggingService.ExecutionLocationChanged += executionLocationChanged;
			DebuggingService.CurrentFrameChanged += currentFrameChanged;
			DebuggingService.StoppedEvent += currentFrameChanged;
			DebuggingService.ResumedEvent += currentFrameChanged;
			breakpoints.BreakpointAdded += breakpointAdded;
			breakpoints.BreakpointRemoved += breakpointRemoved;
			breakpoints.BreakpointStatusChanged += breakpointStatusChanged;
			breakpoints.BreakpointModified += breakpointStatusChanged;
			DebuggingService.PinnedWatches.WatchAdded += OnWatchAdded;
			DebuggingService.PinnedWatches.WatchRemoved += OnWatchRemoved;
			DebuggingService.PinnedWatches.WatchChanged += OnWatchChanged;
			TaskService.Errors.TasksAdded += UpdateTasks;
			TaskService.Errors.TasksRemoved += UpdateTasks;
			TaskService.JumpedToTask += HandleTaskServiceJumpedToTask;
			IdeApp.Preferences.ShowMessageBubblesChanged += HandleIdeAppPreferencesShowMessageBubblesChanged;
			TaskService.TaskToggled += HandleErrorListPadTaskToggled;
			widget.TextEditor.Options.Changed += HandleWidgetTextEditorOptionsChanged;
			IdeApp.Preferences.DefaultHideMessageBubblesChanged += HandleIdeAppPreferencesDefaultHideMessageBubblesChanged;
			Document.AddAnnotation(this);
			FileRegistry.Add(this);
		}

		private void HandleLineChanged(object sender, LineEventArgs e)
		{
			UpdateBreakpoints();
			UpdateWidgetPositions();
			if (messageBubbleCache != null && messageBubbleCache.RemoveLine(e.Line))
			{
				MessageBubbleTextMarker messageBubbleTextMarker = currentErrorMarkers.FirstOrDefault((MessageBubbleTextMarker m) => m.LineSegment == e.Line);
				if (messageBubbleTextMarker != null)
				{
					widget.TextEditor.TextViewMargin.RemoveCachedLine(e.Line);
					messageBubbleTextMarker.GetLineHeight(widget.TextEditor);
				}
			}
		}

		private void HandleTextReplaced(object sender, DocumentChangeEventArgs args)
		{
			if (Document.CurrentAtomicUndoOperationType == OperationType.Format)
			{
				return;
			}
			if (!inLoad)
			{
				if (widget.TextEditor.Document.IsInAtomicUndo)
				{
					wasEdited = true;
				}
				else
				{
					InformAutoSave();
				}
			}
			_ = args.Offset;
			foreach (MessageBubbleTextMarker currentErrorMarker in currentErrorMarkers)
			{
				if (currentErrorMarker.LineSegment.Contains(args.Offset) || currentErrorMarker.LineSegment.Contains(args.Offset + args.InsertionLength) || (args.Offset < currentErrorMarker.LineSegment.Offset && currentErrorMarker.LineSegment.Offset < args.Offset + args.InsertionLength))
				{
					markersToRemove.Enqueue(currentErrorMarker);
				}
			}
			ResetRemoveMarker();
		}

		private void HandleSyntaxModeChanged(object sender, SyntaxModeChangeEventArgs e)
		{
			if (e.OldMode is IQuickTaskProvider provider)
			{
				widget.RemoveQuickTaskProvider(provider);
			}
			if (e.NewMode is IQuickTaskProvider provider2)
			{
				widget.AddQuickTaskProvider(provider2);
			}
		}

		private void HandleEndUndo(object sender, TextDocument.UndoOperationEventArgs e)
		{
			if (wasEdited)
			{
				InformAutoSave();
			}
		}

		private void HandleBeginUndo(object sender, EventArgs e)
		{
			wasEdited = false;
		}

		private void HandleUndone(object sender, TextDocument.UndoOperationEventArgs e)
		{
			AutoSave.InformAutoSaveThread(Document);
		}

		private void HandlePositionChanged(object sender, DocumentLocationEventArgs e)
		{
			OnCaretPositionSet(EventArgs.Empty);
			FireCompletionContextChanged();
		}

		private void HandleFileExtensionRemoved(object sender, FileExtensionEventArgs args)
		{
			if (ContentName != null && !(args.Extension.File.FullPath != (FilePath)System.IO.Path.GetFullPath(ContentName)))
			{
				RemoveFileExtension(args.Extension);
			}
		}

		private void HandleFileExtensionAdded(object sender, FileExtensionEventArgs args)
		{
			if (ContentName != null && !(args.Extension.File.FullPath != (FilePath)System.IO.Path.GetFullPath(ContentName)))
			{
				AddFileExtension(args.Extension);
			}
		}

		private void LoadExtensions()
		{
			if (ContentName != null)
			{
				FileExtension[] fileExtensions = TextEditorService.GetFileExtensions(ContentName);
				foreach (FileExtension extension in fileExtensions)
				{
					AddFileExtension(extension);
				}
			}
		}

		private void AddFileExtension(FileExtension extension)
		{
			if (extension is TopLevelWidgetExtension)
			{
				TopLevelWidgetExtension topLevelWidgetExtension = (TopLevelWidgetExtension)extension;
				Widget widget = topLevelWidgetExtension.CreateWidget();
				if (!CalcWidgetPosition(topLevelWidgetExtension, widget, out var x, out var y))
				{
					widget.Destroy();
					return;
				}
				widgetExtensions[topLevelWidgetExtension] = widget;
				this.widget.TextEditor.TextArea.AddTopLevelWidget(widget, x, y);
				topLevelWidgetExtension.ScrollToViewRequested += HandleScrollToViewRequested;
			}
			else if (extension is TextLineMarkerExtension)
			{
				TextLineMarkerExtension textLineMarkerExtension = (TextLineMarkerExtension)extension;
				DocumentLine line = this.widget.TextEditor.Document.GetLine(textLineMarkerExtension.Line);
				if (line != null)
				{
					TextLineMarker textLineMarker = textLineMarkerExtension.CreateMarker();
					this.widget.TextEditor.Document.AddMarker(line, textLineMarker);
					this.widget.TextEditor.QueueDraw();
					markerExtensions[extension] = new Tuple<TextLineMarker, DocumentLine>(textLineMarker, line);
				}
			}
		}

		private void HandleScrollToViewRequested(object sender, EventArgs e)
		{
			TopLevelWidgetExtension key = (TopLevelWidgetExtension)sender;
			if (widgetExtensions.TryGetValue(key, out var value))
			{
				widget.TextEditor.TextArea.GetTopLevelWidgetPosition(value, out var x, out var y);
				Requisition size = value.SizeRequest();
				Application.Invoke(delegate
				{
					widget.TextEditor.ScrollTo(new Gdk.Rectangle(x, y, size.Width, size.Height));
				});
			}
		}

		private void RemoveFileExtension(FileExtension extension)
		{
			Tuple<TextLineMarker, DocumentLine> value2;
			if (extension is TopLevelWidgetExtension)
			{
				TopLevelWidgetExtension topLevelWidgetExtension = (TopLevelWidgetExtension)extension;
				if (widgetExtensions.TryGetValue(topLevelWidgetExtension, out var value))
				{
					widgetExtensions.Remove(topLevelWidgetExtension);
					widget.TextEditor.TextArea.Remove(value);
					value.Destroy();
					topLevelWidgetExtension.ScrollToViewRequested -= HandleScrollToViewRequested;
				}
			}
			else if (extension is TextLineMarkerExtension && markerExtensions.TryGetValue(extension, out value2))
			{
				widget.TextEditor.Document.RemoveMarker(value2.Item1);
			}
		}

		private void ClearExtensions()
		{
			foreach (TopLevelWidgetExtension key in widgetExtensions.Keys)
			{
				key.ScrollToViewRequested -= HandleScrollToViewRequested;
			}
		}

		private void UpdateWidgetPositions()
		{
			foreach (KeyValuePair<TopLevelWidgetExtension, Widget> widgetExtension in widgetExtensions)
			{
				if (CalcWidgetPosition(widgetExtension.Key, widgetExtension.Value, out var x, out var y))
				{
					widget.TextEditor.TextArea.MoveTopLevelWidget(widgetExtension.Value, x, y);
				}
				else
				{
					widgetExtension.Value.Hide();
				}
			}
		}

		private bool CalcWidgetPosition(TopLevelWidgetExtension widgetExtension, Widget w, out int x, out int y)
		{
			DocumentLine line = widget.TextEditor.Document.GetLine(widgetExtension.Line);
			if (line == null)
			{
				x = (y = 0);
				return false;
			}
			TextViewMargin.LayoutWrapper layout = widget.TextEditor.TextViewMargin.GetLayout(line);
			layout.Layout.GetPixelSize(out var width, out var height);
			if (layout.IsUncached)
			{
				layout.Dispose();
			}
			height = (int)TextEditor.TextViewMargin.GetLineHeight(widgetExtension.Line);
			x = (int)widget.TextEditor.TextViewMargin.XOffset + width + 4;
			y = (int)widget.TextEditor.LineToY(widgetExtension.Line);
			int num = (int)widget.TextEditor.TextViewMargin.XOffset;
			Requisition requisition = w.SizeRequest();
			switch (widgetExtension.HorizontalAlignment)
			{
			case HorizontalAlignment.LineLeft:
				x = (int)widget.TextEditor.TextViewMargin.XOffset;
				break;
			case HorizontalAlignment.LineRight:
				x = num + width + 4;
				break;
			case HorizontalAlignment.LineCenter:
				x = num + (width - requisition.Width) / 2;
				if (x < num)
				{
					x = num;
				}
				break;
			case HorizontalAlignment.Left:
				x = 0;
				break;
			}
			switch (widgetExtension.VerticalAlignment)
			{
			case VerticalAlignment.LineBottom:
				y += height - requisition.Height;
				break;
			case VerticalAlignment.LineCenter:
				y += (height - requisition.Height) / 2;
				break;
			case VerticalAlignment.AboveLine:
				y -= requisition.Height;
				break;
			case VerticalAlignment.BelowLine:
				y += height;
				break;
			}
			x += widgetExtension.OffsetX;
			y += widgetExtension.OffsetY;
			return true;
		}

		private void HandleWorkbenchWindowChanged(object sender, EventArgs e)
		{
			if (WorkbenchWindow != null)
			{
				widget.TextEditor.ExtensionContext = WorkbenchWindow.ExtensionContext;
				WorkbenchWindow.ActiveViewContentChanged += HandleActiveViewContentChanged;
				base.WorkbenchWindowChanged -= HandleWorkbenchWindowChanged;
			}
		}

		private void HandleActiveViewContentChanged(object o, ActiveViewContentEventArgs e)
		{
			widget.UpdateLineCol();
		}

		private void HandleWidgetTextEditorOptionsChanged(object sender, EventArgs e)
		{
			currentErrorMarkers.ForEach(delegate(MessageBubbleTextMarker marker)
			{
				marker.DisposeLayout();
			});
		}

		private void HandleTaskServiceJumpedToTask(object sender, TaskEventArgs e)
		{
			Task task = ((e.Tasks != null) ? e.Tasks.FirstOrDefault() : null);
			TextDocument document = Document;
			if (task == null || document == null || task.FileName != (FilePath)document.FileName || TextEditor == null)
			{
				return;
			}
			DocumentLine line = document.GetLine(task.Line);
			if (line == null)
			{
				return;
			}
			MessageBubbleTextMarker messageBubbleTextMarker = (MessageBubbleTextMarker)line.Markers.FirstOrDefault((TextLineMarker m) => m is MessageBubbleTextMarker);
			if (messageBubbleTextMarker != null)
			{
				messageBubbleTextMarker.SetPrimaryError(task.Description);
				if (TextEditor != null)
				{
					_ = TextEditor.IsComposited;
				}
			}
		}

		private void HandleIdeAppPreferencesDefaultHideMessageBubblesChanged(object sender, MonoDevelop.Core.PropertyChangedEventArgs e)
		{
			currentErrorMarkers.ForEach(delegate(MessageBubbleTextMarker marker)
			{
				marker.IsVisible = !IdeApp.Preferences.DefaultHideMessageBubbles;
			});
			TextEditor.QueueDraw();
		}

		private void HandleIdeAppPreferencesShowMessageBubblesChanged(object sender, MonoDevelop.Core.PropertyChangedEventArgs e)
		{
			UpdateTasks(null, null);
		}

		private void HandleErrorListPadTaskToggled(object sender, TaskEventArgs e)
		{
			TextEditor.QueueDraw();
		}

		private void UpdateTasks(object sender, TaskEventArgs e)
		{
			Task[] fileTasks = TaskService.Errors.GetFileTasks(ContentName);
			if (fileTasks == null)
			{
				return;
			}
			DisposeErrorMarkers();
			if (IdeApp.Preferences.ShowMessageBubbles == ShowMessageBubbles.Never)
			{
				return;
			}
			using (Document.OpenUndoGroup())
			{
				if (messageBubbleCache != null)
				{
					messageBubbleCache.Dispose();
				}
				messageBubbleCache = new MessageBubbleCache(widget.TextEditor);
				Task[] array = fileTasks;
				foreach (Task task in array)
				{
					if (task.Severity != TaskSeverity.Error && task.Severity != TaskSeverity.Warning)
					{
						continue;
					}
					if (IdeApp.Preferences.ShowMessageBubbles == ShowMessageBubbles.ForErrors && task.Severity == TaskSeverity.Warning)
					{
						continue;
					}
					DocumentLine lineSegment = widget.Document.GetLine(task.Line);
					if (lineSegment != null)
					{
						MessageBubbleTextMarker messageBubbleTextMarker = currentErrorMarkers.FirstOrDefault((MessageBubbleTextMarker m) => m.LineSegment == lineSegment);
						if (messageBubbleTextMarker != null)
						{
							messageBubbleTextMarker.AddError(task, task.Severity == TaskSeverity.Error, task.Description);
							continue;
						}
						MessageBubbleTextMarker messageBubbleTextMarker2 = new MessageBubbleTextMarker(messageBubbleCache, task, lineSegment, task.Severity == TaskSeverity.Error, task.Description);
						currentErrorMarkers.Add(messageBubbleTextMarker2);
						messageBubbleTextMarker2.IsVisible = !IdeApp.Preferences.DefaultHideMessageBubbles;
						widget.Document.AddMarker(lineSegment, messageBubbleTextMarker2, commitUpdate: false);
					}
				}
			}
			widget.TextEditor.QueueDraw();
		}

		private void DisposeErrorMarkers()
		{
			currentErrorMarkers.ForEach(delegate(MessageBubbleTextMarker em)
			{
				widget.Document.RemoveMarker(em);
				em.Dispose();
			});
			currentErrorMarkers.Clear();
			if (messageBubbleCache != null)
			{
				messageBubbleCache.Dispose();
				messageBubbleCache = null;
			}
		}

		protected virtual string ProcessSaveText(string text)
		{
			return text;
		}

		public override void Save(string fileName)
		{
			Save(fileName, encoding);
		}

		public void Save(string fileName, Encoding encoding)
		{
			if (widget.HasMessageBar)
			{
				return;
			}
			if (!string.IsNullOrEmpty(ContentName))
			{
				AutoSave.RemoveAutoSaveFile(ContentName);
			}
			if (ContentName != fileName)
			{
				FileService.RequestFileEdit(fileName);
				writeAllowed = true;
				writeAccessChecked = true;
			}
			if (warnOverwrite)
			{
				if (fileName == ContentName)
				{
					string primaryText = GettextCatalog.GetString("This file {0} has been changed outside of {1}. Are you sure you want to overwrite the file?", fileName, BrandingService.ApplicationName);
					if (MessageService.AskQuestion(primaryText, AlertButton.Cancel, AlertButton.OverwriteFile) != AlertButton.OverwriteFile)
					{
						return;
					}
				}
				warnOverwrite = false;
				widget.RemoveMessageBar();
				WorkbenchWindow.ShowNotification = false;
			}
			if (PropertyService.Get("AutoFormatDocumentOnSave", defaultValue: false))
			{
				try
				{
					CodeFormatter formatter = CodeFormatterService.GetFormatter(Document.MimeType);
					if (formatter != null && formatter.SupportsOnTheFlyFormatting)
					{
						using (TextEditor.OpenUndoGroup())
						{
							formatter.OnTheFlyFormat(WorkbenchWindow.Document, 0, Document.TextLength);
							wasEdited = false;
						}
					}
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Error while formatting on save", ex);
				}
			}
			FileRegistry.SuspendFileWatch = true;
			try
			{
				object obj = null;
				if (File.Exists(fileName))
				{
					try
					{
						obj = DesktopService.GetFileAttributes(fileName);
						FileAttributes attributes = File.GetAttributes(fileName);
						if (attributes.HasFlag(FileAttributes.ReadOnly))
						{
							AlertButton alertButton = MessageService.AskQuestion(GettextCatalog.GetString("Can't save file"), GettextCatalog.GetString("The file was marked as read only. Should the file be overwritten?"), AlertButton.Yes, AlertButton.No);
							if (alertButton != AlertButton.Yes)
							{
								return;
							}
							try
							{
								File.SetAttributes(fileName, attributes & ~FileAttributes.ReadOnly);
							}
							catch (Exception)
							{
								MessageService.ShowError(GettextCatalog.GetString("Error"), GettextCatalog.GetString("Operation failed."));
								return;
							}
						}
					}
					catch (Exception ex3)
					{
						LoggingService.LogWarning("Can't get file attributes", ex3);
					}
				}
				try
				{
					Encoding encoding2 = encoding;
					bool flag = hadBom;
					string text = ProcessSaveText(Document.Text);
					if (encoding2 == null)
					{
						if (this.encoding != null)
						{
							encoding2 = this.encoding;
						}
						else
						{
							encoding2 = Encoding.UTF8;
							flag = false;
						}
					}
					TextFileUtility.WriteText(fileName, text, encoding2, flag);
				}
				catch (InvalidEncodingException)
				{
					AlertButton alertButton2 = MessageService.AskQuestion(GettextCatalog.GetString("Can't save file with current codepage."), GettextCatalog.GetString("Some unicode characters in this file could not be saved with the current encoding.\nDo you want to resave this file as Unicode ?\nYou can choose another encoding in the 'save as' dialog."), 1, AlertButton.Cancel, new AlertButton(GettextCatalog.GetString("Save as Unicode")));
					if (alertButton2 == AlertButton.Cancel)
					{
						return;
					}
					hadBom = true;
					this.encoding = Encoding.UTF8;
					TextFileUtility.WriteText(fileName, Document.Text, encoding, hadBom);
				}
				lastSaveTimeUtc = File.GetLastWriteTimeUtc(fileName);
				try
				{
					if (obj != null)
					{
						DesktopService.SetFileAttributes(fileName, obj);
					}
				}
				catch (Exception ex5)
				{
					LoggingService.LogError("Can't set file attributes", ex5);
				}
			}
			catch (UnauthorizedAccessException ex6)
			{
				LoggingService.LogError("Error while saving file", ex6);
				MessageService.ShowError(GettextCatalog.GetString("Can't save file - access denied"), ex6.Message);
			}
			finally
			{
				FileRegistry.SuspendFileWatch = false;
			}
			ContentName = fileName;
			UpdateMimeType(fileName);
			Document.SetNotDirtyState();
			IsDirty = false;
		}

		public override void DiscardChanges()
		{
			if (!string.IsNullOrEmpty(ContentName))
			{
				AutoSave.RemoveAutoSaveFile(ContentName);
			}
		}

		public override void LoadNew(Stream content, string mimeType)
		{
			Document.MimeType = mimeType;
			string text = null;
			if (content != null)
			{
				text = TextFileUtility.GetText(content, out encoding, out hadBom);
				text = ProcessLoadText(text);
				Document.Text = text;
			}
			CreateDocumentParsedHandler();
			RunFirstTimeFoldUpdate(text);
			Document.InformLoadComplete();
		}

		public override void Load(string fileName)
		{
			Load(fileName, null);
		}

		private void RunFirstTimeFoldUpdate(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			ParsedDocument parsedDocument = null;
			IFoldingParser foldingParser = TypeSystemService.GetFoldingParser(Document.MimeType);
			if (foldingParser != null)
			{
				parsedDocument = foldingParser.Parse(Document.FileName, text);
			}
			else
			{
				TypeSystemParser parser = TypeSystemService.GetParser(Document.MimeType);
				if (parser != null)
				{
					using (StringReader content = new StringReader(text))
					{
						parsedDocument = parser.Parse(storeAst: true, Document.FileName, content);
					}
				}
			}
			if (parsedDocument != null)
			{
				widget.UpdateParsedDocument(parsedDocument);
			}
		}

		private void CreateDocumentParsedHandler()
		{
			base.WorkbenchWindowChanged += delegate
			{
				if (WorkbenchWindow != null)
				{
					WorkbenchWindow.DocumentChanged += delegate
					{
						if (WorkbenchWindow.Document != null)
						{
							foreach (IQuickTaskProvider content in WorkbenchWindow.Document.GetContents<IQuickTaskProvider>())
							{
								widget.AddQuickTaskProvider(content);
							}
							foreach (IUsageProvider content2 in WorkbenchWindow.Document.GetContents<IUsageProvider>())
							{
								widget.AddUsageTaskProvider(content2);
							}
							ownerDocument = WorkbenchWindow.Document;
							ownerDocument.DocumentParsed += HandleDocumentParsed;
						}
					};
				}
			};
		}

		protected virtual void HandleDocumentParsed(object sender, EventArgs e)
		{
			widget.UpdateParsedDocument(ownerDocument.ParsedDocument);
		}

		void IEncodedTextContent.Load(string fileName, Encoding loadEncoding)
		{
			Load(fileName, loadEncoding);
		}

		protected virtual string ProcessLoadText(string text)
		{
			return text;
		}

		public void Load(string fileName, Encoding loadEncoding, bool reload = false)
		{
			if (ContentName == fileName)
			{
				AutoSave.RemoveAutoSaveFile(fileName);
			}
			if (warnOverwrite)
			{
				warnOverwrite = false;
				widget.RemoveMessageBar();
				WorkbenchWindow.ShowNotification = false;
			}
			UpdateMimeType(fileName);
			string text = null;
			bool flag;
			if (AutoSave.AutoSaveExists(fileName))
			{
				widget.ShowAutoSaveWarning(fileName);
				encoding = loadEncoding;
				flag = false;
			}
			else
			{
				inLoad = true;
				if (loadEncoding == null)
				{
					text = TextFileUtility.ReadAllText(fileName, out hadBom, out encoding);
				}
				else
				{
					encoding = loadEncoding;
					text = TextFileUtility.ReadAllText(fileName, loadEncoding, out hadBom);
				}
				text = ProcessLoadText(text);
				if (reload)
				{
					Document.Replace(0, Document.TextLength, text);
					Document.DiffTracker.Reset();
				}
				else
				{
					Document.Text = text;
					Document.DiffTracker.SetBaseDocument(Document.CreateDocumentSnapshot());
				}
				inLoad = false;
				flag = true;
			}
			CreateDocumentParsedHandler();
			ContentName = fileName;
			lastSaveTimeUtc = File.GetLastWriteTimeUtc(ContentName);
			RunFirstTimeFoldUpdate(text);
			widget.TextEditor.Caret.Offset = 0;
			UpdateExecutionLocation();
			UpdateBreakpoints();
			UpdatePinnedWatches();
			LoadExtensions();
			IsDirty = !flag;
			UpdateTasks(null, null);
			widget.TextEditor.TextArea.SizeAllocated += HandleTextEditorVAdjustmentChanged;
			if (flag)
			{
				Document.InformLoadComplete();
				widget.EnsureCorrectEolMarker(fileName);
			}
		}

		private void HandleTextEditorVAdjustmentChanged(object sender, EventArgs e)
		{
			widget.TextEditor.TextArea.SizeAllocated -= HandleTextEditorVAdjustmentChanged;
			LoadSettings();
		}

		internal void LoadSettings()
		{
			if (widget != null && !string.IsNullOrEmpty(ContentName) && FileSettingsStore.TryGetValue(ContentName, out var settings))
			{
				widget.TextEditor.Caret.Offset = settings.CaretOffset;
				widget.TextEditor.VAdjustment.Value = settings.vAdjustment;
				widget.TextEditor.HAdjustment.Value = settings.hAdjustment;
			}
		}

		internal void StoreSettings()
		{
			if (!string.IsNullOrEmpty(ContentName))
			{
				FileSettingsStore.Store(ContentName, new FileSettingsStore.Settings
				{
					CaretOffset = widget.TextEditor.Caret.Offset,
					vAdjustment = widget.TextEditor.VAdjustment.Value,
					hAdjustment = widget.TextEditor.HAdjustment.Value
				});
			}
		}

		internal void ReplaceContent(string fileName, string content, Encoding encoding)
		{
			if (warnOverwrite)
			{
				warnOverwrite = false;
				widget.RemoveMessageBar();
				WorkbenchWindow.ShowNotification = false;
			}
			UpdateMimeType(fileName);
			inLoad = true;
			Document.Replace(0, Document.TextLength, content);
			Document.DiffTracker.Reset();
			inLoad = false;
			this.encoding = encoding;
			ContentName = fileName;
			RunFirstTimeFoldUpdate(content);
			CreateDocumentParsedHandler();
			UpdateExecutionLocation();
			UpdateBreakpoints();
			UpdatePinnedWatches();
			LoadExtensions();
			IsDirty = false;
			Document.InformLoadComplete();
		}

		private void UpdateMimeType(string fileName)
		{
			string mimeTypeForUri = DesktopService.GetMimeTypeForUri(fileName);
			if (!(loadedMimeType != mimeTypeForUri))
			{
				return;
			}
			loadedMimeType = mimeTypeForUri;
			if (mimeTypeForUri != null)
			{
				foreach (string item in DesktopService.GetMimeTypeInheritanceChain(loadedMimeType))
				{
					if (Mono.TextEditor.Highlighting.SyntaxModeService.GetSyntaxMode(null, item) != null)
					{
						Document.MimeType = item;
						widget.TextEditor.TextEditorResolverProvider = TextEditorResolverService.GetProvider(item);
						break;
					}
				}
			}
			((StyledSourceEditorOptions)SourceEditorWidget.TextEditor.Options).UpdateStyleParent(Project, loadedMimeType);
		}

		public override void Dispose()
		{
			ClearExtensions();
			FileRegistry.Remove(this);
			RemoveAutoSaveTimer();
			StoreSettings();
			isDisposed = true;
			--Counters.LoadedEditors;
			IdeApp.Preferences.DefaultHideMessageBubblesChanged -= HandleIdeAppPreferencesDefaultHideMessageBubblesChanged;
			IdeApp.Preferences.ShowMessageBubblesChanged -= HandleIdeAppPreferencesShowMessageBubblesChanged;
			TaskService.TaskToggled -= HandleErrorListPadTaskToggled;
			DisposeErrorMarkers();
			ClipbardRingUpdated -= UpdateClipboardRing;
			widget.TextEditor.Document.SyntaxModeChanged -= HandleSyntaxModeChanged;
			widget.TextEditor.Document.TextReplaced -= HandleTextReplaced;
			widget.TextEditor.Document.LineChanged -= HandleLineChanged;
			widget.TextEditor.Document.BeginUndo -= HandleBeginUndo;
			widget.TextEditor.Document.EndUndo -= HandleEndUndo;
			widget.TextEditor.Document.Undone -= HandleUndone;
			widget.TextEditor.Document.Redone -= HandleUndone;
			widget.TextEditor.Caret.PositionChanged -= HandlePositionChanged;
			widget.TextEditor.IconMargin.ButtonPressed -= OnIconButtonPress;
			widget.TextEditor.Document.TextReplacing -= OnTextReplacing;
			widget.TextEditor.Document.TextReplaced -= OnTextReplaced;
			widget.TextEditor.Document.ReadOnlyCheckDelegate = null;
			widget.TextEditor.Options.Changed -= HandleWidgetTextEditorOptionsChanged;
			TextEditorService.FileExtensionAdded -= HandleFileExtensionAdded;
			TextEditorService.FileExtensionRemoved -= HandleFileExtensionRemoved;
			DebuggingService.ExecutionLocationChanged -= executionLocationChanged;
			DebuggingService.DebugSessionStarted -= OnDebugSessionStarted;
			DebuggingService.CurrentFrameChanged -= currentFrameChanged;
			DebuggingService.StoppedEvent -= currentFrameChanged;
			DebuggingService.ResumedEvent -= currentFrameChanged;
			breakpoints.BreakpointAdded -= breakpointAdded;
			breakpoints.BreakpointRemoved -= breakpointRemoved;
			breakpoints.BreakpointStatusChanged -= breakpointStatusChanged;
			breakpoints.BreakpointModified -= breakpointStatusChanged;
			DebuggingService.PinnedWatches.WatchAdded -= OnWatchAdded;
			DebuggingService.PinnedWatches.WatchRemoved -= OnWatchRemoved;
			DebuggingService.PinnedWatches.WatchChanged -= OnWatchChanged;
			TaskService.Errors.TasksAdded -= UpdateTasks;
			TaskService.Errors.TasksRemoved -= UpdateTasks;
			TaskService.Errors.TasksChanged -= UpdateTasks;
			TaskService.JumpedToTask -= HandleTaskServiceJumpedToTask;
			debugStackLineMarker = null;
			currentDebugLineMarker = null;
			executionLocationChanged = null;
			currentFrameChanged = null;
			breakpointAdded = null;
			breakpointRemoved = null;
			breakpointStatusChanged = null;
			if (ownerDocument != null)
			{
				ownerDocument.DocumentParsed -= HandleDocumentParsed;
				ownerDocument = null;
			}
			RemoveMarkerQueue();
		}

		public Ambience GetAmbience()
		{
			string fileName = (base.IsUntitled ? UntitledName : ContentName);
			return AmbienceService.GetAmbienceForFile(fileName);
		}

		private bool CheckReadOnly(int line)
		{
			if (!writeAccessChecked && !base.IsUntitled)
			{
				writeAccessChecked = true;
				writeAllowed = FileService.RequestFileEdit(ContentName, throwIfFails: false);
			}
			if (!base.IsUntitled)
			{
				return writeAllowed;
			}
			return true;
		}

		private void OnTextReplacing(object s, DocumentChangeEventArgs a)
		{
			oldReplaceText = a.RemovedText.Text;
		}

		private void OnTextReplaced(object s, DocumentChangeEventArgs a)
		{
			IsDirty = Document.IsDirty;
			DocumentLocation documentLocation = Document.OffsetToLocation(a.Offset);
			int num = 0;
			int num2 = 0;
			while (num != -1 && num < oldReplaceText.Length)
			{
				num = oldReplaceText.IndexOf('\n', num);
				if (num != -1)
				{
					num2--;
					num++;
				}
			}
			if (a.InsertedText != null)
			{
				num = 0;
				for (string text = a.InsertedText.Text; num < text.Length; num++)
				{
					if (text[num] == '\n')
					{
						num2++;
					}
				}
			}
			if (num2 != 0)
			{
				TextEditorService.NotifyLineCountChanged(this, documentLocation.Line, num2, documentLocation.Column);
			}
		}

		private void OnCurrentFrameChanged(object s, EventArgs args)
		{
			UpdateExecutionLocation();
			if (!DebuggingService.IsDebugging)
			{
				UpdatePinnedWatches();
			}
		}

		private void OnExecutionLocationChanged(object s, EventArgs args)
		{
			UpdateExecutionLocation();
		}

		private void UpdateExecutionLocation()
		{
			if (DebuggingService.IsPaused)
			{
				SourceLocation sourceLocation = CheckLocationIsInFile(DebuggingService.NextStatementLocation) ?? CheckFrameIsInFile(DebuggingService.CurrentFrame) ?? CheckFrameIsInFile(DebuggingService.GetCurrentVisibleFrame());
				if (sourceLocation != null)
				{
					if (lastDebugLine == sourceLocation.Line)
					{
						return;
					}
					RemoveDebugMarkers();
					lastDebugLine = sourceLocation.Line;
					DocumentLine line = widget.TextEditor.Document.GetLine(lastDebugLine);
					if (line != null)
					{
						if (DebuggingService.CurrentFrameIndex == 0)
						{
							currentLineSegment = line;
							widget.TextEditor.Document.AddMarker(line, currentDebugLineMarker);
						}
						else
						{
							debugStackSegment = line;
							widget.TextEditor.Document.AddMarker(line, debugStackLineMarker);
						}
						widget.TextEditor.QueueDraw();
					}
					return;
				}
			}
			if (currentLineSegment != null || debugStackSegment != null)
			{
				RemoveDebugMarkers();
				lastDebugLine = -1;
				widget.TextEditor.QueueDraw();
			}
		}

		private SourceLocation CheckLocationIsInFile(SourceLocation location)
		{
			if (!string.IsNullOrEmpty(ContentName) && location != null && !string.IsNullOrEmpty(location.FileName) && ((FilePath)location.FileName).FullPath == ((FilePath)ContentName).FullPath)
			{
				return location;
			}
			return null;
		}

		private SourceLocation CheckFrameIsInFile(StackFrame frame)
		{
			if (frame == null)
			{
				return null;
			}
			return CheckLocationIsInFile(frame.SourceLocation);
		}

		private void RemoveDebugMarkers()
		{
			if (currentLineSegment != null)
			{
				widget.TextEditor.Document.RemoveMarker(currentDebugLineMarker);
				currentLineSegment = null;
			}
			if (debugStackSegment != null)
			{
				widget.TextEditor.Document.RemoveMarker(debugStackLineMarker);
				debugStackSegment = null;
			}
		}

		private void UpdatePinnedWatches()
		{
			foreach (PinnedWatchInfo pinnedWatch in pinnedWatches)
			{
				widget.TextEditor.Remove(pinnedWatch.Widget);
				pinnedWatch.Widget.Destroy();
			}
			pinnedWatches.Clear();
			if (ContentName == null || !DebuggingService.IsDebugging)
			{
				return;
			}
			foreach (PinnedWatch item in DebuggingService.PinnedWatches.GetWatchesForFile(System.IO.Path.GetFullPath(ContentName)))
			{
				AddWatch(item);
			}
			widget.TextEditor.QueueDraw();
		}

		private void AddWatch(PinnedWatch w)
		{
			DocumentLine line = widget.TextEditor.Document.GetLine(w.Line);
			if (line == null)
			{
				return;
			}
			PinnedWatchInfo item = new PinnedWatchInfo
			{
				Line = line
			};
			if (w.OffsetX < 0)
			{
				w.OffsetY = (int)widget.TextEditor.LineToY(w.Line);
				TextViewMargin.LayoutWrapper layout = widget.TextEditor.TextViewMargin.GetLayout(line);
				layout.Layout.GetPixelSize(out var width, out var _);
				if (layout.IsUncached)
				{
					layout.Dispose();
				}
				w.OffsetX = (int)widget.TextEditor.TextViewMargin.XOffset + width + 4;
			}
			item.Widget = new PinnedWatchWidget(widget.TextEditor, w);
			item.Watch = w;
			pinnedWatches.Add(item);
			widget.TextEditor.TextArea.AddTopLevelWidget(item.Widget, w.OffsetX, w.OffsetY);
		}

		private void OnDebugSessionStarted(object sender, EventArgs e)
		{
			UpdatePinnedWatches();
			foreach (MessageBubbleTextMarker currentErrorMarker in currentErrorMarkers)
			{
				currentErrorMarker.IsVisible = false;
			}
			DebuggingService.DebuggerSession.TargetExited += HandleTargetExited;
		}

		private void HandleTargetExited(object sender, EventArgs e)
		{
			foreach (MessageBubbleTextMarker currentErrorMarker in currentErrorMarkers)
			{
				currentErrorMarker.IsVisible = true;
			}
		}

		private void OnWatchAdded(object s, PinnedWatchEventArgs args)
		{
			if (args.Watch.File == (FilePath)ContentName && DebuggingService.IsDebugging)
			{
				AddWatch(args.Watch);
			}
		}

		private void OnWatchRemoved(object s, PinnedWatchEventArgs args)
		{
			foreach (PinnedWatchInfo pinnedWatch in pinnedWatches)
			{
				if (pinnedWatch.Watch == args.Watch)
				{
					pinnedWatches.Remove(pinnedWatch);
					widget.TextEditor.Remove(pinnedWatch.Widget);
					pinnedWatch.Widget.Destroy();
					break;
				}
			}
		}

		private void OnWatchChanged(object s, PinnedWatchEventArgs args)
		{
			foreach (PinnedWatchInfo pinnedWatch in pinnedWatches)
			{
				if (pinnedWatch.Watch == args.Watch)
				{
					pinnedWatch.Widget.ObjectValue = pinnedWatch.Watch.Value;
					widget.TextEditor.TextArea.MoveTopLevelWidget(pinnedWatch.Widget, args.Watch.OffsetX, args.Watch.OffsetY);
					break;
				}
			}
		}

		private void UpdateBreakpoints(bool forceUpdate = false)
		{
			TextDocument document = widget.TextEditor.Document;
			if (document == null)
			{
				return;
			}
			FilePath name = Name;
			if (!forceUpdate)
			{
				int num = 0;
				int num2 = 0;
				bool flag = false;
				lock (breakpoints)
				{
					foreach (Breakpoint item in breakpoints.GetBreakpointsAtFile(name.FullPath))
					{
						num2++;
						if (num < breakpointSegments.Count)
						{
							int num3 = document.OffsetToLineNumber(breakpointSegments[num].Offset);
							if (num3 != item.Line)
							{
								flag = true;
								break;
							}
							num++;
						}
					}
				}
				if (num2 != breakpointSegments.Count)
				{
					flag = true;
				}
				if (!flag)
				{
					return;
				}
			}
			HashSet<int> hashSet = new HashSet<int>();
			foreach (DocumentLine breakpointSegment in breakpointSegments)
			{
				if (breakpointSegment != null)
				{
					hashSet.Add(document.OffsetToLineNumber(breakpointSegment.Offset));
					document.RemoveMarker(breakpointSegment, typeof(BreakpointTextMarker));
					document.RemoveMarker(breakpointSegment, typeof(DisabledBreakpointTextMarker));
					document.RemoveMarker(breakpointSegment, typeof(InvalidBreakpointTextMarker));
				}
			}
			breakpointSegments.Clear();
			lock (breakpoints)
			{
				foreach (Breakpoint item2 in breakpoints.GetBreakpointsAtFile(name.FullPath))
				{
					hashSet.Add(item2.Line);
					AddBreakpoint(item2);
				}
			}
			foreach (int item3 in hashSet)
			{
				document.RequestUpdate(new LineUpdate(item3));
			}
			document.CommitDocumentUpdate();
			lastDebugLine = -1;
			UpdateExecutionLocation();
		}

		private void AddBreakpoint(Breakpoint bp)
		{
			if (DebuggingService.PinnedWatches.IsWatcherBreakpoint(bp))
			{
				return;
			}
			ExtensibleTextEditor textEditor = widget.TextEditor;
			if (textEditor == null)
			{
				return;
			}
			TextDocument document = textEditor.Document;
			if (document == null || !(Name.FullPath == (FilePath)bp.FileName))
			{
				return;
			}
			if (bp.Line <= 0 || bp.Line > textEditor.Document.LineCount)
			{
				LoggingService.LogWarning(string.Concat("Invalid breakpoint :", bp, " in line ", bp.Line));
				return;
			}
			DocumentLine line = document.GetLine(bp.Line);
			BreakEventStatus status = bp.GetStatus(DebuggingService.DebuggerSession);
			bool tracepoint = (bp.HitAction & HitAction.Break) == 0;
			if (line != null)
			{
				if (!bp.Enabled)
				{
					document.AddMarker(line, new DisabledBreakpointTextMarker(textEditor, tracepoint));
				}
				else if (status == BreakEventStatus.Bound || status == BreakEventStatus.Disconnected)
				{
					document.AddMarker(line, new BreakpointTextMarker(textEditor, tracepoint));
				}
				else
				{
					document.AddMarker(line, new InvalidBreakpointTextMarker(textEditor, tracepoint));
				}
				textEditor.QueueDraw();
				breakpointSegments.Add(line);
			}
		}

		private void OnBreakpointAdded(object s, BreakpointEventArgs args)
		{
			if (ContentName == null || args.Breakpoint.FileName != System.IO.Path.GetFullPath(ContentName))
			{
				return;
			}
			GLib.Timeout.Add(10u, delegate
			{
				if (!isDisposed)
				{
					UpdateBreakpoints();
				}
				return false;
			});
		}

		private void OnBreakpointRemoved(object s, BreakpointEventArgs args)
		{
			if (ContentName == null || args.Breakpoint.FileName != System.IO.Path.GetFullPath(ContentName))
			{
				return;
			}
			GLib.Timeout.Add(10u, delegate
			{
				if (!isDisposed)
				{
					UpdateBreakpoints();
				}
				return false;
			});
		}

		private void OnBreakpointStatusChanged(object s, BreakpointEventArgs args)
		{
			if (ContentName == null || args.Breakpoint.FileName != System.IO.Path.GetFullPath(ContentName))
			{
				return;
			}
			GLib.Timeout.Add(10u, delegate
			{
				if (!isDisposed)
				{
					UpdateBreakpoints(forceUpdate: true);
				}
				return false;
			});
		}

		private void OnIconButtonPress(object s, MarginMouseEventArgs args)
		{
			if (args.LineNumber < 1)
			{
				return;
			}
			if (args.TriggersContextMenu())
			{
				if (TextEditor.Caret.Line != args.LineNumber)
				{
					TextEditor.Caret.Line = args.LineNumber;
					TextEditor.Caret.Column = 1;
				}
				IdeApp.CommandService.ShowContextMenu(TextEditor, args.RawEvent as EventButton, WorkbenchWindow.ExtensionContext ?? AddinManager.AddinEngine, "/MonoDevelop/SourceEditor2/IconContextMenu/Editor");
			}
			else if (args.Button == 1 && !string.IsNullOrEmpty(Document.FileName) && args.LineSegment != null)
			{
				int column = ((TextEditor.Caret.Line != args.LineNumber) ? 1 : TextEditor.Caret.Column);
				lock (breakpoints)
				{
					breakpoints.Toggle(Document.FileName, args.LineNumber, column);
				}
			}
		}

		ITextEditorExtension IExtensibleTextEditor.AttachExtension(ITextEditorExtension extension)
		{
			Extension = extension;
			widget.TextEditor.Extension = extension;
			return widget;
		}

		public void Undo()
		{
			if (!MiscActions.CancelPreEditMode(TextEditor.GetTextEditorData()))
			{
				MiscActions.Undo(TextEditor.GetTextEditorData());
			}
		}

		public void SetCaretTo(int line, int column)
		{
			Document.RunWhenLoaded(delegate
			{
				PrepareToSetCaret(line, column);
				widget.TextEditor.SetCaretTo(line, column, highlight: true);
			});
		}

		public void SetCaretTo(int line, int column, bool highlight)
		{
			Document.RunWhenLoaded(delegate
			{
				PrepareToSetCaret(line, column);
				widget.TextEditor.SetCaretTo(line, column, highlight);
			});
		}

		public void SetCaretTo(int line, int column, bool highlight, bool centerCaret)
		{
			Document.RunWhenLoaded(delegate
			{
				PrepareToSetCaret(line, column);
				widget.TextEditor.SetCaretTo(line, column, highlight, centerCaret);
			});
		}

		protected virtual void PrepareToSetCaret(int line, int column)
		{
		}

		public void Redo()
		{
			if (!MiscActions.CancelPreEditMode(TextEditor.GetTextEditorData()))
			{
				MiscActions.Redo(TextEditor.GetTextEditorData());
			}
		}

		public IDisposable OpenUndoGroup()
		{
			return Document.OpenUndoGroup();
		}

		protected virtual void OnCaretPositionSet(EventArgs args)
		{
			if (CaretPositionSet != null)
			{
				CaretPositionSet(this, args);
			}
		}

		public void RunWhenLoaded(System.Action action)
		{
			Document.RunWhenLoaded(action);
		}

		public void Select(int startPosition, int endPosition)
		{
			TextEditor.SelectionRange = new TextSegment(startPosition, endPosition - startPosition);
			TextEditor.ScrollToCaret();
		}

		public void ShowPosition(int position)
		{
		}

		public string GetText(int startPosition, int endPosition)
		{
			TextDocument document = widget.TextEditor.Document;
			if (startPosition < 0 || endPosition < 0 || startPosition > endPosition || startPosition >= document.TextLength)
			{
				return "";
			}
			int count = Math.Min(endPosition - startPosition, document.TextLength - startPosition);
			return document.GetTextAt(startPosition, count);
		}

		public char GetCharAt(int position)
		{
			return widget.TextEditor.Document.GetCharAt(position);
		}

		public int GetPositionFromLineColumn(int line, int column)
		{
			return widget.TextEditor.Document.LocationToOffset(new DocumentLocation(line, column));
		}

		public void GetLineColumnFromPosition(int position, out int line, out int column)
		{
			DocumentLocation documentLocation = widget.TextEditor.Document.OffsetToLocation(position);
			line = documentLocation.Line;
			column = documentLocation.Column;
		}

		public int InsertText(int position, string text)
		{
			return widget.TextEditor.Insert(position, text);
		}

		public void DeleteText(int position, int length)
		{
			widget.TextEditor.Remove(position, length);
		}

		private DocumentLine GetLine(int position)
		{
			DocumentLocation documentLocation = Document.OffsetToLocation(position);
			return Document.GetLine(documentLocation.Line);
		}

		public void SetBookmarked(int position, bool mark)
		{
			DocumentLine line = GetLine(position);
			if (line != null && line.IsBookmarked != mark)
			{
				int line2 = widget.TextEditor.Document.OffsetToLineNumber(line.Offset);
				line.IsBookmarked = mark;
				widget.TextEditor.Document.RequestUpdate(new LineUpdate(line2));
				widget.TextEditor.Document.CommitDocumentUpdate();
			}
		}

		public bool IsBookmarked(int position)
		{
			return GetLine(position)?.IsBookmarked ?? false;
		}

		public void PrevBookmark()
		{
			TextEditor.RunAction(BookmarkActions.GotoPrevious);
		}

		public void NextBookmark()
		{
			TextEditor.RunAction(BookmarkActions.GotoNext);
		}

		public void ClearBookmarks()
		{
			TextEditor.RunAction(BookmarkActions.ClearAll);
		}

		public void Cut()
		{
			TextEditor.RunAction(ClipboardActions.Cut);
		}

		public void Copy()
		{
			TextEditor.RunAction(ClipboardActions.Copy);
		}

		public void Paste()
		{
			TextEditor.RunAction(ClipboardActions.Paste);
		}

		public void Delete()
		{
			if (TextEditor.IsSomethingSelected)
			{
				TextEditor.DeleteSelectedText();
			}
			else
			{
				TextEditor.RunAction(DeleteActions.Delete);
			}
		}

		public void SelectAll()
		{
			TextEditor.RunAction(SelectionActions.SelectAll);
		}

		public char GetChar(int offset)
		{
			return Document.GetCharAt(offset);
		}

		public void Replace(int offset, int count, string text)
		{
			widget.TextEditor.GetTextEditorData().Replace(offset, count, text);
		}

		public CodeCompletionContext CreateCodeCompletionContext(int triggerOffset)
		{
			CodeCompletionContext codeCompletionContext = new CodeCompletionContext();
			if (widget == null)
			{
				return codeCompletionContext;
			}
			ExtensibleTextEditor textEditor = widget.TextEditor;
			if (textEditor == null)
			{
				return codeCompletionContext;
			}
			codeCompletionContext.TriggerOffset = triggerOffset;
			DocumentLocation location = textEditor.Caret.Location;
			codeCompletionContext.TriggerLine = location.Line;
			codeCompletionContext.TriggerLineOffset = location.Column - 1;
			Cairo.Point point = widget.TextEditor.LocationToPoint(location);
			textEditor.ParentWindow.GetOrigin(out var x, out var y);
			x += textEditor.Allocation.X + point.X;
			y += textEditor.Allocation.Y + point.Y + (int)textEditor.LineHeight;
			codeCompletionContext.TriggerXCoord = x;
			codeCompletionContext.TriggerYCoord = y;
			codeCompletionContext.TriggerTextHeight = (int)TextEditor.GetLineHeight(location.Line);
			return codeCompletionContext;
		}

		public Gdk.Point DocumentToScreenLocation(DocumentLocation location)
		{
			Cairo.Point point = widget.TextEditor.LocationToPoint(location);
			widget.Vbox.ParentWindow.GetOrigin(out var x, out var y);
			x += widget.TextEditor.Allocation.X + point.X;
			y += widget.TextEditor.Allocation.Y + point.Y + (int)TextEditor.LineHeight;
			return new Gdk.Point(x, y);
		}

		public CodeTemplateContext GetCodeTemplateContext()
		{
			return TextEditor.GetTemplateContext();
		}

		public string GetCompletionText(CodeCompletionContext ctx)
		{
			if (ctx == null)
			{
				return null;
			}
			int startOffset = Math.Min(ctx.TriggerOffset, TextEditor.Caret.Offset);
			int endOffset = Math.Max(ctx.TriggerOffset, TextEditor.Caret.Offset);
			return Document.GetTextBetween(startOffset, endOffset);
		}

		public void SetCompletionText(CodeCompletionContext ctx, string partial_word, string complete_word)
		{
			SetCompletionText(ctx, partial_word, complete_word, complete_word.Length);
		}

		public static void SetCompletionText(TextEditorData data, CodeCompletionContext ctx, string partial_word, string complete_word, int wordOffset)
		{
			if (data == null || data.Document == null)
			{
				return;
			}
			int num = ctx.TriggerOffset;
			int num2 = ((!string.IsNullOrEmpty(partial_word)) ? partial_word.Length : 0);
			if (complete_word.EndsWith(":", StringComparison.Ordinal) && data.GetCharAt(num + num2) == ':')
			{
				num2++;
			}
			bool flag = false;
			if (data.IsSomethingSelected)
			{
				flag = data.MainSelection.SelectionMode == Mono.TextEditor.SelectionMode.Block;
				if (flag)
				{
					data.Caret.PreserveSelection = true;
					num = data.Caret.Offset - num2;
				}
				else
				{
					if (data.SelectionRange.Offset < ctx.TriggerOffset)
					{
						num = ctx.TriggerOffset - data.SelectionRange.Length;
					}
					data.DeleteSelectedText();
				}
				num2 = 0;
			}
			int num3 = complete_word.IndexOf('|');
			if (num3 >= 0)
			{
				complete_word = complete_word.Remove(num3, 1);
			}
			num += data.EnsureCaretIsNotVirtual();
			if (flag)
			{
				using (data.OpenUndoGroup())
				{
					int minLine = data.MainSelection.MinLine;
					int maxLine = data.MainSelection.MaxLine;
					int num4 = num - data.Document.GetLineByOffset(num).Offset;
					for (int i = minLine; i <= maxLine; i++)
					{
						DocumentLine line = data.Document.GetLine(i);
						if (line != null)
						{
							int offset = line.Offset + num4;
							data.Replace(offset, num2, complete_word);
						}
					}
					int column = Math.Min(data.MainSelection.Anchor.Column, data.MainSelection.Lead.Column);
					data.MainSelection = data.MainSelection.WithRange(new DocumentLocation((data.Caret.Line == minLine) ? maxLine : minLine, column), data.Caret.Location);
					data.Document.CommitMultipleLineUpdate(data.MainSelection.MinLine, data.MainSelection.MaxLine);
					data.Caret.PreserveSelection = false;
				}
			}
			else
			{
				data.Replace(num, num2, complete_word);
			}
			data.Document.CommitLineUpdate(data.Caret.Line);
			if (num3 >= 0)
			{
				data.Caret.Offset = num + num3;
			}
		}

		public void SetCompletionText(CodeCompletionContext ctx, string partial_word, string complete_word, int wordOffset)
		{
			TextEditorData textEditorData = GetTextEditorData();
			if (textEditorData == null)
			{
				return;
			}
			using (textEditorData.OpenUndoGroup())
			{
				SetCompletionText(textEditorData, ctx, partial_word, complete_word, wordOffset);
				CodeFormatter formatter = CodeFormatterService.GetFormatter(textEditorData.MimeType);
				if (formatter != null && complete_word.IndexOfAny(new char[4] { ' ', '\t', '{', '}' }) > 0 && formatter.SupportsOnTheFlyFormatting)
				{
					formatter.OnTheFlyFormat(WorkbenchWindow.Document, ctx.TriggerOffset, ctx.TriggerOffset + complete_word.Length);
				}
			}
		}

		internal void FireCompletionContextChanged()
		{
			if (CompletionContextChanged != null)
			{
				CompletionContextChanged(this, EventArgs.Empty);
			}
		}

		[CommandHandler(DebugCommands.ExpressionEvaluator)]
		protected void ShowExpressionEvaluator()
		{
			string expression = "";
			if (TextEditor.IsSomethingSelected)
			{
				expression = TextEditor.SelectedText;
			}
			else
			{
				ResolveResult languageItem = TextEditor.GetLanguageItem(TextEditor.Caret.Offset, out var region);
				if (languageItem != null && !languageItem.IsError)
				{
					expression = TextEditor.GetTextBetween(region.Begin, region.End);
				}
			}
			DebuggingService.ShowExpressionEvaluator(expression);
		}

		[CommandUpdateHandler(DebugCommands.ExpressionEvaluator)]
		protected void UpdateShowExpressionEvaluator(CommandInfo cinfo)
		{
			if (DebuggingService.IsDebugging)
			{
				cinfo.Enabled = DebuggingService.CurrentFrame != null;
			}
			else
			{
				cinfo.Visible = false;
			}
		}

		public void SplitHorizontally()
		{
			widget.Split(vSplit: false);
		}

		public void SplitVertically()
		{
			widget.Split(vSplit: true);
		}

		public void Unsplit()
		{
			widget.Unsplit();
		}

		public void SwitchWindow()
		{
			widget.SwitchWindow();
		}

		public void ToggleAllFoldings()
		{
			FoldActions.ToggleAllFolds(TextEditor.GetTextEditorData());
			widget.TextEditor.ScrollToCaret();
		}

		public void FoldDefinitions()
		{
			bool isFolded = true;
			foreach (FoldSegment foldSegment in Document.FoldSegments)
			{
				if ((foldSegment.FoldingType == FoldingType.TypeMember || foldSegment.FoldingType == FoldingType.Comment) && foldSegment.IsFolded)
				{
					isFolded = false;
				}
			}
			foreach (FoldSegment foldSegment2 in Document.FoldSegments)
			{
				if (foldSegment2.FoldingType == FoldingType.TypeDefinition)
				{
					foldSegment2.IsFolded = false;
				}
				if (foldSegment2.FoldingType == FoldingType.TypeMember || foldSegment2.FoldingType == FoldingType.Comment)
				{
					foldSegment2.IsFolded = isFolded;
				}
			}
			widget.TextEditor.Caret.MoveCaretBeforeFoldings();
			Document.RequestUpdate(new UpdateAll());
			Document.CommitDocumentUpdate();
			widget.TextEditor.GetTextEditorData().RaiseUpdateAdjustmentsRequested();
			widget.TextEditor.ScrollToCaret();
		}

		public void ToggleFolding()
		{
			FoldActions.ToggleFold(TextEditor.GetTextEditorData());
			widget.TextEditor.ScrollToCaret();
		}

		public void PrintDocument(PrintingSettings settings)
		{
			RunPrintOperation(PrintOperationAction.PrintDialog, settings);
		}

		public void PrintPreviewDocument(PrintingSettings settings)
		{
			RunPrintOperation(PrintOperationAction.Preview, settings);
		}

		private void RunPrintOperation(PrintOperationAction action, PrintingSettings settings)
		{
			SourceEditorPrintOperation sourceEditorPrintOperation = new SourceEditorPrintOperation(TextEditor.Document, Name);
			if (settings.PrintSettings != null)
			{
				sourceEditorPrintOperation.PrintSettings = settings.PrintSettings;
			}
			if (settings.PageSetup != null)
			{
				sourceEditorPrintOperation.DefaultPageSetup = settings.PageSetup;
			}
			switch (sourceEditorPrintOperation.Run(action, IdeApp.Workbench.RootWindow))
			{
			case PrintOperationResult.Apply:
				settings.PrintSettings = sourceEditorPrintOperation.PrintSettings;
				break;
			case PrintOperationResult.Error:
				MessageService.ShowError(GettextCatalog.GetString("Print operation failed."));
				break;
			}
		}

		static SourceEditorView()
		{
			clipboardRing = new List<TextToolboxNode>();
			CodeSegmentPreviewWindow.CodeSegmentPreviewInformString = GettextCatalog.GetString("Press 'F2' for focus");
			ClipboardActions.CopyOperation.Copy += delegate(string text)
			{
				if (!string.IsNullOrEmpty(text))
				{
					foreach (TextToolboxNode item in clipboardRing)
					{
						if (item.Text == text)
						{
							clipboardRing.Remove(item);
							break;
						}
					}
					TextToolboxNode textToolboxNode = new TextToolboxNode(text);
					string[] array = text.Split('\n');
					for (int i = 0; i < 3 && i < array.Length; i++)
					{
						if (i > 0)
						{
							textToolboxNode.Description += Environment.NewLine;
						}
						string text2 = array[i];
						if (text2.Length > 16)
						{
							text2 = text2.Substring(0, 16) + "...";
						}
						textToolboxNode.Description += text2;
					}
					textToolboxNode.Category = GettextCatalog.GetString("Clipboard ring");
					textToolboxNode.Icon = DesktopService.GetIconForFile("a.txt", IconSize.Menu);
					textToolboxNode.Name = ((text.Length > 16) ? (text.Substring(0, 16) + "...") : text);
					textToolboxNode.Name = textToolboxNode.Name.Replace("\t", "\\t");
					textToolboxNode.Name = textToolboxNode.Name.Replace("\n", "\\n");
					clipboardRing.Add(textToolboxNode);
					while (clipboardRing.Count > 12)
					{
						clipboardRing.RemoveAt(0);
					}
					if (ClipbardRingUpdated != null)
					{
						ClipbardRingUpdated(null, EventArgs.Empty);
					}
				}
			};
		}

		public void UpdateClipboardRing(object sender, EventArgs e)
		{
			if (ItemsChanged != null)
			{
				ItemsChanged(this, EventArgs.Empty);
			}
		}

		public IEnumerable<ItemToolboxNode> GetDynamicItems(IToolboxConsumer consumer)
		{
			foreach (TextToolboxNode item in clipboardRing)
			{
				yield return item;
			}
		}

		void IToolboxConsumer.ConsumeItem(ItemToolboxNode item)
		{
			if (item is ITextToolboxNode textToolboxNode)
			{
				textToolboxNode.InsertAtCaret(base.WorkbenchWindow.Document);
				TextEditor.GrabFocus();
			}
		}

		void IToolboxConsumer.DragItem(ItemToolboxNode item, Widget source, DragContext ctx)
		{
			string dragPreviewText = GetDragPreviewText(item);
			if (!string.IsNullOrEmpty(dragPreviewText))
			{
				dragItem = item;
				customSource = source;
				customSource.DragDataGet += HandleDragDataGet;
				customSource.DragEnd += HandleDragEnd;
			}
		}

		private void HandleDragEnd(object o, DragEndArgs args)
		{
			if (customSource != null)
			{
				customSource.DragDataGet -= HandleDragDataGet;
				customSource.DragEnd -= HandleDragEnd;
				customSource = null;
			}
		}

		private void HandleDragDataGet(object o, DragDataGetArgs args)
		{
			if (dragItem != null)
			{
				TextEditor.CaretToDragCaretPosition();
				((IToolboxConsumer)this).ConsumeItem(dragItem);
				dragItem = null;
			}
		}

		private string GetDragPreviewText(ItemToolboxNode item)
		{
			if (!(item is ITextToolboxNode textToolboxNode))
			{
				LoggingService.LogWarning("Cannot use non-ITextToolboxNode toolbox items in the text editor.");
				return null;
			}
			return textToolboxNode.GetDragPreview(base.WorkbenchWindow.Document);
		}

		bool ICustomFilteringToolboxConsumer.SupportsItem(ItemToolboxNode item)
		{
			if (!(item is ITextToolboxNode textToolboxNode))
			{
				return false;
			}
			return textToolboxNode.IsCompatibleWith(base.WorkbenchWindow.Document);
		}

		bool IToolboxConsumer.CustomFilterSupports(ItemToolboxNode item)
		{
			return false;
		}

		void IZoomable.ZoomIn()
		{
			TextEditor.Options.ZoomIn();
		}

		void IZoomable.ZoomOut()
		{
			TextEditor.Options.ZoomOut();
		}

		void IZoomable.ZoomReset()
		{
			TextEditor.Options.ZoomReset();
		}

		public ResolveResult GetLanguageItem(int offset)
		{
			DomRegion region;
			return SourceEditorWidget.TextEditor.GetLanguageItem(offset, out region);
		}

		public ResolveResult GetLanguageItem(int offset, string expression)
		{
			return SourceEditorWidget.TextEditor.GetLanguageItem(offset, expression);
		}

		void ISupportsProjectReload.Update(Project project)
		{
		}

		public TextEditorData GetTextEditorData()
		{
			return TextEditor?.GetTextEditorData();
		}

		public void InsertTemplate(CodeTemplate template, Document doc)
		{
			TextEditor.InsertTemplate(template, doc);
		}

		[CommandHandler(TextEditorCommands.GotoMatchingBrace)]
		protected void OnGotoMatchingBrace()
		{
			TextEditor.RunAction(MiscActions.GotoMatchingBracket);
		}

		private void CorrectIndenting()
		{
			CodeFormatter formatter = CodeFormatterService.GetFormatter(Document.MimeType);
			if (formatter == null || !formatter.SupportsCorrectingIndent)
			{
				return;
			}
			PolicyBag policyParent = ((Project != null) ? Project.Policies : null);
			TextEditorData textEditorData = TextEditor.GetTextEditorData();
			if (TextEditor.IsSomethingSelected)
			{
				using (TextEditor.OpenUndoGroup())
				{
					Mono.TextEditor.Selection mainSelection = TextEditor.MainSelection;
					int anchorOffset = mainSelection.GetAnchorOffset(textEditorData);
					int leadOffset = mainSelection.GetLeadOffset(textEditorData);
					ITextSourceVersion version = TextEditor.Document.Version;
					int maxLine = mainSelection.MaxLine;
					for (int i = TextEditor.MainSelection.MinLine; i <= maxLine; i++)
					{
						formatter.CorrectIndenting(policyParent, textEditorData, i);
					}
					textEditorData.SetSelection(version.MoveOffsetTo(textEditorData.Document.Version, anchorOffset), version.MoveOffsetTo(textEditorData.Document.Version, leadOffset));
					return;
				}
			}
			formatter.CorrectIndenting(policyParent, textEditorData, TextEditor.Caret.Line);
		}

		[CommandUpdateHandler(TextEditorCommands.MoveBlockDown)]
		[CommandUpdateHandler(TextEditorCommands.MoveBlockUp)]
		private void MoveBlockUpdateHandler(CommandInfo cinfo)
		{
			cinfo.Enabled = widget.EditorHasFocus;
		}

		[CommandHandler(TextEditorCommands.MoveBlockUp)]
		protected void OnMoveBlockUp()
		{
			using (TextEditor.OpenUndoGroup())
			{
				TextEditor.RunAction(MiscActions.MoveBlockUp);
				CorrectIndenting();
			}
		}

		[CommandHandler(TextEditorCommands.MoveBlockDown)]
		protected void OnMoveBlockDown()
		{
			using (TextEditor.OpenUndoGroup())
			{
				TextEditor.RunAction(MiscActions.MoveBlockDown);
				CorrectIndenting();
			}
		}

		[CommandUpdateHandler(TextEditorCommands.ToggleBlockSelectionMode)]
		protected void UpdateToggleBlockSelectionMode(CommandInfo cinfo)
		{
			cinfo.Enabled = TextEditor.IsSomethingSelected;
		}

		[CommandHandler(TextEditorCommands.ToggleBlockSelectionMode)]
		protected void OnToggleBlockSelectionMode()
		{
			TextEditor.SelectionMode = ((TextEditor.SelectionMode == Mono.TextEditor.SelectionMode.Normal) ? Mono.TextEditor.SelectionMode.Block : Mono.TextEditor.SelectionMode.Normal);
			TextEditor.QueueDraw();
		}

		[CommandHandler(SearchCommands.EmacsFindNext)]
		public void EmacsFindNext()
		{
			widget.EmacsFindNext();
		}

		[CommandHandler(SearchCommands.EmacsFindPrevious)]
		public void EmacsFindPrevious()
		{
			widget.EmacsFindPrevious();
		}

		[CommandHandler(SearchCommands.Find)]
		public void ShowSearchWidget()
		{
			widget.ShowSearchWidget();
		}

		[CommandHandler(SearchCommands.Replace)]
		public void ShowReplaceWidget()
		{
			widget.ShowReplaceWidget();
		}

		[CommandUpdateHandler(SearchCommands.UseSelectionForFind)]
		protected void OnUpdateUseSelectionForFind(CommandInfo info)
		{
			widget.OnUpdateUseSelectionForFind(info);
		}

		[CommandHandler(SearchCommands.UseSelectionForFind)]
		public void UseSelectionForFind()
		{
			widget.UseSelectionForFind();
		}

		[CommandUpdateHandler(SearchCommands.UseSelectionForReplace)]
		protected void OnUpdateUseSelectionForReplace(CommandInfo info)
		{
			widget.OnUpdateUseSelectionForReplace(info);
		}

		[CommandHandler(SearchCommands.UseSelectionForReplace)]
		public void UseSelectionForReplace()
		{
			widget.UseSelectionForReplace();
		}

		[CommandHandler(SearchCommands.GotoLineNumber)]
		public void ShowGotoLineNumberWidget()
		{
			widget.ShowGotoLineNumberWidget();
		}

		[CommandHandler(SearchCommands.FindNext)]
		public SearchResult FindNext()
		{
			return widget.FindNext();
		}

		[CommandUpdateHandler(SearchCommands.FindNext)]
		[CommandUpdateHandler(SearchCommands.FindPrevious)]
		private void UpdateFindNextAndPrev(CommandInfo cinfo)
		{
			cinfo.Enabled = !string.IsNullOrEmpty(SearchAndReplaceOptions.SearchPattern);
		}

		[CommandHandler(SearchCommands.FindPrevious)]
		public SearchResult FindPrevious()
		{
			return widget.FindPrevious();
		}

		[CommandHandler(SearchCommands.FindNextSelection)]
		public SearchResult FindNextSelection()
		{
			return widget.FindNextSelection();
		}

		[CommandHandler(SearchCommands.FindPreviousSelection)]
		public SearchResult FindPreviousSelection()
		{
			return widget.FindPreviousSelection();
		}

		[CommandHandler(HelpCommands.Help)]
		internal void MonodocResolver()
		{
			widget.MonodocResolver();
		}

		[CommandUpdateHandler(HelpCommands.Help)]
		internal void MonodocResolverUpdate(CommandInfo cinfo)
		{
			widget.MonodocResolverUpdate(cinfo);
		}

		[CommandUpdateHandler(EditCommands.ToggleCodeComment)]
		internal void OnUpdateToggleComment(CommandInfo info)
		{
			widget.OnUpdateToggleComment(info);
		}

		[CommandHandler(EditCommands.ToggleCodeComment)]
		public void ToggleCodeComment()
		{
			widget.ToggleCodeComment();
		}

		[CommandUpdateHandler(EditCommands.AddCodeComment)]
		internal void OnUpdateAddCodeComment(CommandInfo info)
		{
			widget.OnUpdateToggleComment(info);
		}

		[CommandHandler(EditCommands.AddCodeComment)]
		public void AddCodeComment()
		{
			widget.AddCodeComment();
		}

		[CommandUpdateHandler(EditCommands.RemoveCodeComment)]
		internal void OnUpdateRemoveCodeComment(CommandInfo info)
		{
			widget.OnUpdateToggleComment(info);
		}

		[CommandHandler(EditCommands.RemoveCodeComment)]
		public void RemoveCodeComment()
		{
			widget.RemoveCodeComment();
		}

		[CommandUpdateHandler(SourceEditorCommands.ToggleErrorTextMarker)]
		public void OnUpdateToggleErrorTextMarker(CommandInfo info)
		{
			widget.OnUpdateToggleErrorTextMarker(info);
		}

		[CommandHandler(SourceEditorCommands.ToggleErrorTextMarker)]
		public void OnToggleErrorTextMarker()
		{
			widget.OnToggleErrorTextMarker();
		}

		[CommandHandler(EditCommands.IndentSelection)]
		public void IndentSelection()
		{
			if (widget.TextEditor.IsSomethingSelected)
			{
				MiscActions.IndentSelection(widget.TextEditor.GetTextEditorData());
				return;
			}
			int offset = widget.TextEditor.LocationToOffset(widget.TextEditor.Caret.Line, 1);
			widget.TextEditor.Insert(offset, widget.TextEditor.Options.IndentationString);
		}

		[CommandHandler(EditCommands.UnIndentSelection)]
		public void UnIndentSelection()
		{
			MiscActions.RemoveTab(widget.TextEditor.GetTextEditorData());
		}

		[CommandHandler(EditCommands.InsertGuid)]
		public void InsertGuid()
		{
			TextEditor.InsertAtCaret(Guid.NewGuid().ToString());
		}

		[CommandHandler(SourceEditorCommands.NextIssue)]
		private void NextIssue()
		{
			widget.NextIssue();
		}

		[CommandHandler(SourceEditorCommands.PrevIssue)]
		private void PrevIssue()
		{
			widget.PrevIssue();
		}

		[CommandHandler(SourceEditorCommands.NextIssueError)]
		private void NextIssueError()
		{
			widget.NextIssueError();
		}

		[CommandHandler(SourceEditorCommands.PrevIssueError)]
		private void PrevIssueError()
		{
			widget.PrevIssueError();
		}

		[CommandHandler(ScrollbarCommand.Top)]
		private void GotoTop()
		{
			widget.QuickTaskStrip.GotoTop();
		}

		[CommandHandler(ScrollbarCommand.Bottom)]
		private void GotoBottom()
		{
			widget.QuickTaskStrip.GotoBottom();
		}

		[CommandHandler(ScrollbarCommand.PgUp)]
		private void GotoPgUp()
		{
			widget.QuickTaskStrip.GotoPgUp();
		}

		[CommandHandler(ScrollbarCommand.PgDown)]
		private void GotoPgDown()
		{
			widget.QuickTaskStrip.GotoPgDown();
		}

		[CommandUpdateHandler(ScrollbarCommand.ShowTasks)]
		private void UpdateShowMap(CommandInfo info)
		{
			widget.QuickTaskStrip.UpdateShowMap(info);
		}

		[CommandHandler(ScrollbarCommand.ShowTasks)]
		private void ShowMap()
		{
			widget.QuickTaskStrip.ShowMap();
		}

		[CommandUpdateHandler(ScrollbarCommand.ShowMinimap)]
		private void UpdateShowFull(CommandInfo info)
		{
			widget.QuickTaskStrip.UpdateShowFull(info);
		}

		[CommandHandler(ScrollbarCommand.ShowMinimap)]
		private void ShowFull()
		{
			widget.QuickTaskStrip.ShowFull();
		}
	}
}
