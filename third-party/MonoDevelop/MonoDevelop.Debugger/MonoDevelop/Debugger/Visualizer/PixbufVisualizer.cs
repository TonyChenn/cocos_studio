using System.IO;
using Gdk;
using Gtk;
using Mono.Debugging.Client;

namespace MonoDevelop.Debugger.Visualizer
{
	public class PixbufVisualizer : ValueVisualizer
	{
		public override string Name => "Pixbuf";

		public override bool CanVisualize(ObjectValue val)
		{
			return val.TypeName == "Gdk.Pixbuf";
		}

		public override bool IsDefaultVisualizer(ObjectValue val)
		{
			return true;
		}

		public override Widget GetVisualizerWidget(ObjectValue val)
		{
			EvaluationOptions evaluationOptions = DebuggingService.DebuggerSession.EvaluationOptions.Clone();
			string tempFileName = Path.GetTempFileName();
			evaluationOptions.AllowTargetInvoke = true;
			Pixbuf pixbuf;
			try
			{
				RawValue rawValue = (RawValue)val.GetRawValue(evaluationOptions);
				rawValue.CallMethod("Save", tempFileName, "png");
				pixbuf = new Pixbuf(tempFileName);
			}
			finally
			{
				File.Delete(tempFileName);
			}
			ScrolledWindow scrolledWindow = new ScrolledWindow();
			scrolledWindow.ShadowType = ShadowType.In;
			scrolledWindow.HscrollbarPolicy = PolicyType.Automatic;
			scrolledWindow.VscrollbarPolicy = PolicyType.Automatic;
			Gtk.Image child = new Gtk.Image(pixbuf);
			scrolledWindow.AddWithViewport(child);
			scrolledWindow.ShowAll();
			return scrolledWindow;
		}
	}
}
