using System;

namespace CocoStudio.Lib.Prism
{
	public class DataEventArgs<TData> : EventArgs
	{
		public DataEventArgs(TData value)
		{
			this._value = value;
		}

		public TData Value
		{
			get
			{
				return this._value;
			}
		}

		private readonly TData _value;
	}
}
