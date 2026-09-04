using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Framework;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	public class MDConsoleLogger : ILogger
	{
		private string parameters;

		private int indent;

		private LoggerVerbosity verbosity;

		private WriteHandler writeHandler;

		private int errorCount;

		private int warningCount;

		private DateTime buildStart;

		private bool performanceSummary;

		private bool showSummary;

		private bool skipProjectStartedText;

		private List<string> errors;

		private List<string> warnings;

		private bool projectFailed;

		private ConsoleColor errorColor;

		private ConsoleColor warningColor;

		private ConsoleColor eventColor;

		private ConsoleColor messageColor;

		private ConsoleColor highMessageColor;

		private ColorSetter colorSet;

		private ColorResetter colorReset;

		private IEventSource eventSource;

		private bool no_message_color;

		private bool use_colors;

		private bool noItemAndPropertyList;

		private List<BuildEvent> events;

		private Dictionary<string, List<string>> errorsTable;

		private Dictionary<string, List<string>> warningsTable;

		private SortedDictionary<string, PerfInfo> targetPerfTable;

		private SortedDictionary<string, PerfInfo> tasksPerfTable;

		private string current_events_string;

		private static bool InEmacs = Environment.GetEnvironmentVariable("EMACS") == "t";

		public string Parameters
		{
			get
			{
				return parameters;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				parameters = value;
				if (parameters != string.Empty)
				{
					ParseParameters();
				}
			}
		}

		private string EventsAsString
		{
			get
			{
				if (current_events_string == null)
				{
					current_events_string = EventsToString();
				}
				return current_events_string;
			}
		}

		public bool ShowSummary
		{
			get
			{
				return showSummary;
			}
			set
			{
				showSummary = value;
			}
		}

		public bool SkipProjectStartedText
		{
			get
			{
				return skipProjectStartedText;
			}
			set
			{
				skipProjectStartedText = value;
			}
		}

		public LoggerVerbosity Verbosity
		{
			get
			{
				return verbosity;
			}
			set
			{
				verbosity = value;
			}
		}

		protected WriteHandler WriteHandler
		{
			get
			{
				return writeHandler;
			}
			set
			{
				writeHandler = value;
			}
		}

		public MDConsoleLogger()
			: this(LoggerVerbosity.Normal, null, null, null)
		{
		}

		public MDConsoleLogger(LoggerVerbosity verbosity)
			: this(verbosity, null, null, null)
		{
		}

		public MDConsoleLogger(LoggerVerbosity verbosity, WriteHandler write, ColorSetter colorSet, ColorResetter colorReset)
		{
			this.verbosity = verbosity;
			indent = 0;
			errorCount = 0;
			warningCount = 0;
			if (write == null)
			{
				writeHandler = (WriteHandler)Delegate.Combine(writeHandler, new WriteHandler(WriteHandlerFunction));
			}
			else
			{
				writeHandler = (WriteHandler)Delegate.Combine(writeHandler, write);
			}
			performanceSummary = false;
			showSummary = true;
			skipProjectStartedText = false;
			errors = new List<string>();
			warnings = new List<string>();
			this.colorSet = colorSet;
			this.colorReset = colorReset;
			events = new List<BuildEvent>();
			errorsTable = new Dictionary<string, List<string>>();
			warningsTable = new Dictionary<string, List<string>>();
			targetPerfTable = new SortedDictionary<string, PerfInfo>();
			tasksPerfTable = new SortedDictionary<string, PerfInfo>();
			errorColor = ConsoleColor.DarkRed;
			warningColor = ConsoleColor.DarkYellow;
			eventColor = ConsoleColor.DarkCyan;
			messageColor = ConsoleColor.DarkGray;
			highMessageColor = ConsoleColor.White;
			no_message_color = true;
			use_colors = false;
			if (colorSet == null || colorReset == null)
			{
				return;
			}
			string environmentVariable = Environment.GetEnvironmentVariable("XBUILD_COLORS");
			if (environmentVariable == null)
			{
				use_colors = true;
			}
			else
			{
				if (environmentVariable == "disable")
				{
					return;
				}
				use_colors = true;
				string[] array = environmentVariable.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				string[] array2 = array;
				foreach (string text in array2)
				{
					string[] array3 = text.Split(new char[1] { '=' }, StringSplitOptions.RemoveEmptyEntries);
					if (array3.Length == 2)
					{
						if (array3[0] == "errors")
						{
							TryParseConsoleColor(array3[1], ref errorColor);
						}
						else if (array3[0] == "warnings")
						{
							TryParseConsoleColor(array3[1], ref warningColor);
						}
						else if (array3[0] == "events")
						{
							TryParseConsoleColor(array3[1], ref eventColor);
						}
						else if (array3[0] == "messages" && TryParseConsoleColor(array3[1], ref messageColor))
						{
							highMessageColor = GetBrightColorFor(messageColor);
							no_message_color = false;
						}
					}
				}
			}
		}

		private bool TryParseConsoleColor(string color_str, ref ConsoleColor color)
		{
			switch (color_str.ToLower())
			{
			case "black":
				color = ConsoleColor.Black;
				break;
			case "blue":
				color = ConsoleColor.DarkBlue;
				break;
			case "green":
				color = ConsoleColor.DarkGreen;
				break;
			case "cyan":
				color = ConsoleColor.DarkCyan;
				break;
			case "red":
				color = ConsoleColor.DarkRed;
				break;
			case "magenta":
				color = ConsoleColor.DarkMagenta;
				break;
			case "yellow":
				color = ConsoleColor.DarkYellow;
				break;
			case "grey":
				color = ConsoleColor.DarkGray;
				break;
			case "brightgrey":
				color = ConsoleColor.Gray;
				break;
			case "brightblue":
				color = ConsoleColor.Blue;
				break;
			case "brightgreen":
				color = ConsoleColor.Green;
				break;
			case "brightcyan":
				color = ConsoleColor.Cyan;
				break;
			case "brightred":
				color = ConsoleColor.Red;
				break;
			case "brightmagenta":
				color = ConsoleColor.Magenta;
				break;
			case "brightyellow":
				color = ConsoleColor.Yellow;
				break;
			case "white":
			case "brightwhite":
				color = ConsoleColor.White;
				break;
			default:
				return false;
			}
			return true;
		}

		private ConsoleColor GetBrightColorFor(ConsoleColor color)
		{
			switch (color)
			{
			case ConsoleColor.DarkBlue:
				return ConsoleColor.Blue;
			case ConsoleColor.DarkGreen:
				return ConsoleColor.Green;
			case ConsoleColor.DarkCyan:
				return ConsoleColor.Cyan;
			case ConsoleColor.DarkRed:
				return ConsoleColor.Red;
			case ConsoleColor.DarkMagenta:
				return ConsoleColor.Magenta;
			case ConsoleColor.DarkYellow:
				return ConsoleColor.Yellow;
			case ConsoleColor.DarkGray:
				return ConsoleColor.Gray;
			case ConsoleColor.Gray:
				return ConsoleColor.White;
			default:
				return color;
			}
		}

		public void ApplyParameter(string parameterName, string parameterValue)
		{
		}

		public virtual void Initialize(IEventSource eventSource)
		{
			this.eventSource = eventSource;
			eventSource.BuildStarted += BuildStartedHandler;
			eventSource.BuildFinished += BuildFinishedHandler;
			eventSource.ProjectStarted += PushEvent;
			eventSource.ProjectFinished += PopEvent;
			eventSource.TargetStarted += PushEvent;
			eventSource.TargetFinished += PopEvent;
			eventSource.TaskStarted += PushEvent;
			eventSource.TaskFinished += PopEvent;
			eventSource.MessageRaised += MessageHandler;
			eventSource.WarningRaised += WarningHandler;
			eventSource.ErrorRaised += ErrorHandler;
		}

		public void BuildStartedHandler(object sender, BuildStartedEventArgs args)
		{
			if (IsVerbosityGreaterOrEqual(LoggerVerbosity.Normal))
			{
				WriteLine(string.Empty);
				WriteLine($"Build started {args.Timestamp}.");
				WriteLine("__________________________________________________");
			}
			buildStart = args.Timestamp;
			PushEvent(args);
		}

		public void BuildFinishedHandler(object sender, BuildFinishedEventArgs args)
		{
			BuildFinishedHandlerActual(args);
			events.Clear();
			errorsTable.Clear();
			warningsTable.Clear();
			targetPerfTable.Clear();
			tasksPerfTable.Clear();
			errors.Clear();
			warnings.Clear();
			indent = 0;
			errorCount = 0;
			warningCount = 0;
			projectFailed = false;
		}

		private void BuildFinishedHandlerActual(BuildFinishedEventArgs args)
		{
			if (!IsVerbosityGreaterOrEqual(LoggerVerbosity.Normal))
			{
				PopEvent(args);
				return;
			}
			TimeSpan timeSpan = args.Timestamp - buildStart;
			if (performanceSummary || verbosity == LoggerVerbosity.Diagnostic)
			{
				DumpPerformanceSummary();
			}
			if (args.Succeeded && !projectFailed)
			{
				WriteLine("Build succeeded.");
			}
			else
			{
				WriteLine("Build FAILED.");
			}
			if (warnings.Count > 0)
			{
				WriteLine(Environment.NewLine + "Warnings:");
				SetColor(warningColor);
				WriteLine(string.Empty);
				foreach (KeyValuePair<string, List<string>> item in warningsTable)
				{
					if (!string.IsNullOrEmpty(item.Key))
					{
						WriteLine(item.Key);
					}
					string arg = (string.IsNullOrEmpty(item.Key) ? string.Empty : "\t");
					foreach (string item2 in item.Value)
					{
						WriteLine($"{arg}{item2}");
					}
					WriteLine(string.Empty);
				}
				ResetColor();
			}
			if (errors.Count > 0)
			{
				WriteLine("Errors:");
				SetColor(errorColor);
				WriteLine(string.Empty);
				foreach (KeyValuePair<string, List<string>> item3 in errorsTable)
				{
					if (!string.IsNullOrEmpty(item3.Key))
					{
						WriteLine(item3.Key);
					}
					string arg = (string.IsNullOrEmpty(item3.Key) ? string.Empty : "\t");
					foreach (string item4 in item3.Value)
					{
						WriteLine($"{arg}{item4}");
					}
					WriteLine(string.Empty);
				}
				ResetColor();
			}
			if (showSummary)
			{
				WriteLine($"\t {warningCount} Warning(s)");
				WriteLine($"\t {errorCount} Error(s)");
				WriteLine(string.Empty);
				WriteLine($"Time Elapsed {timeSpan}");
			}
			PopEvent(args);
		}

		public void ProjectStartedHandler(object sender, ProjectStartedEventArgs args)
		{
			if (IsVerbosityGreaterOrEqual(LoggerVerbosity.Normal))
			{
				SetColor(eventColor);
				WriteLine(string.Format("Project \"{0}\" ({1} target(s)):", args.ProjectFile, string.IsNullOrEmpty(args.TargetNames) ? "default" : args.TargetNames));
				ResetColor();
				DumpProperties(args.Properties);
				DumpItems(args.Items);
			}
		}

		public void ProjectFinishedHandler(object sender, ProjectFinishedEventArgs args)
		{
			if (IsVerbosityGreaterOrEqual(LoggerVerbosity.Normal))
			{
				if (indent == 1)
				{
					indent--;
				}
				SetColor(eventColor);
				WriteLine(string.Format("Done building project \"{0}\".{1}", args.ProjectFile, args.Succeeded ? string.Empty : "-- FAILED"));
				ResetColor();
				WriteLine(string.Empty);
			}
			if (!projectFailed)
			{
				projectFailed = !args.Succeeded;
			}
		}

		public void TargetStartedHandler(object sender, TargetStartedEventArgs args)
		{
			if (IsVerbosityGreaterOrEqual(LoggerVerbosity.Normal))
			{
				indent++;
				SetColor(eventColor);
				WriteLine(string.Empty);
				WriteLine($"Target {args.TargetName}:");
				ResetColor();
			}
		}

		public void TargetFinishedHandler(object sender, TargetFinishedEventArgs args)
		{
			if (IsVerbosityGreaterOrEqual(LoggerVerbosity.Detailed) || (!args.Succeeded && IsVerbosityGreaterOrEqual(LoggerVerbosity.Normal)))
			{
				SetColor(eventColor);
				WriteLine(string.Format("Done building target \"{0}\" in project \"{1}\".{2}", args.TargetName, args.ProjectFile, args.Succeeded ? string.Empty : "-- FAILED"));
				ResetColor();
				WriteLine(string.Empty);
			}
			indent--;
		}

		public void TaskStartedHandler(object sender, TaskStartedEventArgs args)
		{
			if (IsVerbosityGreaterOrEqual(LoggerVerbosity.Detailed))
			{
				SetColor(eventColor);
				WriteLine($"Task \"{args.TaskName}\"");
				ResetColor();
			}
			indent++;
		}

		public void TaskFinishedHandler(object sender, TaskFinishedEventArgs args)
		{
			indent--;
			if (IsVerbosityGreaterOrEqual(LoggerVerbosity.Detailed) || (!args.Succeeded && IsVerbosityGreaterOrEqual(LoggerVerbosity.Normal)))
			{
				SetColor(eventColor);
				if (args.Succeeded)
				{
					WriteLine($"Done executing task \"{args.TaskName}\"");
				}
				else
				{
					WriteLine($"Task \"{args.TaskName}\" execution -- FAILED");
				}
				ResetColor();
			}
		}

		public void MessageHandler(object sender, BuildMessageEventArgs args)
		{
			if (IsMessageOk(args))
			{
				if (no_message_color)
				{
					ExecutePendingEventHandlers();
					WriteLine(args.Message);
					return;
				}
				ExecutePendingEventHandlers();
				SetColor((args.Importance == MessageImportance.High) ? highMessageColor : messageColor);
				WriteLine(args.Message);
				ResetColor();
			}
		}

		public void WarningHandler(object sender, BuildWarningEventArgs args)
		{
			string text = FormatWarningEvent(args);
			if (IsVerbosityGreaterOrEqual(LoggerVerbosity.Quiet))
			{
				ExecutePendingEventHandlers();
				SetColor(warningColor);
				WriteLineWithoutIndent(text);
				ResetColor();
			}
			warnings.Add(text);
			List<string> value = null;
			if (!warningsTable.TryGetValue(EventsAsString, out value))
			{
				value = (warningsTable[EventsAsString] = new List<string>());
			}
			value.Add(text);
			warningCount++;
		}

		public void ErrorHandler(object sender, BuildErrorEventArgs args)
		{
			string text = FormatErrorEvent(args);
			if (IsVerbosityGreaterOrEqual(LoggerVerbosity.Quiet))
			{
				ExecutePendingEventHandlers();
				SetColor(errorColor);
				WriteLineWithoutIndent(text);
				ResetColor();
			}
			errors.Add(text);
			List<string> value = null;
			if (!errorsTable.TryGetValue(EventsAsString, out value))
			{
				value = (errorsTable[EventsAsString] = new List<string>());
			}
			value.Add(text);
			errorCount++;
		}

		public void CustomEventHandler(object sender, CustomBuildEventArgs args)
		{
		}

		private void WriteLine(string message)
		{
			if (indent > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < indent; i++)
				{
					stringBuilder.Append('\t');
				}
				stringBuilder.Append(message);
				writeHandler(stringBuilder.ToString());
			}
			else
			{
				writeHandler(message);
			}
		}

		private void PushEvent<T>(object sender, T args) where T : BuildStatusEventArgs
		{
			PushEvent(args);
		}

		private void PushEvent<T>(T args) where T : BuildStatusEventArgs
		{
			BuildEvent buildEvent = new BuildEvent();
			buildEvent.EventArgs = args;
			buildEvent.StartHandlerHasExecuted = false;
			buildEvent.ConsoleLogger = this;
			BuildEvent item = buildEvent;
			events.Add(item);
			current_events_string = null;
		}

		private void PopEvent<T>(object sender, T finished_args) where T : BuildStatusEventArgs
		{
			PopEvent(finished_args);
		}

		private void PopEvent<T>(T finished_args) where T : BuildStatusEventArgs
		{
			if (events.Count == 0)
			{
				throw new InvalidOperationException("INTERNAL ERROR: Trying to pop from an empty events stack");
			}
			BuildEvent buildEvent = events[events.Count - 1];
			if (performanceSummary || verbosity == LoggerVerbosity.Diagnostic)
			{
				BuildStatusEventArgs eventArgs = buildEvent.EventArgs;
				if (eventArgs is TargetStartedEventArgs e)
				{
					AddPerfInfo(e.TargetName, eventArgs.Timestamp, targetPerfTable);
				}
				else if (eventArgs is TaskStartedEventArgs e2)
				{
					AddPerfInfo(e2.TaskName, eventArgs.Timestamp, tasksPerfTable);
				}
			}
			buildEvent.ExecuteFinishedHandler(finished_args);
			events.RemoveAt(events.Count - 1);
			current_events_string = null;
		}

		private void ExecutePendingEventHandlers()
		{
			foreach (BuildEvent @event in events)
			{
				@event.ExecuteStartedHandler();
			}
		}

		private string EventsToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = string.Empty;
			for (int i = 0; i < events.Count; i++)
			{
				BuildStatusEventArgs eventArgs = events[i].EventArgs;
				if (eventArgs is ProjectStartedEventArgs e)
				{
					stringBuilder.AppendFormat("{0} ({1}) ->\n", e.ProjectFile, string.IsNullOrEmpty(e.TargetNames) ? "default targets" : e.TargetNames);
					text = string.Empty;
				}
				else if (eventArgs is TargetStartedEventArgs e2)
				{
					if (e2.TargetFile != e2.ProjectFile && e2.TargetFile != text)
					{
						stringBuilder.AppendFormat("{0} ", e2.TargetFile);
					}
					text = e2.TargetFile;
					stringBuilder.AppendFormat("({0} target) ->\n", e2.TargetName);
				}
			}
			return stringBuilder.ToString();
		}

		private void AddPerfInfo(string name, DateTime start, IDictionary<string, PerfInfo> perf_table)
		{
			if (!perf_table.TryGetValue(name, out var value))
			{
				value = (perf_table[name] = new PerfInfo());
			}
			value.Time += DateTime.Now - start;
			value.NumberOfCalls++;
		}

		private void DumpPerformanceSummary()
		{
			SetColor(eventColor);
			WriteLine("Target perfomance summary:");
			ResetColor();
			foreach (KeyValuePair<string, PerfInfo> item in targetPerfTable)
			{
				WriteLine($"{item.Value.Time.TotalMilliseconds,10:0.000} ms  {item.Key,-50}  {item.Value.NumberOfCalls,5} calls");
			}
			WriteLine(string.Empty);
			SetColor(eventColor);
			WriteLine("Tasks perfomance summary:");
			ResetColor();
			foreach (KeyValuePair<string, PerfInfo> item2 in tasksPerfTable)
			{
				WriteLine($"{item2.Value.Time.TotalMilliseconds,10:0.000} ms  {item2.Key,-50}  {item2.Value.NumberOfCalls,5} calls");
			}
			WriteLine(string.Empty);
		}

		private void WriteLineWithoutIndent(string message)
		{
			writeHandler(message);
		}

		private void WriteHandlerFunction(string message)
		{
			Console.WriteLine(message);
		}

		private void SetColor(ConsoleColor color)
		{
			if (use_colors)
			{
				colorSet(color);
			}
		}

		private void ResetColor()
		{
			if (use_colors)
			{
				colorReset();
			}
		}

		private void ParseParameters()
		{
			string[] array = parameters.Split(';');
			string[] array2 = array;
			foreach (string text in array2)
			{
				switch (text)
				{
				case "PerformanceSummary":
					performanceSummary = true;
					break;
				case "NoSummary":
					showSummary = false;
					break;
				case "NoItemAndPropertyList":
					noItemAndPropertyList = true;
					break;
				default:
					throw new ArgumentException("Invalid parameter : " + text);
				}
			}
		}

		public virtual void Shutdown()
		{
			if (eventSource != null)
			{
				eventSource.BuildStarted -= BuildStartedHandler;
				eventSource.BuildFinished -= BuildFinishedHandler;
				eventSource.ProjectStarted -= PushEvent;
				eventSource.ProjectFinished -= PopEvent;
				eventSource.TargetStarted -= PushEvent;
				eventSource.TargetFinished -= PopEvent;
				eventSource.TaskStarted -= PushEvent;
				eventSource.TaskFinished -= PopEvent;
				eventSource.MessageRaised -= MessageHandler;
				eventSource.WarningRaised -= WarningHandler;
				eventSource.ErrorRaised -= ErrorHandler;
			}
		}

		private string FormatErrorEvent(BuildErrorEventArgs args)
		{
			string text = ((args.Subcategory == null || args.Subcategory == "" || args.Subcategory == " ") ? "" : " ");
			string text2 = ((text == "") ? "" : args.Subcategory);
			if (args.LineNumber != 0)
			{
				if (args.ColumnNumber != 0 && !InEmacs)
				{
					return $"{args.File}({args.LineNumber},{args.ColumnNumber}): {text}{text2}error {args.Code}: {args.Message}";
				}
				return $"{args.File}({args.LineNumber}): {text}{text2}error {args.Code}: {args.Message}";
			}
			return $"{args.File}: {text}{text2}error {args.Code}: {args.Message}";
		}

		private string FormatWarningEvent(BuildWarningEventArgs args)
		{
			string text = ((args.Subcategory == null || args.Subcategory == "" || args.Subcategory == " ") ? "" : " ");
			string text2 = ((text == "") ? "" : args.Subcategory);
			if (args.LineNumber != 0)
			{
				if (args.ColumnNumber != 0 && !InEmacs)
				{
					return $"{args.File}({args.LineNumber},{args.ColumnNumber}): {text}{text2}warning {args.Code}: {args.Message}";
				}
				return $"{args.File}({args.LineNumber}): {text}{text2}warning {args.Code}: {args.Message}";
			}
			return $"{args.File}: {args.Subcategory} warning {args.Code}: {args.Message}";
		}

		private bool IsMessageOk(BuildMessageEventArgs bsea)
		{
			if (bsea.Importance == MessageImportance.High && IsVerbosityGreaterOrEqual(LoggerVerbosity.Minimal))
			{
				return true;
			}
			if (bsea.Importance == MessageImportance.Normal && IsVerbosityGreaterOrEqual(LoggerVerbosity.Normal))
			{
				return true;
			}
			if (bsea.Importance == MessageImportance.Low && IsVerbosityGreaterOrEqual(LoggerVerbosity.Detailed))
			{
				return true;
			}
			return false;
		}

		private bool IsVerbosityGreaterOrEqual(LoggerVerbosity v)
		{
			switch (v)
			{
			case LoggerVerbosity.Diagnostic:
				return LoggerVerbosity.Diagnostic <= verbosity;
			case LoggerVerbosity.Detailed:
				return LoggerVerbosity.Detailed <= verbosity;
			case LoggerVerbosity.Normal:
				return LoggerVerbosity.Normal <= verbosity;
			case LoggerVerbosity.Minimal:
				return LoggerVerbosity.Minimal <= verbosity;
			case LoggerVerbosity.Quiet:
				return true;
			default:
				return false;
			}
		}

		private void DumpProperties(IEnumerable properties)
		{
			if (noItemAndPropertyList || !IsVerbosityGreaterOrEqual(LoggerVerbosity.Diagnostic))
			{
				return;
			}
			SetColor(eventColor);
			WriteLine(string.Empty);
			WriteLine("Initial Properties:");
			ResetColor();
			if (properties == null)
			{
				return;
			}
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			foreach (DictionaryEntry property in properties)
			{
				sortedDictionary[(string)property.Key] = (string)property.Value;
			}
			foreach (KeyValuePair<string, string> item in sortedDictionary)
			{
				WriteLine($"{item.Key} = {item.Value}");
			}
		}

		private void DumpItems(IEnumerable items)
		{
			if (noItemAndPropertyList || !IsVerbosityGreaterOrEqual(LoggerVerbosity.Diagnostic) || items == null)
			{
				return;
			}
			SetColor(eventColor);
			WriteLine(string.Empty);
			WriteLine("Initial Items:");
			ResetColor();
			if (items == null)
			{
				return;
			}
			SortedDictionary<string, List<ITaskItem>> sortedDictionary = new SortedDictionary<string, List<ITaskItem>>();
			foreach (DictionaryEntry item in items)
			{
				string key = (string)item.Key;
				if (!sortedDictionary.ContainsKey(key))
				{
					sortedDictionary[key] = new List<ITaskItem>();
				}
				sortedDictionary[key].Add((ITaskItem)item.Value);
			}
			foreach (string key2 in sortedDictionary.Keys)
			{
				WriteLine(key2);
				indent++;
				foreach (ITaskItem item2 in sortedDictionary[key2])
				{
					WriteLine(item2.ItemSpec);
				}
				indent--;
			}
		}
	}
}
