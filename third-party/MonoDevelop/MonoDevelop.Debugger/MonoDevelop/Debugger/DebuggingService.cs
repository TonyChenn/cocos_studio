using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Gdk;
using Gtk;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.Addins;
using Mono.Debugging.Client;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Debugger.Viewers;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Ide.TextEditing;
using MonoDevelop.Projects;
using Xwt;

namespace MonoDevelop.Debugger
{
	public static class DebuggingService
	{
		private const string FactoriesPath = "/MonoDevelop/Debugging/DebuggerEngines";

		private const string EvaluatorsPath = "/MonoDevelop/Debugging/Evaluators";

		private static DebuggerEngine[] engines;

		private static Dictionary<string, ExpressionEvaluatorExtensionNode> evaluators;

		private static readonly PinnedWatchStore pinnedWatches;

		private static readonly BreakpointStore breakpoints;

		private static readonly DebugExecutionHandlerFactory executionHandlerFactory;

		private static IConsole console;

		private static Dictionary<long, SourceLocation> nextStatementLocations;

		private static DebuggerEngine currentEngine;

		private static DebuggerSession session;

		private static Backtrace currentBacktrace;

		private static int currentFrame;

		private static ExceptionCaughtMessage exceptionDialog;

		private static BusyEvaluatorDialog busyDialog;

		private static StatusBarIcon busyStatusIcon;

		private static bool isBusy;

		private static readonly object cleanup_lock;

		public static DebuggerSession DebuggerSession => session;

		public static BreakpointStore Breakpoints => breakpoints;

		public static PinnedWatchStore PinnedWatches => pinnedWatches;

		[Obsolete]
		public static string[] EnginePriority
		{
			get
			{
				return new string[0];
			}
			set
			{
			}
		}

		public static bool IsDebuggingSupported => AddinManager.GetExtensionNodes("/MonoDevelop/Debugging/DebuggerEngines").Count > 0;

		internal static ExceptionCaughtMessage ExceptionCaughtMessage => exceptionDialog;

		public static bool IsDebugging => session != null;

		public static bool IsConnected
		{
			get
			{
				if (IsDebugging)
				{
					return session.IsConnected;
				}
				return false;
			}
		}

		public static bool IsRunning
		{
			get
			{
				if (IsDebugging)
				{
					return session.IsRunning;
				}
				return false;
			}
		}

		public static bool IsPaused
		{
			get
			{
				if (IsDebugging && !IsRunning)
				{
					return currentBacktrace != null;
				}
				return false;
			}
		}

		public static Backtrace CurrentCallStack => currentBacktrace;

		public static SourceLocation NextStatementLocation
		{
			get
			{
				SourceLocation value = null;
				if (IsPaused)
				{
					nextStatementLocations.TryGetValue(session.ActiveThread.Id, out value);
				}
				return value;
			}
		}

		public static StackFrame CurrentFrame
		{
			get
			{
				if (currentBacktrace != null && currentFrame != -1)
				{
					return currentBacktrace.GetFrame(currentFrame);
				}
				return null;
			}
		}

		public static int CurrentFrameIndex
		{
			get
			{
				return currentFrame;
			}
			set
			{
				if (currentBacktrace != null && value < currentBacktrace.FrameCount)
				{
					currentFrame = value;
					DispatchService.GuiDispatch(delegate
					{
						NotifyCurrentFrameChanged();
					});
				}
				else
				{
					currentFrame = -1;
				}
			}
		}

		public static ThreadInfo ActiveThread
		{
			get
			{
				return session.ActiveThread;
			}
			set
			{
				session.ActiveThread = value;
				SetCurrentBacktrace(session.ActiveThread.Backtrace);
			}
		}

		public static event EventHandler DebugSessionStarted;

		public static event EventHandler PausedEvent;

		public static event EventHandler ResumedEvent;

		public static event EventHandler StoppedEvent;

		public static event EventHandler CallStackChanged;

		public static event EventHandler CurrentFrameChanged;

		public static event EventHandler ExecutionLocationChanged;

		public static event EventHandler DisassemblyRequested;

		public static event EventHandler<DocumentEventArgs> DisableConditionalCompilation;

		public static event EventHandler EvaluationOptionsChanged;

