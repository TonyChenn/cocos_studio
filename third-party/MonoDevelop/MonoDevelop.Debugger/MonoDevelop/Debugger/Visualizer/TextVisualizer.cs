using System;
using GLib;
using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Core;

namespace MonoDevelop.Debugger.Visualizer
{
	public class TextVisualizer : ValueVisualizer
	{
		private const int CHUNK_SIZE = 1024;

		private RawValueString rawString;

		private RawValueArray rawArray;

		private TextView textView;

		private uint idle_id;

		private int length;

		private int offset;

		public override string Name => GettextCatalog.GetString("Text");

		public override bool CanVisualize(ObjectValue val)
		{
			switch (val.TypeName)
			{
			case "char[]":
				return true;
			case "string":
				return true;
			default:
				return false;
			}
		}

		public override bool IsDefaultVisualizer(ObjectValue val)
		{
			return true;
		}

		private bool GetNextCharArrayChunk()
		{
			int num = Math.Min(length - offset, 1024);
			char[] value = rawArray.GetValues(offset, num) as char[];
			TextIter iter = textView.Buffer.EndIter;
			textView.Buffer.Insert(ref iter, new string(value));
			offset += num;
			if (offset < length)
			{
				return true;
			}
			idle_id = 0u;
			return false;
		}

		private bool GetNextStringChunk()
		{
			int num = Math.Min(length - offset, 1024);
			string text = rawString.Substring(offset, num);
			TextIter iter = textView.Buffer.EndIter;
			textView.Buffer.Insert(ref iter, text);
			offset += num;
			if (offset < length)
			{
				return true;
			}
			idle_id = 0u;
			return false;
		}

		private void PopulateTextView(ObjectValue value)
		{
			EvaluationOptions evaluationOptions = DebuggingService.DebuggerSession.EvaluationOptions.Clone();
			evaluationOptions.AllowTargetInvoke = true;
			evaluationOptions.ChunkRawStrings = true;
			if (value.TypeName == "string")
			{
				rawString = value.GetRawValue(evaluationOptions) as RawValueString;
				length = rawString.Length;
				offset = 0;
				if (length <= 0)
				{
					return;
				}
				idle_id = Idle.Add(GetNextStringChunk);
				textView.Destroyed += delegate
				{
					if (idle_id != 0)
					{
						Source.Remove(idle_id);
						idle_id = 0u;
					}
				};
			}
			else
			{
				if (!(value.TypeName == "char[]"))
				{
					return;
				}
				rawArray = value.GetRawValue(evaluationOptions) as RawValueArray;
				length = rawArray.Length;
				offset = 0;
				if (length <= 0)
				{
					return;
				}
				idle_id = Idle.Add(GetNextCharArrayChunk);
				textView.Destroyed += delegate
				{
					if (idle_id != 0)
					{
						Source.Remove(idle_id);
						idle_id = 0u;
					}
				};
			}
		}

		public override Widget GetVisualizerWidget(ObjectValue val)
		{
			textView = new TextView
			{
				WrapMode = WrapMode.Char
			};
			ScrolledWindow scrolledWindow = new ScrolledWindow();
			scrolledWindow.HscrollbarPolicy = PolicyType.Automatic;
			scrolledWindow.VscrollbarPolicy = PolicyType.Automatic;
			scrolledWindow.ShadowType = ShadowType.In;
			ScrolledWindow scrolledWindow2 = scrolledWindow;
			scrolledWindow2.Add(textView);
			CheckButton check = new CheckButton(GettextCatalog.GetString("Wrap text"));
			check.Active = true;
			check.Toggled += delegate
			{
				if (check.Active)
				{
					textView.WrapMode = WrapMode.WordChar;
				}
				else
				{
					textView.WrapMode = WrapMode.None;
				}
			};
			VBox vBox = new VBox(homogeneous: false, 6);
			vBox.PackStart(scrolledWindow2, expand: true, fill: true, 0u);
			vBox.PackStart(check, expand: false, fill: false, 0u);
			vBox.ShowAll();
			PopulateTextView(val);
			return vBox;
		}

		public override bool StoreValue(ObjectValue val)
		{
			EvaluationOptions evaluationOptions = DebuggingService.DebuggerSession.EvaluationOptions.Clone();
			evaluationOptions.AllowTargetInvoke = true;
			switch (val.TypeName)
			{
			case "char[]":
				val.SetRawValue(textView.Buffer.Text.ToCharArray(), evaluationOptions);
				return true;
			case "string":
				val.SetRawValue(textView.Buffer.Text, evaluationOptions);
				return true;
			default:
				return false;
			}
		}

		public override bool CanEdit(ObjectValue val)
		{
			switch (val.TypeName)
			{
			case "char[]":
				return true;
			case "string":
				return true;
			default:
				return false;
			}
		}
	}
}
