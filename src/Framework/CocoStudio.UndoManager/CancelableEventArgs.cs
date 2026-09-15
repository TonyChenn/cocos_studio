using System;

namespace CocoStudio.UndoManager
{
	public class CancelableEventArgs<TPayload>
	{
		public bool Cancel
		{
			get
			{
				return this.cancel;
			}
			set
			{
				if (value)
				{
					this.cancel = true;
				}
			}
		}

		public TPayload Payload { get; set; }

		public CancelableEventArgs(TPayload payload)
		{
			this.Payload = payload;
		}

		private bool cancel;
	}
}
