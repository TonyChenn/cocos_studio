using System;
using System.Collections.Generic;
using System.Linq;
using GLib;
using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Debugger
{
	public class ImmediatePad : IPadContent, IDisposable
	{
		private static readonly object mutex = new object();

		private DebuggerConsoleView view;

		public Widget Control => view;

		public void Initialize(IPadWindow container)
		{
			view = new DebuggerConsoleView();
			view.ConsoleInput += OnViewConsoleInput;
			view.ShadowType = ShadowType.None;
			view.ShowAll();
			view.Editable = DebuggingService.IsPaused;
			DebuggingService.PausedEvent += DebuggerPaused;
			DebuggingService.ResumedEvent += DebuggerResumed;
			DebuggingService.StoppedEvent += DebuggerStopped;
		}

		private void OnViewConsoleInput(object sender, ConsoleInputEventArgs e)
		{
			if (!DebuggingService.IsDebugging)
			{
				view.WriteOutput(GettextCatalog.GetString("Debug session not started."));
				FinishPrinting();
				return;
			}
			if (DebuggingService.IsRunning || DebuggingService.CurrentFrame == null)
			{
				view.WriteOutput(GettextCatalog.GetString("The expression can't be evaluated while the application is running."));
				FinishPrinting();
				return;
			}
			StackFrame currentFrame = DebuggingService.CurrentFrame;
			EvaluationOptions evaluationOptions = GetEvaluationOptions();
			string text = e.Text;
			ValidationResult validationResult = currentFrame.ValidateExpression(text, evaluationOptions);
			if (!validationResult)
			{
				view.WriteOutput(validationResult.Message);
				FinishPrinting();
				return;
			}
			ObjectValue expressionValue = currentFrame.GetExpressionValue(text, evaluationOptions);
			if (expressionValue.IsEvaluating)
			{
				WaitForCompleted(expressionValue);
			}
			else
			{
				PrintValue(expressionValue);
			}
		}

		private static EvaluationOptions GetEvaluationOptions()
		{
			EvaluationOptions defaultOptions = EvaluationOptions.DefaultOptions;
			defaultOptions.AllowMethodEvaluation = true;
			defaultOptions.AllowToStringCalls = true;
			defaultOptions.AllowTargetInvoke = true;
			defaultOptions.EvaluationTimeout = 20000;
			defaultOptions.EllipsizeStrings = false;
			defaultOptions.MemberEvaluationTimeout = 20000;
			return defaultOptions;
		}

		private static string GetErrorText(ObjectValue val)
		{
			if (val.IsNotSupported)
			{
				if (!string.IsNullOrEmpty(val.Value))
				{
					return val.Value;
				}
				return GettextCatalog.GetString("Expression not supported.");
			}
			if (val.IsError || val.IsUnknown)
			{
				if (!string.IsNullOrEmpty(val.Value))
				{
					return val.Value;
				}
				return GettextCatalog.GetString("Evaluation failed.");
			}
			return string.Empty;
		}

		private void PrintValue(ObjectValue val)
		{
			string value = val.Value;
			if (string.IsNullOrEmpty(value) || val.IsError || val.IsUnknown || val.IsNotSupported)
			{
				view.WriteOutput(GetErrorText(val));
				FinishPrinting();
				return;
			}
			EvaluationOptions evaluationOptions = GetEvaluationOptions();
			ObjectValue[] allChildren = val.GetAllChildren(evaluationOptions);
			bool hasMore = false;
			view.WriteOutput(value);
			if (allChildren.Length > 0 && string.Equals(allChildren[0].Name, "[0..99]"))
			{
				allChildren = allChildren[0].GetAllChildren();
				hasMore = true;
			}
			Dictionary<ObjectValue, bool> dictionary = new Dictionary<ObjectValue, bool>();
			ObjectValue[] array = allChildren;
			foreach (ObjectValue objectValue in array)
			{
				if (objectValue.IsEvaluating)
				{
					dictionary.Add(objectValue, value: false);
				}
				else
				{
					PrintChildValue(objectValue);
				}
			}
			if (dictionary.Count > 0)
			{
				foreach (KeyValuePair<ObjectValue, bool> item in dictionary)
				{
					WaitChildForCompleted(item.Key, dictionary, hasMore);
				}
				return;
			}
			FinishPrinting(hasMore);
		}

		private void PrintChildValue(ObjectValue val)
		{
			view.WriteOutput(Environment.NewLine);
			if (val.IsError || val.IsUnknown)
			{
				view.WriteOutput($"\t{GetErrorText(val)}");
			}
			else if (!val.IsNotSupported)
			{
				view.WriteOutput($"\t{val.Name}: {val.Value}");
			}
		}

		private void PrintChildValueAtMark(ObjectValue val, TextMark mark)
		{
			string text = "\t" + val.Name + ": ";
			string value = val.Value;
			if (string.IsNullOrEmpty(value) || val.IsError || val.IsUnknown || val.IsNotSupported)
			{
				SetLineText(text + GetErrorText(val), mark);
			}
			else
			{
				SetLineText(text + value, mark);
			}
		}

		private void FinishPrinting(bool hasMore = false)
		{
			if (hasMore)
			{
				view.WriteOutput("\n\t" + GettextCatalog.GetString("< More... (The first {0} items were displayed.) >", 100));
			}
			view.Prompt(newLine: true);
		}

		private TextIter DeleteLineAtMark(TextMark mark)
		{
			TextIter start = view.Buffer.GetIterAtMark(mark);
			TextIter end = view.Buffer.GetIterAtMark(mark);
			end.ForwardLine();
			view.Buffer.Delete(ref start, ref end);
			return start;
		}

		private void SetLineText(string text, TextMark mark)
		{
			TextIter iter = DeleteLineAtMark(mark);
			view.Buffer.Insert(ref iter, text + "\n");
		}

		private void WaitForCompleted(ObjectValue val)
		{
			TextMark mark = view.Buffer.CreateMark(null, view.InputLineEnd, left_gravity: true);
			int iteration = 0;
			GLib.Timeout.Add(100u, delegate
			{
				if (!val.IsEvaluating)
				{
					if (iteration >= 5)
					{
						DeleteLineAtMark(mark);
					}
					PrintValue(val);
					return false;
				}
				if (++iteration == 5)
				{
					SetLineText(GettextCatalog.GetString("Evaluating"), mark);
				}
				else if (iteration > 5 && (iteration - 5) % 10 == 0)
				{
					string text = string.Join("", Enumerable.Repeat(".", iteration / 10));
					SetLineText(GettextCatalog.GetString("Evaluating") + " " + text, mark);
				}
				return true;
			});
		}

		private void WaitChildForCompleted(ObjectValue val, IDictionary<ObjectValue, bool> evaluatingList, bool hasMore)
		{
			view.WriteOutput("\n ");
			TextMark mark = view.Buffer.CreateMark(null, view.InputLineEnd, left_gravity: true);
			int iteration = 0;
			GLib.Timeout.Add(100u, delegate
			{
				if (!val.IsEvaluating)
				{
					PrintChildValueAtMark(val, mark);
					lock (mutex)
					{
						evaluatingList[val] = true;
						if (evaluatingList.All((KeyValuePair<ObjectValue, bool> x) => x.Value))
						{
							FinishPrinting(hasMore);
						}
					}
					return false;
				}
				string text = "\t" + val.Name + ": ";
				if (++iteration == 5)
				{
					SetLineText(text + GettextCatalog.GetString("Evaluating"), mark);
				}
				else if (iteration > 5 && (iteration - 5) % 10 == 0)
				{
					string text2 = string.Join("", Enumerable.Repeat(".", iteration / 10));
					SetLineText(text + GettextCatalog.GetString("Evaluating") + " " + text2, mark);
				}
				return true;
			});
		}

		public void RedrawContent()
		{
		}

		public void Dispose()
		{
			DebuggingService.PausedEvent -= DebuggerPaused;
			DebuggingService.ResumedEvent -= DebuggerResumed;
			DebuggingService.StoppedEvent -= DebuggerStopped;
		}

		private void DebuggerResumed(object sender, EventArgs e)
		{
			view.Editable = false;
		}

		private void DebuggerPaused(object sender, EventArgs e)
		{
			view.Editable = true;
		}

		private void DebuggerStopped(object sender, EventArgs e)
		{
			view.Editable = false;
		}
	}
}
