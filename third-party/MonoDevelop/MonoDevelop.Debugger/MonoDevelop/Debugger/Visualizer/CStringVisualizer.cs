using System;
using System.Text;
using GLib;
using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Core;

namespace MonoDevelop.Debugger.Visualizer
{
	public class CStringVisualizer : ValueVisualizer
	{
		private const int CHUNK_SIZE = 1024;

		private RawValueArray rawArray;

		private TextView textView;

		private uint idle_id;

		private int length;

		private int offset;

		public override string Name => GettextCatalog.GetString("C String");

		public override bool CanVisualize(ObjectValue val)
		{
			switch (val.TypeName)
			{
			case "sbyte[]":
				return true;
			case "byte[]":
				return true;
			default:
				return false;
			}
		}

		private static void AppendByte(StringBuilder text, byte c)
		{
			switch (c)
			{
			case 0:
				text.Append("\\0");
				return;
			case 7:
				text.Append("\\a");
				return;
			case 8:
				text.Append("\\b");
				return;
			case 9:
				text.Append("\\t");
				return;
			case 10:
				text.Append("\\n");
				return;
			case 11:
				text.Append("\\v");
				return;
			case 13:
				text.Append("\\r");
				return;
			}
			if (c < 20 || c > 126)
			{
				text.AppendFormat("\\x{0:x,2}", c);
			}
			else
			{
				text.Append((char)c);
			}
		}

		private static string ByteArrayToCString(byte[] buf)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < buf.Length; i++)
			{
				AppendByte(stringBuilder, buf[i]);
			}
			return stringBuilder.ToString();
		}

		private static string SByteArrayToCString(sbyte[] buf)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < buf.Length; i++)
			{
				AppendByte(stringBuilder, (byte)buf[i]);
			}
			return stringBuilder.ToString();
		}

		private bool GetNextSByteArrayChunk()
		{
			int num = Math.Min(length - offset, 1024);
			sbyte[] buf = rawArray.GetValues(offset, num) as sbyte[];
			string text = SByteArrayToCString(buf);
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

		private bool GetNextByteArrayChunk()
		{
			int num = Math.Min(length - offset, 1024);
			byte[] buf = rawArray.GetValues(offset, num) as byte[];
			string text = ByteArrayToCString(buf);
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
			rawArray = value.GetRawValue(evaluationOptions) as RawValueArray;
			length = rawArray.Length;
			offset = 0;
			if (length <= 0)
			{
				return;
			}
			switch (value.TypeName)
			{
			default:
				return;
			case "sbyte[]":
				idle_id = Idle.Add(GetNextSByteArrayChunk);
				break;
			case "byte[]":
				idle_id = Idle.Add(GetNextByteArrayChunk);
				break;
			}
			textView.Destroyed += delegate
			{
				if (idle_id != 0)
				{
					Source.Remove(idle_id);
					idle_id = 0u;
				}
			};
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
	}
}