		static DebuggingService()
		{
			pinnedWatches = new PinnedWatchStore();
			breakpoints = new BreakpointStore();
			nextStatementLocations = new Dictionary<long, SourceLocation>();
			cleanup_lock = new object();
			executionHandlerFactory = new DebugExecutionHandlerFactory();
			TextEditorService.LineCountChanged += OnLineCountChanged;
			IdeApp.Initialized += delegate
			{
				IdeApp.Workspace.StoringUserPreferences += OnStoreUserPrefs;
				IdeApp.Workspace.LoadingUserPreferences += OnLoadUserPrefs;
				IdeApp.Workspace.LastWorkspaceItemClosed += OnSolutionClosed;
				busyDialog = new BusyEvaluatorDialog();
				busyDialog.TransientFor = MessageService.RootWindow;
				busyDialog.DestroyWithParent = true;
			};
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/Debugging/DebuggerEngines", delegate
			{
				engines = null;
			});
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/Debugging/Evaluators", delegate
			{
				evaluators = null;
			});
		}

		public static IExecutionHandler GetExecutionHandler()
		{
			return executionHandlerFactory;
		}

		public static void SetLiveUpdateMode(PinnedWatch watch, bool liveUpdate)
		{
			if (watch.LiveUpdate == liveUpdate)
			{
				return;
			}
			watch.LiveUpdate = liveUpdate;
			if (liveUpdate)
			{
				Breakpoint breakpoint = new Breakpoint(watch.File, watch.Line);
				breakpoint.TraceExpression = "{" + watch.Expression + "}";
				breakpoint.HitAction |= HitAction.PrintExpression;
				lock (breakpoints)
				{
					breakpoints.Add(breakpoint);
				}
				pinnedWatches.Bind(watch, breakpoint);
				return;
			}
			pinnedWatches.Bind(watch, null);
			lock (breakpoints)
			{
				breakpoints.Remove(watch.BoundTracer);
			}
		}

		private static void BreakpointTraceHandler(BreakEvent be, string trace)
		{
			if (!(be is Breakpoint) || !pinnedWatches.UpdateLiveWatch((Breakpoint)be, trace))
			{
				DebugWriter(0, "", trace + Environment.NewLine);
			}
		}

		internal static IEnumerable<ValueVisualizer> GetValueVisualizers(ObjectValue val)
		{
			try
			{
				object[] extensionObjects = AddinManager.GetExtensionObjects("/MonoDevelop/Debugging/ValueVisualizers", reuseCachedInstance: false);
				foreach (object v in extensionObjects)
				{
					if (v is ValueVisualizer)
					{
						ValueVisualizer vv = (ValueVisualizer)v;
						if (vv.CanVisualize(val))
						{
							yield return vv;
						}
					}
				}
			}
			finally
			{
			}
		}

		internal static bool HasValueVisualizers(ObjectValue val)
		{
			return GetValueVisualizers(val).Any();
		}

		internal static InlineVisualizer GetInlineVisualizer(ObjectValue val)
		{
			object[] extensionObjects = AddinManager.GetExtensionObjects("/MonoDevelop/Debugging/InlineVisualizers", reuseCachedInstance: true);
			foreach (object obj in extensionObjects)
			{
				if (obj is InlineVisualizer inlineVisualizer && inlineVisualizer.CanInlineVisualize(val))
				{
					return inlineVisualizer;
				}
			}
			return null;
		}

		internal static bool HasInlineVisualizer(ObjectValue val)
		{
			return GetInlineVisualizer(val) != null;
		}

		internal static PreviewVisualizer GetPreviewVisualizer(ObjectValue val)
		{
			object[] extensionObjects = AddinManager.GetExtensionObjects("/MonoDevelop/Debugging/PreviewVisualizers", reuseCachedInstance: true);
			foreach (object obj in extensionObjects)
			{
				if (obj is PreviewVisualizer previewVisualizer && previewVisualizer.CanVisualize(val))
				{
					return previewVisualizer;
				}
			}
			return null;
		}

		internal static bool HasPreviewVisualizer(ObjectValue val)
		{
			return GetPreviewVisualizer(val) != null;
		}

		public static DebugValueConverter<T> GetGetConverter<T>(ObjectValue val)
		{
			object[] extensionObjects = AddinManager.GetExtensionObjects("/MonoDevelop/Debugging/DebugValueConverters", reuseCachedInstance: true);
			foreach (object obj in extensionObjects)
			{
				if (obj is DebugValueConverter<T> debugValueConverter && debugValueConverter.CanGetValue(val))
				{
					return debugValueConverter;
				}
			}
			return null;
		}

		public static bool HasGetConverter<T>(ObjectValue val)
		{
			return GetGetConverter<T>(val) != null;
		}

		public static DebugValueConverter<T> GetSetConverter<T>(ObjectValue val)
		{
			object[] extensionObjects = AddinManager.GetExtensionObjects("/MonoDevelop/Debugging/DebugValueConverters", reuseCachedInstance: true);
			foreach (object obj in extensionObjects)
			{
				if (obj is DebugValueConverter<T> debugValueConverter && debugValueConverter.CanSetValue(val))
				{
					return debugValueConverter;
				}
			}
			return null;
		}

		public static bool HasSetConverter<T>(ObjectValue val)
		{
			return GetSetConverter<T>(val) != null;
		}

		public static void ShowValueVisualizer(ObjectValue val)
		{
			ValueVisualizerDialog valueVisualizerDialog = new ValueVisualizerDialog();
			valueVisualizerDialog.Show(val);
			MessageService.ShowCustomDialog(valueVisualizerDialog);
		}

		public static void ShowPreviewVisualizer(ObjectValue val, Control widget, Gdk.Rectangle previewButtonArea)
		{
			PreviewWindowManager.Show(val, widget, previewButtonArea);
		}

		public static bool ShowBreakpointProperties(ref BreakEvent bp, BreakpointType breakpointType = BreakpointType.Location)
		{
			using (BreakpointPropertiesDialog breakpointPropertiesDialog = new BreakpointPropertiesDialog(bp, breakpointType))
			{
				Command command = breakpointPropertiesDialog.Run();
				if (bp == null)
				{
					bp = breakpointPropertiesDialog.GetBreakEvent();
				}
				return command == Command.Ok;
			}
		}

		public static void AddWatch(string expression)
		{
			Pad pad = IdeApp.Workbench.GetPad<WatchPad>();
			WatchPad watchPad = (WatchPad)pad.Content;
			pad.BringToFront(grabFocus: false);
			watchPad.AddWatch(expression);
		}

		public static bool IsFeatureSupported(IBuildTarget target, DebuggerFeatures feature)
		{
			return (GetSupportedFeatures(target) & feature) == feature;
		}

		public static bool CurrentSessionSupportsFeature(DebuggerFeatures feature)
		{
			return (currentEngine.SupportedFeatures & feature) == feature;
		}

		public static bool IsFeatureSupported(DebuggerFeatures feature)
		{
			DebuggerEngine[] debuggerEngines = GetDebuggerEngines();
			foreach (DebuggerEngine debuggerEngine in debuggerEngines)
			{
				if ((debuggerEngine.SupportedFeatures & feature) == feature)
				{
					return true;
				}
			}
			return false;
		}

		public static DebuggerFeatures GetSupportedFeatures(IBuildTarget target)
		{
			FeatureCheckerHandlerFactory featureCheckerHandlerFactory = new FeatureCheckerHandlerFactory();
			ExecutionContext context = new ExecutionContext(featureCheckerHandlerFactory, null, IdeApp.Workspace.ActiveExecutionTarget);
			target.CanExecute(context, IdeApp.Workspace.ActiveConfiguration);
			return featureCheckerHandlerFactory.SupportedFeatures;
		}

		public static DebuggerFeatures GetSupportedFeaturesForCommand(ExecutionCommand command)
		{
			return GetFactoryForCommand(command)?.SupportedFeatures ?? DebuggerFeatures.None;
		}

		public static void ShowExpressionEvaluator(string expression)
		{
			ExpressionEvaluatorDialog expressionEvaluatorDialog = new ExpressionEvaluatorDialog();
			if (expression != null)
			{
				expressionEvaluatorDialog.Expression = expression;
			}
			MessageService.ShowCustomDialog(expressionEvaluatorDialog);
		}

		public static void ShowExceptionCaughtDialog()
		{
			EvaluationOptions evaluationOptions = session.EvaluationOptions.Clone();
			evaluationOptions.MemberEvaluationTimeout = 0;
			evaluationOptions.EvaluationTimeout = 0;
			evaluationOptions.EllipsizeStrings = false;
			ExceptionInfo exception = CurrentFrame.GetException(evaluationOptions);
			if (exception != null)
			{
				HideExceptionCaughtDialog();
				exceptionDialog = new ExceptionCaughtMessage(exception, CurrentFrame.SourceLocation.FileName, CurrentFrame.SourceLocation.Line, CurrentFrame.SourceLocation.Column);
				if (CurrentFrame.SourceLocation.FileName != null)
				{
					exceptionDialog.ShowButton();
				}
				else
				{
					exceptionDialog.ShowDialog();
				}
				exceptionDialog.Closed += delegate
				{
					exceptionDialog = null;
				};
			}
		}

		private static void HideExceptionCaughtDialog()
		{
			if (exceptionDialog != null)
			{
				exceptionDialog.Dispose();
				exceptionDialog = null;
			}
		}

		private static void SetupSession()
		{
			isBusy = false;
			session.Breakpoints = breakpoints;
			session.TargetEvent += OnTargetEvent;
			session.TargetStarted += OnStarted;
			session.OutputWriter = OutputWriter;
			session.LogWriter = LogWriter;
			session.DebugWriter = DebugWriter;
			session.BusyStateChanged += OnBusyStateChanged;
			session.TypeResolverHandler = ResolveType;
			session.BreakpointTraceHandler = BreakpointTraceHandler;
			session.GetExpressionEvaluator = OnGetExpressionEvaluator;
			session.ConnectionDialogCreator = () => new StatusBarConnectionDialog();
			console.CancelRequested += OnCancelRequested;
			DispatchService.GuiDispatch(delegate
			{
				if (DebugSessionStarted != null)
				{
					DebugSessionStarted(null, EventArgs.Empty);
				}
				NotifyLocationChanged();
			});
		}

		private static void Cleanup()
		{
			StatusBarIcon currentIcon;
			DebuggerSession debuggerSession;
			IConsole console;
			lock (cleanup_lock)
			{
				if (!IsDebugging)
				{
					return;
				}
				currentIcon = busyStatusIcon;
				debuggerSession = session;
				console = DebuggingService.console;
				nextStatementLocations.Clear();
				currentBacktrace = null;
				busyStatusIcon = null;
				session = null;
				DebuggingService.console = null;
				pinnedWatches.InvalidateAll();
			}
			UnsetDebugLayout();
			debuggerSession.BusyStateChanged -= OnBusyStateChanged;
			debuggerSession.TargetEvent -= OnTargetEvent;
			debuggerSession.TargetStarted -= OnStarted;
			debuggerSession.BreakpointTraceHandler = null;
			debuggerSession.GetExpressionEvaluator = null;
			debuggerSession.TypeResolverHandler = null;
			debuggerSession.OutputWriter = null;
			debuggerSession.LogWriter = null;
			if (console != null)
			{
				console.CancelRequested -= OnCancelRequested;
				console.Dispose();
			}
			DispatchService.GuiDispatch(delegate
			{
				HideExceptionCaughtDialog();
				if (currentIcon != null)
				{
					currentIcon.Dispose();
					currentIcon = null;
				}
				if (StoppedEvent != null)
				{
					StoppedEvent(null, new EventArgs());
				}
				NotifyCallStackChanged();
				NotifyCurrentFrameChanged();
				NotifyLocationChanged();
			});
			debuggerSession.Dispose();
		}

		private static void UnsetDebugLayout()
		{
			DispatchService.GuiSyncDispatch(delegate
			{
				IdeApp.Workbench.HideCommandBar("Debug");
				if (IdeApp.Workbench.CurrentLayout == "Debug")
				{
					IdeApp.Workbench.CurrentLayout = "Solution";
				}
			});
		}

		private static void SetDebugLayout()
		{
			DispatchService.GuiSyncDispatch(delegate
			{
				IdeApp.Workbench.CurrentLayout = "Debug";
				IdeApp.Workbench.ShowCommandBar("Debug");
			});
		}

		public static void Pause()
		{
			session.Stop();
		}

		public static void Resume()
		{
			if (!CheckIsBusy())
			{
				session.Continue();
				NotifyLocationChanged();
			}
		}

		public static void RunToCursor(string fileName, int line, int column)
		{
			if (!CheckIsBusy())
			{
				RunToCursorBreakpoint bp = new RunToCursorBreakpoint(fileName, line, column);
				Breakpoints.Add(bp);
				session.Continue();
				NotifyLocationChanged();
			}
		}

		public static void SetNextStatement(string fileName, int line, int column)
		{
			if (IsDebugging && !IsRunning && !CheckIsBusy())
			{
				session.SetNextStatement(fileName, line, column);
				SourceLocation value = new SourceLocation(CurrentFrame.SourceLocation.MethodName, fileName, line);
				nextStatementLocations[session.ActiveThread.Id] = value;
				NotifyLocationChanged();
			}
		}

		public static IProcessAsyncOperation Run(string file, IConsole console)
		{
			return Run(file, null, null, null, console);
		}

		public static IProcessAsyncOperation Run(string file, string args, string workingDir, IDictionary<string, string> envVars, IConsole console)
		{
			DebugExecutionHandler debugExecutionHandler = new DebugExecutionHandler(null);
			ProcessExecutionCommand processExecutionCommand = Runtime.ProcessService.CreateCommand(file);
			if (args != null)
			{
				processExecutionCommand.Arguments = args;
			}
			if (workingDir != null)
			{
				processExecutionCommand.WorkingDirectory = workingDir;
			}
			if (envVars != null)
			{
				processExecutionCommand.EnvironmentVariables = envVars;
			}
			return debugExecutionHandler.Execute(processExecutionCommand, console);
		}

		public static IAsyncOperation AttachToProcess(DebuggerEngine debugger, ProcessInfo proc)
		{
			currentEngine = debugger;
			session = debugger.CreateSession();
			session.ExceptionHandler = ExceptionHandler;
			IProgressMonitor monitor = IdeApp.Workbench.ProgressMonitors.GetRunProgressMonitor();
			console = monitor as IConsole;
			SetupSession();
			session.TargetExited += delegate
			{
				monitor.Dispose();
			};
			SetDebugLayout();
			session.AttachToProcess(proc, GetUserOptions());
			return monitor.AsyncOperation;
		}

		public static DebuggerSessionOptions GetUserOptions()
		{
			EvaluationOptions defaultOptions = EvaluationOptions.DefaultOptions;
			defaultOptions.AllowTargetInvoke = PropertyService.Get("MonoDevelop.Debugger.DebuggingService.AllowTargetInvoke", defaultValue: true);
			defaultOptions.AllowToStringCalls = PropertyService.Get("MonoDevelop.Debugger.DebuggingService.AllowToStringCalls", defaultValue: true);
			defaultOptions.EvaluationTimeout = PropertyService.Get("MonoDevelop.Debugger.DebuggingService.EvaluationTimeout", 2500);
			defaultOptions.FlattenHierarchy = PropertyService.Get("MonoDevelop.Debugger.DebuggingService.FlattenHierarchy", defaultValue: false);
			defaultOptions.GroupPrivateMembers = PropertyService.Get("MonoDevelop.Debugger.DebuggingService.GroupPrivateMembers", defaultValue: true);
			defaultOptions.GroupStaticMembers = PropertyService.Get("MonoDevelop.Debugger.DebuggingService.GroupStaticMembers", defaultValue: true);
			defaultOptions.MemberEvaluationTimeout = defaultOptions.EvaluationTimeout * 2;
			DebuggerSessionOptions debuggerSessionOptions = new DebuggerSessionOptions();
			debuggerSessionOptions.StepOverPropertiesAndOperators = PropertyService.Get("MonoDevelop.Debugger.DebuggingService.StepOverPropertiesAndOperators", defaultValue: true);
			debuggerSessionOptions.ProjectAssembliesOnly = PropertyService.Get("MonoDevelop.Debugger.DebuggingService.ProjectAssembliesOnly", defaultValue: true);
			debuggerSessionOptions.EvaluationOptions = defaultOptions;
			return debuggerSessionOptions;
		}

		public static void SetUserOptions(DebuggerSessionOptions options)
		{
			PropertyService.Set("MonoDevelop.Debugger.DebuggingService.StepOverPropertiesAndOperators", options.StepOverPropertiesAndOperators);
			PropertyService.Set("MonoDevelop.Debugger.DebuggingService.ProjectAssembliesOnly", options.ProjectAssembliesOnly);
			PropertyService.Set("MonoDevelop.Debugger.DebuggingService.AllowTargetInvoke", options.EvaluationOptions.AllowTargetInvoke);
			PropertyService.Set("MonoDevelop.Debugger.DebuggingService.AllowToStringCalls", options.EvaluationOptions.AllowToStringCalls);
			PropertyService.Set("MonoDevelop.Debugger.DebuggingService.EvaluationTimeout", options.EvaluationOptions.EvaluationTimeout);
			PropertyService.Set("MonoDevelop.Debugger.DebuggingService.FlattenHierarchy", options.EvaluationOptions.FlattenHierarchy);
			PropertyService.Set("MonoDevelop.Debugger.DebuggingService.GroupPrivateMembers", options.EvaluationOptions.GroupPrivateMembers);
			PropertyService.Set("MonoDevelop.Debugger.DebuggingService.GroupStaticMembers", options.EvaluationOptions.GroupStaticMembers);
			if (session != null)
			{
				session.Options.EvaluationOptions = GetUserOptions().EvaluationOptions;
				if (EvaluationOptionsChanged != null)
				{
					EvaluationOptionsChanged(null, EventArgs.Empty);
				}
			}
		}

		public static void ShowDisassembly()
		{
			if (DisassemblyRequested != null)
			{
				DisassemblyRequested(null, EventArgs.Empty);
			}
		}

		internal static void InternalRun(ExecutionCommand cmd, DebuggerEngine factory, IConsole c)
		{
			if (factory == null)
			{
				factory = GetFactoryForCommand(cmd);
				if (factory == null)
				{
					throw new InvalidOperationException("Unsupported command: " + cmd);
				}
			}
			if (session != null)
			{
				throw new InvalidOperationException("A debugger session is already started");
			}
			DebuggerStartInfo debuggerStartInfo = factory.CreateDebuggerStartInfo(cmd);
			debuggerStartInfo.UseExternalConsole = c is ExternalConsole;
			debuggerStartInfo.CloseExternalConsoleOnExit = c.CloseOnDispose;
			currentEngine = factory;
			session = factory.CreateSession();
			session.ExceptionHandler = ExceptionHandler;
			if (debuggerStartInfo.UseExternalConsole)
			{
				console = (IConsole)IdeApp.Workbench.ProgressMonitors.GetRunProgressMonitor();
			}
			else
			{
				console = c;
			}
			SetupSession();
			SetDebugLayout();
			try
			{
				session.Run(debuggerStartInfo, GetUserOptions());
			}
			catch
			{
				Cleanup();
				throw;
			}
		}

		private static bool ExceptionHandler(Exception ex)
		{
			Gtk.Application.Invoke(delegate
			{
				if (ex is DebuggerException)
				{
					MessageService.ShowError(ex.Message);
				}
				else
				{
					MessageService.ShowError("Debugger operation failed", ex);
				}
			});
			return true;
		}

		private static void LogWriter(bool iserr, string text)
		{
			console?.Log.Write(text);
		}

		private static void DebugWriter(int level, string category, string message)
		{
			IConsole console = DebuggingService.console;
			IDebugConsole debugConsole = console as IDebugConsole;
			if (console != null)
			{
				if (debugConsole != null)
				{
					debugConsole.Debug(level, category, message);
				}
				else if (level == 0 && string.IsNullOrEmpty(category))
				{
					console.Log.Write(message);
				}
				else
				{
					console.Log.Write($"[{level}:{category}] {message}");
				}
			}
		}

		private static void OutputWriter(bool iserr, string text)
		{
			IConsole console = DebuggingService.console;
			if (console != null)
			{
				if (iserr)
				{
					console.Error.Write(text);
				}
				else
				{
					console.Out.Write(text);
				}
			}
		}

		private static void OnBusyStateChanged(object s, BusyStateEventArgs args)
		{
			isBusy = args.IsBusy;
			DispatchService.GuiDispatch(delegate
			{
				busyDialog.UpdateBusyState(args);
				if (args.IsBusy)
				{
					if (busyStatusIcon == null)
					{
						busyStatusIcon = IdeApp.Workbench.StatusBar.ShowStatusIcon(ImageService.GetIcon("md-execute-debug", Gtk.IconSize.Menu));
						busyStatusIcon.SetAlertMode(100);
						busyStatusIcon.ToolTip = GettextCatalog.GetString("The Debugger is waiting for an expression evaluation to finish.");
						busyStatusIcon.Clicked += delegate
						{
							MessageService.PlaceDialog(busyDialog, MessageService.RootWindow);
						};
					}
				}
				else if (busyStatusIcon != null)
				{
					busyStatusIcon.Dispose();
					busyStatusIcon = null;
				}
			});
		}

		private static bool CheckIsBusy()
		{
			if (isBusy && !busyDialog.Visible)
			{
				MessageService.PlaceDialog(busyDialog, MessageService.RootWindow);
			}
			return isBusy;
		}

		private static void OnStarted(object s, EventArgs a)
		{
			nextStatementLocations.Clear();
			currentBacktrace = null;
			DispatchService.GuiDispatch(delegate
			{
				HideExceptionCaughtDialog();
				if (ResumedEvent != null)
				{
					ResumedEvent(null, a);
				}
				NotifyCallStackChanged();
				NotifyCurrentFrameChanged();
				NotifyLocationChanged();
			});
		}

		private static void OnTargetEvent(object sender, TargetEventArgs args)
		{
			nextStatementLocations.Clear();
			try
			{
				switch (args.Type)
				{
				case TargetEventType.TargetExited:
					Breakpoints.RemoveRunToCursorBreakpoints();
					Cleanup();
					break;
				case TargetEventType.TargetStopped:
				case TargetEventType.TargetInterrupted:
				case TargetEventType.TargetHitBreakpoint:
				case TargetEventType.TargetSignaled:
				case TargetEventType.ExceptionThrown:
				case TargetEventType.UnhandledException:
					Breakpoints.RemoveRunToCursorBreakpoints();
					SetCurrentBacktrace(args.Backtrace);
					NotifyPaused();
					NotifyException(args);
					break;
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Error handling debugger target event", ex);
			}
		}

		private static void OnDisableConditionalCompilation(DocumentEventArgs e)
		{
			DisableConditionalCompilation?.Invoke(null, e);
		}

		private static void NotifyPaused()
		{
			DispatchService.GuiDispatch(delegate
			{
				if (PausedEvent != null)
				{
					PausedEvent(null, EventArgs.Empty);
				}
				NotifyLocationChanged();
				IdeApp.Workbench.GrabDesktopFocus();
			});
		}

		private static void NotifyException(TargetEventArgs args)
		{
			if (args.Type != TargetEventType.UnhandledException && args.Type != TargetEventType.ExceptionThrown)
			{
				return;
			}
			DispatchService.GuiDispatch(delegate
			{
				if (CurrentFrame != null)
				{
					ShowExceptionCaughtDialog();
				}
			});
		}

		private static void NotifyLocationChanged()
		{
			if (ExecutionLocationChanged != null)
			{
				ExecutionLocationChanged(null, EventArgs.Empty);
			}
		}

		private static void NotifyCurrentFrameChanged()
		{
			if (currentBacktrace != null)
			{
				pinnedWatches.InvalidateAll();
			}
			if (CurrentFrameChanged != null)
			{
				CurrentFrameChanged(null, EventArgs.Empty);
			}
		}

		private static void NotifyCallStackChanged()
		{
			if (CallStackChanged != null)
			{
				CallStackChanged(null, EventArgs.Empty);
			}
		}

		private static void OnCancelRequested(object sender, EventArgs args)
		{
			Stop();
		}

		public static void Stop()
		{
			if (IsDebugging)
			{
				session.Exit();
				Cleanup();
			}
		}

		public static void StepInto()
		{
			if (IsDebugging && !IsRunning && !CheckIsBusy())
			{
				session.StepLine();
				NotifyLocationChanged();
			}
		}

		public static void StepOver()
		{
			if (IsDebugging && !IsRunning && !CheckIsBusy())
			{
				session.NextLine();
				NotifyLocationChanged();
			}
		}

		public static void StepOut()
		{
			if (IsDebugging && !IsRunning && !CheckIsBusy())
			{
				session.Finish();
				NotifyLocationChanged();
			}
		}

		public static StackFrame GetCurrentVisibleFrame()
		{
			if (currentBacktrace != null && currentFrame != -1)
			{
				for (int i = currentFrame; i < currentBacktrace.FrameCount; i++)
				{
					StackFrame frame = currentBacktrace.GetFrame(currentFrame);
					if (!frame.IsExternalCode)
					{
						return frame;
					}
				}
			}
			return null;
		}

		private static void SetCurrentBacktrace(Backtrace bt)
		{
			currentBacktrace = bt;
			if (currentBacktrace != null)
			{
				currentFrame = 0;
			}
			else
			{
				currentFrame = -1;
			}
			DispatchService.GuiDispatch(delegate
			{
				NotifyCallStackChanged();
				NotifyCurrentFrameChanged();
				NotifyLocationChanged();
			});
		}

		public static void ShowCurrentExecutionLine()
		{
			if (currentBacktrace != null)
			{
				StackFrame currentVisibleFrame = GetCurrentVisibleFrame();
				if (currentVisibleFrame != null && !string.IsNullOrEmpty(currentVisibleFrame.SourceLocation.FileName) && File.Exists(currentVisibleFrame.SourceLocation.FileName) && currentVisibleFrame.SourceLocation.Line != -1)
				{
					Document document = IdeApp.Workbench.OpenDocument(currentVisibleFrame.SourceLocation.FileName, null, currentVisibleFrame.SourceLocation.Line, 1, OpenDocumentOptions.Debugger);
					OnDisableConditionalCompilation(new DocumentEventArgs(document));
				}
			}
		}

		public static void ShowNextStatement()
		{
			SourceLocation nextStatementLocation = NextStatementLocation;
			if (nextStatementLocation != null && File.Exists(nextStatementLocation.FileName))
			{
				Document document = IdeApp.Workbench.OpenDocument(nextStatementLocation.FileName, null, nextStatementLocation.Line, 1, OpenDocumentOptions.Debugger);
				OnDisableConditionalCompilation(new DocumentEventArgs(document));
			}
			else
			{
				ShowCurrentExecutionLine();
			}
		}

		public static bool CanDebugCommand(ExecutionCommand command)
		{
			return GetFactoryForCommand(command) != null;
		}

		public static DebuggerEngine[] GetDebuggerEngines()
		{
			if (engines == null)
			{
				List<DebuggerEngine> list = new List<DebuggerEngine>();
				foreach (DebuggerEngineExtensionNode extensionNode in AddinManager.GetExtensionNodes("/MonoDevelop/Debugging/DebuggerEngines"))
				{
					list.Add(new DebuggerEngine(extensionNode));
				}
				engines = list.ToArray();
			}
			return engines;
		}

		public static Dictionary<string, ExpressionEvaluatorExtensionNode> GetExpressionEvaluators()
		{
			if (evaluators == null)
			{
				Dictionary<string, ExpressionEvaluatorExtensionNode> dictionary = new Dictionary<string, ExpressionEvaluatorExtensionNode>(StringComparer.InvariantCultureIgnoreCase);
				foreach (ExpressionEvaluatorExtensionNode extensionNode in AddinManager.GetExtensionNodes("/MonoDevelop/Debugging/Evaluators"))
				{
					dictionary.Add(extensionNode.extension, extensionNode);
				}
				evaluators = dictionary;
			}
			return evaluators;
		}

		private static DebuggerEngine GetFactoryForCommand(ExecutionCommand cmd)
		{
			DebuggerEngine debuggerEngine = null;
			DebuggerEngine[] debuggerEngines = GetDebuggerEngines();
			foreach (DebuggerEngine debuggerEngine2 in debuggerEngines)
			{
				if (debuggerEngine2.CanDebugCommand(cmd))
				{
					if (debuggerEngine2.IsDefaultDebugger(cmd))
					{
						return debuggerEngine2;
					}
					if (debuggerEngine == null)
					{
						debuggerEngine = debuggerEngine2;
					}
				}
			}
			return debuggerEngine;
		}

		private static void OnLineCountChanged(object ob, LineCountEventArgs a)
		{
			lock (breakpoints)
			{
				foreach (Breakpoint breakpoint in breakpoints.GetBreakpoints())
				{
					if (!((FilePath)breakpoint.FileName == a.TextFile.Name))
					{
						continue;
					}
					if (breakpoint.Line > a.LineNumber)
					{
						if (breakpoint.Line + a.LineCount >= a.LineNumber)
						{
							breakpoints.UpdateBreakpointLine(breakpoint, breakpoint.Line + a.LineCount);
						}
						else
						{
							breakpoints.Remove(breakpoint);
						}
					}
					else if (breakpoint.Line == a.LineNumber && a.LineCount < 0)
					{
						breakpoints.Remove(breakpoint);
					}
				}
			}
		}

		private static void OnStoreUserPrefs(object s, UserPreferencesEventArgs args)
		{
			lock (breakpoints)
			{
				args.Properties.SetValue("MonoDevelop.Ide.DebuggingService.Breakpoints", breakpoints.Save());
			}
			args.Properties.SetValue("MonoDevelop.Ide.DebuggingService.PinnedWatches", pinnedWatches);
		}

		private static void OnLoadUserPrefs(object s, UserPreferencesEventArgs args)
		{
			XmlElement xmlElement = args.Properties.GetValue<XmlElement>("MonoDevelop.Ide.DebuggingService.Breakpoints") ?? args.Properties.GetValue<XmlElement>("MonoDevelop.Ide.DebuggingService");
			if (xmlElement != null)
			{
				lock (breakpoints)
				{
					breakpoints.Load(xmlElement);
				}
			}
			PinnedWatchStore value = args.Properties.GetValue<PinnedWatchStore>("MonoDevelop.Ide.DebuggingService.PinnedWatches");
			if (value != null)
			{
				pinnedWatches.LoadFrom(value);
			}
			lock (breakpoints)
			{
				pinnedWatches.BindAll(breakpoints);
			}
		}

		private static void OnSolutionClosed(object s, EventArgs args)
		{
			lock (breakpoints)
			{
				breakpoints.Clear();
			}
		}

		private static string ResolveType(string identifier, SourceLocation location)
		{
			Document document = IdeApp.Workbench.GetDocument(location.FileName);
			if (document != null)
			{
				ITextEditorResolver content = document.GetContent<ITextEditorResolver>();
				if (content != null)
				{
					ResolveResult languageItem = content.GetLanguageItem(document.Editor.Document.LocationToOffset(location.Line, 1), identifier);
					if (languageItem is NamespaceResolveResult namespaceResolveResult)
					{
						return namespaceResolveResult.NamespaceName;
					}
					if (languageItem is TypeResolveResult typeResolveResult && !typeResolveResult.IsError && (typeResolveResult.Type.Kind != TypeKind.Dynamic || !(typeResolveResult.Type.FullName == "dynamic")))
					{
						return typeResolveResult.Type.FullName;
					}
				}
			}
			return null;
		}

		public static ExpressionEvaluatorExtensionNode EvaluatorForExtension(string extension)
		{
			if (GetExpressionEvaluators().TryGetValue(extension, out var value))
			{
				return value;
			}
			return null;
		}

		private static IExpressionEvaluator OnGetExpressionEvaluator(string extension)
		{
			return EvaluatorForExtension(extension)?.Evaluator;
		}
	}
}
