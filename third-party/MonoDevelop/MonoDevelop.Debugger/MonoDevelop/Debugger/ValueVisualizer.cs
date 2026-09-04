using Gtk;
using Mono.Debugging.Client;

namespace MonoDevelop.Debugger
{
	public abstract class ValueVisualizer
	{
		public abstract string Name { get; }

		public abstract bool CanVisualize(ObjectValue val);

		public virtual bool IsDefaultVisualizer(ObjectValue val)
		{
			return false;
		}

		public abstract Widget GetVisualizerWidget(ObjectValue val);

		public virtual bool StoreValue(ObjectValue val)
		{
			return false;
		}

		public virtual bool CanEdit(ObjectValue val)
		{
			return false;
		}
	}
}
