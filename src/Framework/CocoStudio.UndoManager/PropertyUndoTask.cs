using System;
using System.Reflection;
using CocoStudio.UndoManager.Recorder;

namespace CocoStudio.UndoManager
{
	public class PropertyUndoTask : UndoTask
	{
		public object NewValue { get; private set; }

		public object OldValue { get; private set; }

		public string PropertyName
		{
			get
			{
				return this.prop.Name;
			}
		}

		public PropertyUndoTask(DefaultRecorder recorder, object sender, PropertyInfo prop, object oldValue, object newValue) : base(recorder)
		{
			this.objectItem = sender;
			this.prop = prop;
			this.OldValue = oldValue;
			this.NewValue = newValue;
			this.execute = new Action<object>(this.Redoing);
			this.unExecute = new Action<object>(this.Undoing);
		}

		private void Undoing(object temp)
		{
			this.SetPropertyValue(this.OldValue);
		}

		private void Redoing(object temp)
		{
			this.SetPropertyValue(this.NewValue);
		}

		private void SetPropertyValue(object value)
		{
			this.prop.SetValue(this.objectItem, value, DefaultRecorder.EmptyArray);
		}

		public override string DescriptionForUser
		{
			get
			{
				return "PropertyUndoTask";
			}
		}

		~PropertyUndoTask()
		{
			this.Dispose();
		}

		public override void Dispose()
		{
			this.objectItem = null;
			this.prop = null;
			this.NewValue = null;
			this.OldValue = null;
			GC.SuppressFinalize(this);
			base.Dispose();
		}

		private object objectItem;

		private PropertyInfo prop;
	}
}
