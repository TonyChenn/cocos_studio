using System;

namespace CocoStudio.Model.ViewModel
{
	public class SpeedChangedArgs : EventArgs
	{
		public float Speed { get; private set; }

		public SpeedChangedArgs(float speed)
		{
			this.Speed = speed;
		}
	}
}
