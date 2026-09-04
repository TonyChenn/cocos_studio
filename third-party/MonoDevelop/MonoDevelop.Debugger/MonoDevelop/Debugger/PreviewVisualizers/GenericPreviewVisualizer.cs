using System;
using Gdk;
using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Components;
using Pango;

namespace MonoDevelop.Debugger.PreviewVisualizers
{
	public class GenericPreviewVisualizer : PreviewVisualizer
	{
		public override bool CanVisualize(ObjectValue val)
		{
			throw new InvalidOperationException();
		}

		public override Control GetVisualizerWidget(ObjectValue val)
		{
			string text = val.Value;
			Gdk.Color color = new Gdk.Color(85, 85, 85);
			if (!val.IsNull && (val.TypeName == "string" || val.TypeName == "char[]"))
			{
				text = '"' + GetString(val) + '"';
			}
			if (DebuggingService.HasInlineVisualizer(val))
			{
				text = DebuggingService.GetInlineVisualizer(val).InlineVisualize(val);
			}
			Label label = new Label(text);
			FontDescription fontDescription = label.Style.FontDescription.Copy();
			if (fontDescription.SizeIsAbsolute)
			{
				fontDescription.AbsoluteSize = fontDescription.Size - 1;
			}
			else
			{
				fontDescription.Size -= (int)Pango.Scale.PangoScale;
			}
			label.ModifyFont(fontDescription);
			label.ModifyFg(StateType.Normal, color);
			label.SetPadding(4, 4);
			if (label.SizeRequest().Width > 500)
			{
				label.WidthRequest = 500;
				label.Wrap = true;
				label.LineWrapMode = Pango.WrapMode.WordChar;
			}
			else
			{
				label.Justify = Justification.Center;
			}
			if (label.Layout.GetLine(1) != null)
			{
				label.Justify = Justification.Left;
				LayoutLine line = label.Layout.GetLine(15);
				if (line != null)
				{
					label.Text = text.Substring(0, line.StartIndex).TrimEnd('\r', '\n') + "\n…";
				}
			}
			label.Show();
			return label;
		}

		private string GetString(ObjectValue val)
		{
			EvaluationOptions evaluationOptions = DebuggingService.DebuggerSession.EvaluationOptions.Clone();
			evaluationOptions.AllowTargetInvoke = true;
			evaluationOptions.ChunkRawStrings = true;
			if (val.TypeName == "string")
			{
				RawValueString rawValueString = val.GetRawValue(evaluationOptions) as RawValueString;
				int length = rawValueString.Length;
				if (length > 0)
				{
					return rawValueString.Substring(0, Math.Min(length, 4096));
				}
				return "";
			}
			if (val.TypeName == "char[]")
			{
				RawValueArray rawValueArray = val.GetRawValue(evaluationOptions) as RawValueArray;
				int length2 = rawValueArray.Length;
				if (length2 > 0)
				{
					return new string(rawValueArray.GetValues(0, Math.Min(length2, 4096)) as char[]);
				}
				return "";
			}
			throw new InvalidOperationException();
		}
	}
}
