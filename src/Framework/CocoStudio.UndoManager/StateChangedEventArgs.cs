using System;

namespace CocoStudio.UndoManager
{
	public class StateChangedEventArgs : EventArgs
	{
		public string PropertyName { get; private set; }

		public object OldValue { get; private set; }

		public object NewValue { get; private set; }

		public bool IsProvideValue { get; private set; }

		public StateChangedEventArgs(string propertyName)
		{
			this.PropertyName = propertyName;
		}

		public StateChangedEventArgs(string propertyName, object oldValue, object newValue)
		{
			this.PropertyName = propertyName;
			this.OldValue = oldValue;
			this.NewValue = newValue;
			this.IsProvideValue = true;
		}
	}
}
