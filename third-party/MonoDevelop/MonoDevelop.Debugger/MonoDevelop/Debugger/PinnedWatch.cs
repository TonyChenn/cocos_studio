using Mono.Debugging.Backend;
using Mono.Debugging.Client;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects;

namespace MonoDevelop.Debugger
{
	public class PinnedWatch
	{
		[ProjectPathItemProperty]
		private FilePath file;

		[ItemProperty]
		private int line;

		[ItemProperty(DefaultValue = 0)]
		private int offsetX;

		[ItemProperty(DefaultValue = 0)]
		private int offsetY;

		[ItemProperty]
		private string expression;

		[ItemProperty]
		private bool liveUpdate;

		private ObjectValue value;

		private bool evaluated;

		internal Breakpoint BoundTracer;

		internal PinnedWatchStore Store { get; set; }

		public FilePath File
		{
			get
			{
				return file;
			}
			set
			{
				file = value;
				NotifyChanged();
			}
		}

		public bool LiveUpdate
		{
			get
			{
				return liveUpdate;
			}
			internal set
			{
				liveUpdate = value;
			}
		}

		public int Line
		{
			get
			{
				return line;
			}
			set
			{
				line = value;
				NotifyChanged();
			}
		}

		public int OffsetX
		{
			get
			{
				return offsetX;
			}
			set
			{
				offsetX = value;
				NotifyChanged();
			}
		}

		public int OffsetY
		{
			get
			{
				return offsetY;
			}
			set
			{
				offsetY = value;
				NotifyChanged();
			}
		}

		public ObjectValue Value
		{
			get
			{
				if (!evaluated)
				{
					Evaluate(notify: false);
				}
				return value;
			}
			set
			{
				evaluated = true;
				this.value = value;
				value.Name = expression;
				NotifyChanged();
			}
		}

		public string Expression
		{
			get
			{
				return expression;
			}
			set
			{
				if (expression != value)
				{
					evaluated = false;
					expression = value;
					NotifyChanged();
				}
			}
		}

		internal void Evaluate(bool notify)
		{
			if (DebuggingService.CurrentFrame != null)
			{
				evaluated = true;
				value = DebuggingService.CurrentFrame.GetExpressionValue(expression, evaluateMethods: true);
				value.Name = expression;
				if (notify)
				{
					NotifyChanged();
				}
			}
		}

		internal void UpdateFromTrace(string trace)
		{
			EvaluationResult evaluationResult = new EvaluationResult(trace);
			ObjectValueFlags flags = ObjectValueFlags.Primitive | ObjectValueFlags.Field;
			string typeName = "";
			if (value != null)
			{
				flags = value.Flags;
				typeName = value.TypeName;
			}
			value = ObjectValue.CreatePrimitive(null, new ObjectPath(Expression), typeName, evaluationResult, flags);
			evaluated = true;
			NotifyChanged();
		}

		internal void Invalidate()
		{
			value = null;
			evaluated = false;
			NotifyChanged();
		}

		internal void LoadValue(ObjectValue val)
		{
			value = val;
			value.Name = expression;
			evaluated = true;
		}

		private void NotifyChanged()
		{
			if (Store != null)
			{
				Store.NotifyWatchChanged(this);
			}
		}
	}
}
